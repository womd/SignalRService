using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using SignalRService.Models;

namespace SignalRService.Localization
{
    public class UiResources
    {
        private DAL.ServiceContext db;
        private Dictionary<string, string> cache;
        private static readonly object lockobj = new object();
        private static UiResources instance;
        public UiResources()
        {
            cache = new Dictionary<string, string>();
            db = db = new DAL.ServiceContext();
        }

        private string buildCacheKey(string key, string culture)
        {
            return string.Format("{0}:{1}", key, culture);
        }


        public static UiResources Instance
        {
            get
            {
                //doublecheck locking   
                if (instance == null)
                {
                    lock (lockobj)
                    {
                        if (instance == null)
                        {
                            instance = new UiResources();
                        }
                    }
                }
                return instance;
            }
        }

        public bool KeyExists(string skey)
        {
            return db.Localization.Any(ln => ln.Key == skey);
        }

        public bool KeyExists(string Key, string CultueName)
        {
            return db.Localization.Any(ln => ln.Key == Key && ln.Culture == CultueName);
        }

      

        public string GetResourceValueFromDb(string sKey)
        {
            string resourceValue = string.Empty;
            string culture = Utils.CultureHelper.GetImplementedCulture(CultureInfo.CurrentCulture.Name);

            if (cache.ContainsKey(buildCacheKey(sKey, culture)))
                cache.TryGetValue(buildCacheKey(sKey, culture), out resourceValue);
            else
            {
                db = new DAL.ServiceContext();
                Models.LocalizationModel localization = db.Localization.FirstOrDefault(l => l.Key == sKey && l.Culture == CultureInfo.CurrentCulture.Name);
                resourceValue = localization != null ? localization.Value : string.Format("{0}(!!)", sKey);
                cache.Add(buildCacheKey(sKey, culture), resourceValue);

                if (localization != null && localization.WasHit == false)
                {
                    localization.WasHit = true;
                    db.SaveChanges();
                }
            }
            return resourceValue;
        }

        public void removeFromCache(string key, string culture)
        {
            if (Instance.cache.ContainsKey(buildCacheKey(key, culture)))
                Instance.cache.Remove(buildCacheKey(key, culture));
        }

        public void clearCache()
        {
            Instance.cache = new Dictionary<string, string>();
        }

    }
}