using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class ItineraryDescription
    {
        public ItineraryDescription()
        {
            ItineraryName = null;
            ItineraryVersion = null;
            WasItineraryInCache = null;
        }

        public string ItineraryName { get; set; }
        public string ItineraryVersion { get; set; }
        public bool? WasItineraryInCache { get; set; }
    }
}
