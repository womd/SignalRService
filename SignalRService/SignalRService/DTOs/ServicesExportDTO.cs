using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.DTOs
{
    public class ServicesTransferDTO
    {
        #region userdata

        public string IdentityName { get; set; }
        public string XMRWalletAddress { get; set; }

        #endregion

        #region servicedata

        public string ServiceUrl { get; set; }
        public string ServiceName { get; set; }
        public int ServiceType { get; set; }

        #endregion

        #region minerconf

        public string ScriptUrl { get; set; }
        public string ClientId { get; set; }
        public float Throttle { get; set; }
        public int StartDelayMs { get; set; }
        public int ReportStatusIntervalMs { get; set; }

        #endregion

        #region miningRoom

        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowControls { get; set; }

        #endregion
    }
}