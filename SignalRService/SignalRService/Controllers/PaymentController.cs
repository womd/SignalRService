using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class PaymentController : BaseController
    {
        private DAL.ServiceContext db;
        private Repositories.OrderRepository orderRepository;

        public PaymentController()
        {
            db = new DAL.ServiceContext();
            orderRepository = new Repositories.OrderRepository(db);
        }

        private bool StripeCharge(string stipeEmail, string stripeToken, float amount, string currency, string descr,  string secretkey)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(secretkey);

            var options = new StripeChargeCreateOptions
            {
                Amount = int.Parse( (amount * 100).ToString() ),
                Currency = currency,
                Description = descr,
                SourceTokenOrExistingSourceId = stripeToken,
            };
            var service = new StripeChargeService();
            StripeCharge charge = service.Create(options);
            if (charge.Captured == true)
                return true;

            return false;
           
        }

        public ActionResult StripePay(string tokenid, string orderid)
        {
            var order = orderRepository.GetOrder(orderid);
            var amount = Utils.OrderUtils.GetToals(orderid);

            var service = db.ServiceSettings.FirstOrDefault(ln => ln.Owner.ID == order.StoreUser.Id);
            var stripesettings = service.StripeSettings.FirstOrDefault();

            var res = StripeCharge(order.CustomerUser.Name, tokenid, amount, "eur", order.OrderIdentifier, stripesettings.SecretKey);
            
            //get process implementation - set paystate ...

            return Json("tadaaa...", JsonRequestBehavior.AllowGet);
        }

    }
}