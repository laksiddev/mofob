using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.BizTalk.Messages;

namespace Open.MOF.BizTalk.ItineraryLookupService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract(Name = "IItineraryLookupService", ConfigurationName = "Open.MOF.BizTalk.ItineraryLookupService.IItineraryLookupService", Namespace = "http://mof.open/BizTalkEsb/ItineraryLookupService/ServiceContracts/1/0/")]
    public interface IItineraryLookupService
    {
        [OperationContract(Name = "ProcessEsbItineraryLookupRequest", Action = "http://mof.open/BizTalkEsb/ItineraryLookupService/ServiceContracts/1/0/IItineraryLookupService/ProcessEsbItineraryLookupRequest")]
        MessageItineraryMappingResponseMessage ProcessEsbItineraryLookupRequest(MessageItineraryMappingRequestMessage requestMesssage);
    }
}
