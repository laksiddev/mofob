using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Messaging.Test.WcfService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract(Name = "ITestDataService", ConfigurationName = "Open.MOF.Messaging.Test.WcfService.ITestDataService", Namespace = "http://mof.open/MessagingTests/ServiceContracts/1/0/")]
    public interface ITestDataService
    {
        [OperationContract(Name = "ProcessTestDataRequest", Action = "http://mof.open/MessagingTests/ServiceContracts/1/0/ITestDataService/ProcessTestDataRequest")]
        TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage messsage);

        [OperationContract(Name = "ProcessTestDataResponse", Action = "http://mof.open/MessagingTests/ServiceContracts/1/0/ITestDataService/ProcessTestDataResponse")]
        void ProcessTestDataRequest(TestDataResponseMessage messsage);
    }
}
