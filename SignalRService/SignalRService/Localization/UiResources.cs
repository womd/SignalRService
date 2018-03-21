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
        private readonly DAL.ServiceContext db = new DAL.ServiceContext();
        private Dictionary<string, string> cache;
        private static readonly object lockobj = new object();
        private static UiResources instance;
        public UiResources()
        {
            cache = new Dictionary<string, string>();
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

        public string GetResourceValueFromDb(string sKey)
        {
            string resourceValue = string.Empty;

            if (cache.ContainsKey(buildCacheKey(sKey, CultureInfo.CurrentCulture.Name)))
                cache.TryGetValue(buildCacheKey(sKey, CultureInfo.CurrentCulture.Name), out resourceValue);
            else
            {
                Models.LocalizationModel localization = db.Localization.FirstOrDefault(l => l.Key.Equals(sKey) && l.Culture.Equals(CultureInfo.CurrentCulture.Name));
                resourceValue = localization != null ? localization.Value : string.Format("{0}(!!)", sKey);
                cache.Add(buildCacheKey(sKey, CultureInfo.CurrentCulture.Name), resourceValue);

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
            if (cache.ContainsKey(buildCacheKey(key, culture)))
                cache.Remove(buildCacheKey(key, CultureInfo.CurrentCulture.Name));
        }

        public void clearCache()
        {
            cache = new Dictionary<string, string>();
        }

    }
}