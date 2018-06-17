using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                Description = "*** Welcome to your new Room ***",
                ShowControls = true
            };

            parentService.MiningRooms.Add(newRoom);
            _db.SaveChanges();

            return newRoom;
        }

    }


}