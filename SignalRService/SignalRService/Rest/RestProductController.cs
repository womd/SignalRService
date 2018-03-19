using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SignalRService.Utils;
using SignalRService.ViewModels;

namespace SignalRService.Rest
{
   
    public class RestroductController : ApiController
    {

        private Repositories.ProductRepository productRepository;

        public RestroductController()
        {
            productRepository = new Repositories.ProductRepository(new DAL.ServiceContext());
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("api/p/create")]
        public HttpResponseMessage CreateProduct([FromUri] ProductViewModel model)
        {
            // http://localhost:64000/api/p/create?Id=0&PartNumber=xxx11xx&Name=testname&serialnumber=E5L0372977&partnumber=16911639

            var product = productRepository.CreateProduct(model);
            return Request.CreateResponse(product.ToProductViewModel());
        }

    }
}
