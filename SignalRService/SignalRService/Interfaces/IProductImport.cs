using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Interfaces
{
    public interface IProductImport
    {
        bool LoadSourceToTmpStore(string src, int ownerId);
        bool CreateProductsFromTmpStore(UserDataViewModel user);
    }
}
