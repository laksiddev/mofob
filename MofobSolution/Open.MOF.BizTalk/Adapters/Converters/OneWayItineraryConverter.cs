using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters
{
    public class OneWayItineraryConverter : System.ComponentModel.TypeConverter
    {
        private const string _constResponseMessageExchangePattern = "One-Way";
        private const bool _constIsRequestResponse = false;
        private const string _constMessageState = "Pending";
        private const string _constServiceType = "Messaging";
        private const string _constTransformType = "";
        private const string _constDefaultServiceName = "BizTalkEsbService";
        private const string _constVersionNumber = "1.0";
        
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(FrameworkMessage).IsAssignableFrom(sourceType))
            {
                return true;
            }
            else if (sourceType == typeof(Open.MOF.BizTalk.Adapters.MessageHandlers.ItineraryDescription))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is Open.MOF.Messaging.FrameworkMessage)
            {
                Open.MOF.Messaging.FrameworkMessage message = (Open.MOF.Messaging.FrameworkMessage)value;

                string resolverString = GetStaticResolverString(message.To);
                Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.Itinerary itinerary = BuildItinerary(resolverString);

                return itinerary;
            }
            else if (value is Open.MOF.BizTalk.Adapters.MessageHandlers.ItineraryDescription)
            {
                Open.MOF.BizTalk.Adapters.MessageHandlers.ItineraryDescription itineraryDescription = (Open.MOF.BizTalk.Adapters.MessageHandlers.ItineraryDescription)value;

                string resolverString = GetSelectorResolverString(itineraryDescription);
                Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.Itinerary itinerary = BuildItinerary(resolverString);

                return itinerary;
            }

            return base.ConvertFrom(context, culture, value);
        }

        private Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.Itinerary BuildItinerary(string resolverString)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.Itinerary itinerary = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.Itinerary();
            string itineraryUUID = Guid.NewGuid().ToString();
            string serviceName = (((ConfigurationManager.AppSettings["EsbServiceName"] != null) && (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EsbServiceName"]))) ? ConfigurationManager.AppSettings["EsbServiceName"] : _constDefaultServiceName);

            List<Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServices> wcfItineraryServices = new List<Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServices>();
            List<Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryResolvers> wcfItineraryResolvers = new List<Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryResolvers>();

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryResolvers itinResolvers = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryResolvers();
            itinResolvers.serviceId = serviceName + "0";
            itinResolvers.Value = resolverString; //GetResolverString(message);
            wcfItineraryResolvers.Add(itinResolvers);

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServices services = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServices();
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServicesService srv = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServicesService();
            srv.type = _constServiceType;
            srv.name = serviceName;
            srv.isRequestResponseSpecified = true;
            srv.isRequestResponse = _constIsRequestResponse;
            srv.positionSpecified = true;
            srv.position = 0;
            srv.state = _constMessageState;
            //srv.uuid = ((!string.IsNullOrEmpty(uuid)) ? uuid : itineraryUUID);
            srv.uuid = itineraryUUID;
            srv.beginTime = String.Empty;
            srv.completeTime = String.Empty;
            srv.serviceInstanceId = String.Empty;
            services.Service = srv;
            wcfItineraryServices.Add(services);

            itinerary.isRequestResponseSpecified = true;
            itinerary.isRequestResponse = _constIsRequestResponse;
            itinerary.state = _constMessageState;
            //itinerary.uuid = ((!string.IsNullOrEmpty(uuid)) ? uuid : itineraryUUID);
            itinerary.uuid = itineraryUUID;
            itinerary.beginTime = String.Empty;
            itinerary.completeTime = String.Empty;

            itinerary.ServiceInstance = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayBundledServiceInstance.ItineraryServiceInstance();
            itinerary.ServiceInstance.name = serviceName;
            itinerary.ServiceInstance.isRequestResponseSpecified = true;
            itinerary.ServiceInstance.isRequestResponse = _constIsRequestResponse;
            itinerary.ServiceInstance.positionSpecified = true;
            itinerary.ServiceInstance.position = 0;
            itinerary.ServiceInstance.type = _constServiceType;
            itinerary.ServiceInstance.state = _constMessageState;
            //itinerary.ServiceInstance.uuid = ((!string.IsNullOrEmpty(uuid)) ? uuid : itineraryUUID);
            itinerary.ServiceInstance.uuid = itineraryUUID;

            itinerary.Services = wcfItineraryServices.ToArray();
            itinerary.ResolverGroups = wcfItineraryResolvers.ToArray();

            return itinerary;
        }

        /*
        private string GetResolverString(Open.MOF.Messaging.FrameworkMessage message)
        {
            if ((message.To != null) && (!String.IsNullOrEmpty(message.To.Uri)))
            {
                return GetStaticResolverString(message.To);
            }
            else
            {
                return GetSelectorResolverString(message.MessageXmlType);
            }
        }
        */

        private string GetStaticResolverString(Open.MOF.Messaging.MessagingEndpoint toEndpoint)
        {
            string toTransportLocation = toEndpoint.Uri;
            string toAction = toEndpoint.Action;

            string toTransportType = string.Empty;
            if (toTransportLocation.IndexOf("net.tcp://", StringComparison.CurrentCulture) != -1)
            {
                toTransportType = "WCF-NetTcp";
            }
            else if ((toTransportLocation.IndexOf("http://", StringComparison.CurrentCulture) != -1) || 
                (toTransportLocation.IndexOf("https://", StringComparison.CurrentCulture) != -1))
            {
                toTransportType = "WCF-WSHttp";
            }
            else if (toTransportLocation.IndexOf("net.msmq://", StringComparison.CurrentCulture) != -1)
            {
                toTransportType = "WCF-NetMsmq";
            }
            else if (toTransportLocation.IndexOf("MSMQ://", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                toTransportType = "MSMQ";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<![CDATA[");
            sb.Append("STATIC");
            sb.Append(@":\\");

            sb.Append("TransportType=");
            sb.Append(((!string.IsNullOrEmpty(toTransportType)) ? toTransportType : string.Empty));
            sb.Append(";");

            sb.Append("TransportLocation=");
            sb.Append(((!string.IsNullOrEmpty(toTransportLocation)) ? toTransportLocation : string.Empty));
            sb.Append(";");

            sb.Append("Action=");
            sb.Append(((!string.IsNullOrEmpty(toAction)) ? toAction : string.Empty));
            sb.Append(";");

            sb.Append("MessageExchangePattern=");
            sb.Append(_constResponseMessageExchangePattern);
            sb.Append(";");

            sb.Append("TransformType=");
            sb.Append(_constTransformType);
            sb.Append(";");

            sb.Append("]]>");

            return sb.ToString();
        }

        private string GetSelectorResolverString(Open.MOF.BizTalk.Adapters.MessageHandlers.ItineraryDescription itineraryDescription)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<![CDATA[");
            sb.Append("ITINERARY");
            sb.Append(@":\\");

            sb.Append("name=");
            sb.Append(((!string.IsNullOrEmpty(itineraryDescription.ItineraryName)) ? itineraryDescription.ItineraryName : string.Empty));
            sb.Append(";");

            sb.Append("version=");
            sb.Append(((!string.IsNullOrEmpty(itineraryDescription.ItineraryVersion)) ? itineraryDescription.ItineraryVersion : string.Empty));
            sb.Append(";");

            sb.Append("]]>");

            return sb.ToString();
        } 
    }
}
