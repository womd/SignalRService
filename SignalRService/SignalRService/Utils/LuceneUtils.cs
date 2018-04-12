using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using SignalRService.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
   /// https://www.codeproject.com/Articles/320219/Lucene-Net-ultra-fast-search-for-MVC-or-WebForms
    public static class LuceneUtils
    {
        private static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");
        private static FSDirectory _directoryTemp;
        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        private static void _addToLuceneIndex(ProductImportModel productData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", productData.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", productData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("SrcId", productData.SrcId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Title", productData.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", productData.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Brand", productData.Brand, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Gtin", productData.Gtin, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Mpn", productData.Mpn, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("OwnerString", productData.OwnerIdString, Field.Store.YES, Field.Index.ANALYZED));
          
            // add entry to index
            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<ProductImportModel> productDatas)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var productData in productDatas) _addToLuceneIndex(productData, writer);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(ProductImportModel productData)
        {
            AddUpdateLuceneIndex(new List<ProductImportModel> { productData });
        }

        public static void ClearLuceneIndexRecord(int record_id)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entries
                writer.DeleteAll();

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        private static ProductModel _mapLuceneDocumentToData(Document doc)
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            string srcId = doc.Get("SrcId");
            var db_product = db.Products.FirstOrDefault(ln => ln.SrcIdentifier == srcId);
            if(db_product == null)
            {
                db_product = new ProductModel() { ID = 0, Name = "lucene_map_error" };
            }
            return db_product;
        }

        private static IEnumerable<ProductModel> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<ProductModel> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }
        private static IEnumerable<ProductModel> _search (string searchQuery, int UserId, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<ProductModel>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 20;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                Lucene.Net.Search.TermRangeFilter filter = new TermRangeFilter("OwnerIdString", UserId.ToString(), UserId.ToString(), true, true);


                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);

                    ScoreDoc[] hits;
                    hits = searcher.Search(query, hits_limit).ScoreDocs;
                    //if (UserId != 0)
                    //    hits = searcher.Search(query, filter, hits_limit).ScoreDocs;
                    //else
                    //    hits = searcher.Search(query, hits_limit).ScoreDocs;

                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "SrcId", "Title", "Description", "Brand", "Gtin", "Mpn" }, analyzer);
                    var query = parseQuery(searchQuery, parser);

                    ScoreDoc[] hits;
                    hits = searcher.Search(query, hits_limit).ScoreDocs;

                    //if (UserId != 0)
                    //    hits = searcher.Search(query, filter, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    //else
                    //    hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;

                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
        public static IEnumerable<ProductModel> Search(string input, int userID, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<ProductModel>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return _search(input, userID, fieldName);
        }

        public static IEnumerable<ProductModel> SearchDefault(string input, int UserId, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<ProductModel>() : _search(input, UserId, fieldName);
        }

        public static IEnumerable<ProductModel> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<ProductModel>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }
    }
}