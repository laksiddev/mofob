
namespace Open.MOF.Messaging.Test.WcfServiceProxyReference {
    
    
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfServiceProxyReference.ITestDataService")]
    public interface ITestDataService {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://mofob.open/MessagingTests/ServiceContracts/1/0/) of message TestDataRequestMessage does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITestDataService/ProcessTestDataRequest", ReplyAction="http://tempuri.org/ITestDataService/ProcessTestDataRequestResponse")]
        Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage ProcessTestDataRequest(Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataRequestMessage request);
    }
    
    [System.ServiceModel.MessageContractAttribute(WrapperName="TestDataRequestMessage", WrapperNamespace="http://mofob.open/MessagingTests/ServiceContracts/1/0/", IsWrapped=true)]
    public partial class TestDataRequestMessage : Open.MOF.Messaging.DataRequestMessage<TestDataRequestMessage> {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mofob.open/MessagingTests/DataContracts/1/0/", Order=0)]
        public string name;
       
        public TestDataRequestMessage() {
        }
     }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TestDataResponseMessage", WrapperNamespace="http://mofob.open/MessagingTests/ServiceContracts/1/0/", IsWrapped=true)]
    public partial class TestDataResponseMessage : Open.MOF.Messaging.ResponseMessage<TestDataResponseMessage> {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mofob.open/MessagingTests/DataContracts/1/0/", Order=0)]
        public string value;
        
        public TestDataResponseMessage() {
        }
    }
}
