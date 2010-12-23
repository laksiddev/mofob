using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal interface IMessageItineraryMapper 
    {
        ItineraryDescription MapMessageToItinerary(SimpleMessage message);
    }
}
