using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    internal class EsbMessageHandlerFactory
    {
        public static IEsbMessageHandler CreateHander(ChannelEndpointElement channel) 
        {
            bool isGenericItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay", StringComparison.CurrentCultureIgnoreCase) != -1);
            bool isStaticItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay", StringComparison.CurrentCultureIgnoreCase) != -1);
            bool isExceptionHandlingEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance", StringComparison.CurrentCultureIgnoreCase) != -1);
            bool isQueuedContract = (channel.Contract.IndexOf("Queued", StringComparison.CurrentCultureIgnoreCase) != -1);

            if (isGenericItineraryEndpoint)
            {
                if (isQueuedContract)
                {
                    return new GenericItineraryQueuedEsbMessageHandler(channel.Name);
                }
                else
                {
                    return new GenericItineraryEsbMessageHandler(channel.Name);
                }
            }
            else if (isStaticItineraryEndpoint)
            {
                if (isQueuedContract)
                {
                    return new StaticItineraryQueuedEsbMessageHandler(channel.Name);
                }
                else
                {
                    return new StaticItineraryEsbMessageHandler(channel.Name);
                }
            }
            else if (isExceptionHandlingEndpoint)
            {
                if (isQueuedContract)
                {
                    return new ExceptionQueuedEsbMessageHandler(channel.Name);
                }
                else
                {
                    return new ExceptionEsbMessageHandler(channel.Name);
                }
            }
            else
                throw new MessagingConfigurationException("Error configuring BizTalk ESB Endpoint.  The channel endpoint does not appear to support a know ESB messaging contract.");
        }
    }
}
