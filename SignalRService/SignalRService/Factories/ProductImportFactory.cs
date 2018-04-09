using SignalRService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Factories
{
    public class ProductImportFactory
    {
        public static IProductImport GetProductImportImplementation(Enums.EnumImportType importType)
        {
            switch (importType)
            {
                case Enums.EnumImportType.GoogleProductXML:
                    return new Implementation.ProductImportGoogleMerchantXML();
                 default:
                    return null;
            }

        }
    }
}