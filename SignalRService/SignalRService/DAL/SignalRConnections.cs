using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;

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
                result.Add(new UserDataViewModel() {
                    ConnectionId = item.Key,
                    ConnectionState = item.Value.ConnectionState.ToString(),
                    RefererUrl = item.Value.RefererUrl,
                    RemoteIp = item.Value.RemoteIp,
                    UserId = item.Value.UserId,
                    UserName = item.Value.UserId
                });
            }
            return result;
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