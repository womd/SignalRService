using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SignalRService.Localization
{
    public class BaseResource
    {
        public static string Get(string key)
        {
            UiResources ui = new UiResources();
            return ui.GetResourceValueFromDb(key);
        }

        public static void removeFromCache(string key, string culture)
        {
            UiResources ui = new UiResources();
            ui.removeFromCache(key, culture);
        }

        public static void clearCache()
        {
            UiResources ui = new UiResources();
            ui.clearCache();
        }
        public static string CreateLocalizationSrc(DAL.ServiceContext dbcontext, string filePath)
        {
            var litems = dbcontext.Localization.ToList();
            Console.WriteLine("\nBuilding LocalizationSeedingFile ...(" + litems.Count + " items) ");
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<Models.LocalizationModel>));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                    xsSubmit.Serialize(writer, litems);
                    xml = sww.ToString();
                }
            }

            // write to file
            using (var writer = File.CreateText(filePath))
            {
                writer.WriteLine(xml);
                writer.Flush();
                writer.Close();
            }

            return filePath;
        }
    }
}