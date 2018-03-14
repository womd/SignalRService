using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Hubs;
using SignalRService.Models;
using SignalRService.ViewModels;

namespace SignalRService.DAL
{
    /// <summary>
    /// implements lazy singleton for datastore of connected clients
    /// </summary>
    public sealed class SignalRConnections
    {
        static readonly Lazy<SignalRConnections> lazy = new Lazy<SignalRConnections>(() => new SignalRConnections());
        private SignalRConnections() {
            _data = new Dictionary<string, UserDataModel>();
        }

        public static SignalRConnections Instance => lazy.Value;

        private IDictionary<string,UserDataModel> _data;
       
        public List<UserDataViewModel> List()
        {
            List<UserDataViewModel> result = new List<UserDataViewModel>();
            foreach(var item in _data)
            {
                var mstat = new MinerStatusData();
                if(item.Value.MinerStatus != null)
                    mstat =(MinerStatusData) item.Value.MinerStatus;

                result.Add(new UserDataViewModel() {
                    ConnectionId = item.Key,
                    ConnectionState = item.Value.ConnectionState.ToString(),
                    RefererUrl = item.Value.RefererUrl,
                    RemoteIp = item.Value.RemoteIp,
                    UserId = item.Value.UserId,
                    UserName = item.Value.UserId,
                    NrOfGroups = item.Value.Groups.Count,

                    MinerIsRunning = mstat.running,
                    MinerIsMobile = mstat.onMobile,
                    MinerHps = mstat.hps,
                    MinerThrottle = mstat.throttle
                    
                });
            }
            return result;
        }

        public void GroupAddOrUpdate(string ConnectionId, string Group)
        {
            if (_data.TryGetValue(ConnectionId, out UserDataModel userData))
            {
                if(!userData.Groups.Contains(Group))
                    userData.Groups.Add(Group);
            }
        }

        public void GroupRemove(string ConnectionId, string Group)
        {
            if (_data.TryGetValue(ConnectionId, out UserDataModel userData))
            {
                _data.Remove(ConnectionId);
            }
        }

        public void UpdateMinerStatusData(string ConnectionId, MinerStatusData Data)
        {
            if (_data.TryGetValue(ConnectionId, out UserDataModel userData))
            {
                userData.MinerStatus = Data;
            }
        }

        public void AddOrUpdate(string ConnectionId, Enums.EnumSignalRConnectionState ConnectionState, string refererUrl = "", string remoteIp = "", string userid = "")
        {
            if (_data.TryGetValue(ConnectionId, out UserDataModel userData))
            {
                userData.ConnectionState = ConnectionState;
                if(!string.IsNullOrEmpty(remoteIp))
                    userData.RemoteIp = remoteIp;
                if(!string.IsNullOrEmpty(refererUrl))
                    userData.RefererUrl = refererUrl;
                if (!string.IsNullOrEmpty(userid))
                    userData.UserId = userid;
            }
            else
            {
                _data.Add(ConnectionId, new UserDataModel()
                {
                    ConnectionState = ConnectionState,
                    RefererUrl = refererUrl,
                    RemoteIp = remoteIp,
                    UserId = userid
                });
            }
        }

        public void Remove(string ConnectionId)
        {
            if (_data.ContainsKey(ConnectionId))
                _data.Remove(ConnectionId);
        }
    }



   
}