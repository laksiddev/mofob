using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.SampleService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract(Name = "ITestDataService", ConfigurationName = "Open.MOF.SampleService.ITestDataService", Namespace = "http://mof.open/Samples/ServiceContracts/1/0/")]
    public interface ITestDataService
    {
        [OperationContract(Name = "ProcessTestDataRequest", Action = "http://mof.open/Samples/ServiceContracts/1/0/ITestDataService/ProcessTestDataRequest")]
        TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage requestMessage);

        [OperationContract(Name = "ProcessTestTransactionRequest", Action = "http://mof.open/Samples/ServiceContracts/1/0/ITestDataService/ProcessTestTransactionRequest")]
        TestTransactionResponseMessage ProcessTestTransactionRequest(TestTransactionRequestMessage requestMessage);

        [OperationContract(Name = "ProcessTestTransactionSubmit", Action = "http://mof.open/Samples/ServiceContracts/1/0/ITestDataService/ProcessTestTransactionSubmit")]
        void ProcessTestTransactionSubmit(TestTransactionSubmitMessage message);

        [OperationContract(Name = "ProcessTestPubSubRequest", Action = "http://mof.open/Samples/ServiceContracts/1/0/ITestDataService/ProcessTestPubSubRequest")]
        void ProcessTestPubSubRequest(TestPubSubRequestMessage message);
    }
}
