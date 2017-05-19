using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService
{
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Microsoft.BizTalk.ESB/", ConfigurationName = "Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.IProcessRequest")]
    public interface IProcessRequest
    {

        // CODEGEN: Generating message contract since message SubmitRequest requires protection.
        [System.ServiceModel.OperationContractAttribute(Action = "SubmitRequest", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestResponse SubmitRequest(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestRequest request);
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary")]
    public partial class Itinerary
    {

        private ItineraryBizTalkSegment bizTalkSegmentField;

        private ItineraryServiceInstance serviceInstanceField;

        private ItineraryServices[] servicesField;

        private ItineraryResolvers[] resolverGroupsField;

        private string uuidField;

        private string beginTimeField;

        private string completeTimeField;

        private string stateField;

        private bool isRequestResponseField;

        private bool isRequestResponseFieldSpecified;

        private int servicecountField;

        private bool servicecountFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ItineraryBizTalkSegment BizTalkSegment
        {
            get
            {
                return this.bizTalkSegmentField;
            }
            set
            {
                this.bizTalkSegmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public ItineraryServiceInstance ServiceInstance
        {
            get
            {
                return this.serviceInstanceField;
            }
            set
            {
                this.serviceInstanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Services", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public ItineraryServices[] Services
        {
            get
            {
                return this.servicesField;
            }
            set
            {
                this.servicesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Resolvers", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ItineraryResolvers[] ResolverGroups
        {
            get
            {
                return this.resolverGroupsField;
            }
            set
            {
                this.resolverGroupsField = value;
            }
        }

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
        public int servicecount
        {
            get
            {
                return this.servicecountField;
            }
            set
            {
                this.servicecountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool servicecountSpecified
        {
            get
            {
                return this.servicecountFieldSpecified;
            }
            set
            {
                this.servicecountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152")]
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
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    public partial class SubmitRequestRequest
    {

        [System.ServiceModel.MessageHeaderAttribute(Namespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary", ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
        public Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.Itinerary Itinerary;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, Order = 0)]
        public object part;

        public SubmitRequestRequest()
        {
        }

        public SubmitRequestRequest(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.Itinerary Itinerary, object part)
        {
            this.Itinerary = Itinerary;
            this.part = part;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false, ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    public partial class SubmitRequestResponse
    {

        public SubmitRequestResponse()
        {
        }
    }
}
