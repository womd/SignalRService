using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;

namespace SignalRService.Repositories
{
    public class SignalRConnectionContext
    {
        private readonly DAL.ServiceContext _db;
        public SignalRConnectionContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public void RemoveConnections(List<Models.SignalRConnectionModel>connections)
        {
            if (connections.Count == 0)
                return;

            var dependentMinerstati = new List<Models.MinerStatusModel>();
            var dependentGroups = new List<Models.SignalRGroupsModel>();
            foreach (var ditem in connections)
            {
                if (ditem.MinerStatus != null)
                    dependentMinerstati.Add(ditem.MinerStatus);

            //    if (ditem.Groups != null)
            //        dependentGroups.AddRange(ditem.Groups);

                        
            }

          //  _db.SignalRGroups.RemoveRange(dependentGroups);
            _db.MinerStatus.RemoveRange(dependentMinerstati);
            _db.SignalRConnections.RemoveRange(connections);
            _db.SaveChanges();
        }

        public void RemoveConnection(string ConnectionId)
        {
            var conn = _db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == ConnectionId);
            _db.SignalRConnections.Remove(conn);
        }

    }
}