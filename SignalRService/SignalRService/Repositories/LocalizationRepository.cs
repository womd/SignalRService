using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class LocalizationRepository
    {
        private DAL.ServiceContext db;

        public LocalizationRepository(DAL.ServiceContext _db)
        {
            db = _db;
        }

        public Models.LocalizationModel Create(Models.LocalizationModel model, string ModUser)
        {
            var dbobj = db.Localization.Add(new Models.LocalizationModel()
            {
                Key = model.Key,
                Value = model.Value,
                Culture = model.Culture,
                LastModDate = DateTime.Now,
                ModUser = ModUser,
                TranslationStatus = Enums.EnumTranslationStatus.Undefined,
                WasHit = false
            });
            db.SaveChanges();
            return dbobj;
        }

        public Models.LocalizationModel Update(Models.LocalizationModel model, string ModUser)
        {
            var freshObj = db.Localization.FirstOrDefault(ln => ln.ID == model.ID);
            freshObj.Culture = model.Culture;
            freshObj.Key = model.Key;
            freshObj.Value = model.Value;
            freshObj.LastModDate = DateTime.Now;
            freshObj.ModUser = ModUser;
            freshObj.TranslationStatus = Enums.EnumTranslationStatus.Approved;
            freshObj.WasHit = false;
            db.SaveChanges();

            return freshObj;
        }

        public bool Delete(int Id)
        {
            var rmObj = db.Localization.FirstOrDefault(ln => ln.ID == Id);
            db.Localization.Remove(rmObj);
            db.SaveChanges();
            return true;
        }

        public bool Exists(string Key, string Culture)
        {
            return db.Localization.Any(ln => ln.Key == Key && ln.Culture == Culture);
        }

        public Models.LocalizationModel Get(string Key, string Culture)
        {
            return db.Localization.FirstOrDefault(ln => ln.Key == Key && ln.Culture == Culture);
        }
    }

   
}