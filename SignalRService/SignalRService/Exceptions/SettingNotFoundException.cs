using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Exceptions
{
    
        [Serializable()]
        public class SettingNotFoundException : System.Exception
        {
            public SettingNotFoundException() : base() { }
            public SettingNotFoundException(string message) : base(message) { }
            public SettingNotFoundException(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an
            // exception propagates from a remoting server to the client. 
            protected SettingNotFoundException(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
            { }
        }

    
}