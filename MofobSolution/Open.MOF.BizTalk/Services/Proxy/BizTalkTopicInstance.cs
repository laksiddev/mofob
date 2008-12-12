﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Open.MOF.BizTalk.Services.Proxy
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Microsoft.BizTalk.ESB/", ConfigurationName = "ProcessTopic")]
    public interface ProcessTopic
    {

        // CODEGEN: Generating message contract since message SubmitRequest requires protection.
        [System.ServiceModel.OperationContractAttribute(Action = "SubmitTopic", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        void SubmitTopic(SubmitTopicRequest request);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Microsoft.BizTalk.ESB/", ConfigurationName = "ProcessTopicOneWay")]
    public interface ProcessTopicOneWay
    {

        // CODEGEN: Generating message contract since message SubmitRequest requires protection.
        [System.ServiceModel.OperationContractAttribute(Action = "SubmitTopic", IsOneWay = true)]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        void SubmitTopic(SubmitTopicRequestOneWay request);
    } 

    /*
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.30")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class ItineraryBizTalkSegment
    {

        private string interchangeIdField;

        private string epmRRCorrelationTokenField;

        private string receiveInstanceIdField;

        private string messageIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string interchangeId
        {
            get
            {
                return this.interchangeIdField;
            }
            set
            {
                this.interchangeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string epmRRCorrelationToken
        {
            get
            {
                return this.epmRRCorrelationTokenField;
            }
            set
            {
                this.epmRRCorrelationTokenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string receiveInstanceId
        {
            get
            {
                return this.receiveInstanceIdField;
            }
            set
            {
                this.receiveInstanceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string messageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.30")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class ItineraryServiceInstance
    {

        private string uuidField;

        private string nameField;

        private string typeField;

        private string stateField;

        private int positionField;

        private bool positionFieldSpecified;

        private bool isRequestResponseField;

        private bool isRequestResponseFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string uuid
        {
            get
            {
                return this.uuidField;
            }
            set
            {
                this.uuidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool positionSpecified
        {
            get
            {
                return this.positionFieldSpecified;
            }
            set
            {
                this.positionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isRequestResponse
        {
            get
            {
                return this.isRequestResponseField;
            }
            set
            {
                this.isRequestResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isRequestResponseSpecified
        {
            get
            {
                return this.isRequestResponseFieldSpecified;
            }
            set
            {
                this.isRequestResponseFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.30")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class ItineraryServices
    {

        private ItineraryServicesService serviceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ItineraryServicesService Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.30")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class ItineraryServicesService
    {

        private string uuidField;

        private string beginTimeField;

        private string completeTimeField;

        private string nameField;

        private string typeField;

        private string stateField;

        private bool resolveField;

        private bool resolveFieldSpecified;

        private bool isRequestResponseField;

        private bool isRequestResponseFieldSpecified;

        private int positionField;

        private bool positionFieldSpecified;

        private string serviceInstanceIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string uuid
        {
            get
            {
                return this.uuidField;
            }
            set
            {
                this.uuidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string beginTime
        {
            get
            {
                return this.beginTimeField;
            }
            set
            {
                this.beginTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string completeTime
        {
            get
            {
                return this.completeTimeField;
            }
            set
            {
                this.completeTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool resolve
        {
            get
            {
                return this.resolveField;
            }
            set
            {
                this.resolveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool resolveSpecified
        {
            get
            {
                return this.resolveFieldSpecified;
            }
            set
            {
                this.resolveFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isRequestResponse
        {
            get
            {
                return this.isRequestResponseField;
            }
            set
            {
                this.isRequestResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isRequestResponseSpecified
        {
            get
            {
                return this.isRequestResponseFieldSpecified;
            }
            set
            {
                this.isRequestResponseFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool positionSpecified
        {
            get
            {
                return this.positionFieldSpecified;
            }
            set
            {
                this.positionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string serviceInstanceId
        {
            get
            {
                return this.serviceInstanceIdField;
            }
            set
            {
                this.serviceInstanceIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.30")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class ItineraryResolvers
    {

        private string serviceIdField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string serviceId
        {
            get
            {
                return this.serviceIdField;
            }
            set
            {
                this.serviceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    } */

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    //System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    public partial class SubmitTopicRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, Order = 0)]
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", ProtectionLevel = System.Net.Security.ProtectionLevel.None, Order = 0)]
        public object topic;

        public SubmitTopicRequest()
        {
        }

        public SubmitTopicRequest(object topic)
        {
            this.topic = topic;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    //[System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    public partial class SubmitTopicRequestOneWay
    {

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, Order = 0)]
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", ProtectionLevel = System.Net.Security.ProtectionLevel.None, Order = 0)]
        public object topic;

        public SubmitTopicRequestOneWay()
        {
        }

        public SubmitTopicRequestOneWay(object topic)
        {
            this.topic = topic;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ProcessTopicChannel : ProcessTopic, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ProcessTopicClient : System.ServiceModel.ClientBase<ProcessTopic>, ProcessTopic
    {

        public ProcessTopicClient()
        {
        }

        public ProcessTopicClient(string endpointConfigurationName)
            :
                base(endpointConfigurationName)
        {
        }

        public ProcessTopicClient(string endpointConfigurationName, string remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProcessTopicClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProcessTopicClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(binding, remoteAddress)
        {
        }

        void ProcessTopic.SubmitTopic(SubmitTopicRequest request)
        {
            base.Channel.SubmitTopic(request);
        }

        public void SubmitTopic(object topic)
        {
            SubmitTopicRequest inValue = new SubmitTopicRequest();
            inValue.topic = topic;
            ((ProcessTopic)(this)).SubmitTopic(inValue);
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ProcessTopicOneWayClient : System.ServiceModel.ClientBase<ProcessTopicOneWay>, ProcessTopicOneWay
    {

        public ProcessTopicOneWayClient()
        {
        }

        public ProcessTopicOneWayClient(string endpointConfigurationName)
            :
                base(endpointConfigurationName)
        {
        }

        public ProcessTopicOneWayClient(string endpointConfigurationName, string remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProcessTopicOneWayClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ProcessTopicOneWayClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
            :
                base(binding, remoteAddress)
        {
        }

        void ProcessTopicOneWay.SubmitTopic(SubmitTopicRequestOneWay request)
        {
            base.Channel.SubmitTopic(request);
        }

        public void SubmitTopic(object topic)
        {
            SubmitTopicRequestOneWay inValue = new SubmitTopicRequestOneWay();
            inValue.topic = topic;
            ((ProcessTopicOneWay)this).SubmitTopic(inValue);
        }
    }
    


}