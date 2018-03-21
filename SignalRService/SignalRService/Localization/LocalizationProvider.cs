using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Localization
{
    public class LocalizationProvider : DataAnnotationsModelMetadataProvider
    {

        protected override ModelMetadata CreateMetadata(
                             IEnumerable<Attribute> attributes,
                             Type containerType,
                             Func<object> modelAccessor,
                             Type modelType,
                             string propertyName)
        {

            string sKey = string.Empty;
            string sLocalizedText = string.Empty;

            HttpContext.Current.Application.Lock();
            foreach (var attr in attributes)
            {
                if (attr != null)
                {
                    string typeName = attr.GetType().Name;
                    string attrAppKey = string.Empty;

                    if (typeName.Equals("DisplayAttribute"))
                    {
                        sKey = ((DisplayAttribute) attr).Name;

                        if (!string.IsNullOrEmpty(sKey))
                        {
                            attrAppKey = string.Format("{0}-{1}-{2}",
                            containerType.Name, propertyName, typeName);
                            if (HttpContext.Current.Application[attrAppKey] == null)
                            {
                                HttpContext.Current.Application[attrAppKey] = sKey;
                            }
                            else
                            {
                                sKey = HttpContext.Current.Application[attrAppKey].ToString();
                            }

                            sLocalizedText = UiResources.Instance.GetResourceValueFromDb(sKey);
                            if (string.IsNullOrEmpty(sLocalizedText))
                            {
                                sLocalizedText = sKey;
                            }

                            ((DisplayAttribute) attr).Name = sLocalizedText;
                        }
                    }
                    else if (attr is ValidationAttribute)
                    {
                        sKey = ((ValidationAttribute) attr).ErrorMessage;

                        if (!string.IsNullOrEmpty(sKey))
                        {
                            attrAppKey = string.Format("{0}-{1}-{2}",
                            containerType.Name, propertyName, typeName);
                            if (HttpContext.Current.Application[attrAppKey] == null)
                            {
                                HttpContext.Current.Application[attrAppKey] = sKey;
                            }
                            else
                            {
                                sKey = HttpContext.Current.Application[attrAppKey].ToString();
                            }

                            sLocalizedText = UiResources.Instance.GetResourceValueFromDb(sKey);
                            if (string.IsNullOrEmpty(sLocalizedText))
                            {
                                sLocalizedText = sKey;
                            }

                            ((ValidationAttribute) attr).ErrorMessage = sLocalizedText;
                        }
                    }
                }
            }
            HttpContext.Current.Application.UnLock();

            return base.CreateMetadata
              (attributes, containerType, modelAccessor, modelType, propertyName);
        }
    }
}