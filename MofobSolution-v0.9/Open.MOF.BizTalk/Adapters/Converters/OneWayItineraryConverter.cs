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

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is Open.MOF.Messaging.FrameworkMessage)
            {
                Open.MOF.Messaging.FrameworkMessage message = (Open.MOF.Messaging.FrameworkMessage)value;

                string resolverString = GetStaticResolverString(message.To);
                Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.Itinerary itinerary = BuildItinerary(resolverString);

                return itinerary;
            }

            return base.ConvertFrom(context, culture, value);
        }

        private Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.Itinerary BuildItinerary(string resolverString)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.Itinerary itinerary = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.Itinerary();
            string itineraryUUID = Guid.NewGuid().ToString();
            string serviceName = (((ConfigurationManager.AppSettings["EsbServiceName"] != null) && (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EsbServiceName"]))) ? ConfigurationManager.AppSettings["EsbServiceName"] : _constDefaultServiceName);

            List<Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServices> wcfItineraryServices = new List<Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServices>();
            List<Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryResolvers> wcfItineraryResolvers = new List<Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryResolvers>();

            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryResolvers itinResolvers = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryResolvers();
            itinResolvers.serviceId = serviceName + "0";
            itinResolvers.Value = resolverString; //GetResolverString(message);
            wcfItineraryResolvers.Add(itinResolvers);

            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServices services = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServices();
            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServicesService srv = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServicesService();
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

            itinerary.ServiceInstance = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayAddressedServiceInstance.ItineraryServiceInstance();
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
    }
}
