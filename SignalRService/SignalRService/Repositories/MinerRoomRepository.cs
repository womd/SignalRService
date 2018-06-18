using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Localization;

namespace SignalRService.Repositories
{
    public class MinerRoomRepository
    {

        private Repositories.MinerRoomContext roomContext;
        private DAL.ServiceContext _db;

        public MinerRoomRepository(DAL.ServiceContext db)
        {
            _db = db;
            roomContext = new MinerRoomContext(db);
        }

        public Models.MiningRoomModel CreateRoom(Models.ServiceSettingModel parentService)
        {
            if (parentService.MiningRooms == null)
                parentService.MiningRooms = new List<Models.MiningRoomModel>();

            var newRoom = new Models.MiningRoomModel()
            {
                Name = parentService.ServiceName,
                Description = SignalRService.Localization.BaseResource.Get("NewMiningRoomDefaultDescription"),
                ShowControls = true
            };

            parentService.MiningRooms.Add(newRoom);
            _db.ServiceSettings.Add(parentService);
            _db.SaveChanges();

            return newRoom;
        }

    }


}