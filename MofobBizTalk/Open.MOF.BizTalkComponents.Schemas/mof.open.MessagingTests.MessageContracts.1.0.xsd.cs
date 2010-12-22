namespace Open.MOF.BizTalk.Import.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"TestDataRequestMessage", @"TestDataResponseMessage", @"TestTransactionRequestMessage", @"TestTransactionResponseMessage", @"TestTransactionSubmitMessage", @"TestPubSubRequestMessage"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"Open.MOF.BizTalk.Schemas.mof_open_Messaging_DataContracts_1_0", typeof(global::Open.MOF.BizTalk.Schemas.mof_open_Messaging_DataContracts_1_0))]
    public sealed class mof_open_MessagingTests_MessageContracts_1_0 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://mof.open/MessagingTests/MessageContracts/1/0/"" elementFormDefault=""qualified"" targetNamespace=""http://mof.open/MessagingTests/MessageContracts/1/0/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""Open.MOF.BizTalk.Schemas.mof_open_Messaging_DataContracts_1_0"" namespace=""http://mof.open/Messaging/DataContracts/1/0/"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        <reference targetNamespace=""http://mof.open/Messaging/DataContracts/1/0/"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""TestDataRequestMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""name"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q1=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q1:messageId"" />
        <xs:element xmlns:q2=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q2:To"" />
        <xs:element xmlns:q3=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q3:From"" />
        <xs:element xmlns:q4=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q4:ReplyTo"" />
        <xs:element xmlns:q5=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q5:senderDescription"" />
        <xs:element xmlns:q6=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q6:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""TestDataResponseMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""value"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q7=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q7:messageId"" />
        <xs:element xmlns:q8=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q8:To"" />
        <xs:element xmlns:q9=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q9:From"" />
        <xs:element xmlns:q10=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q10:ReplyTo"" />
        <xs:element xmlns:q11=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q11:senderDescription"" />
        <xs:element xmlns:q12=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q12:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""TestTransactionRequestMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""name"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q13=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q13:messageId"" />
        <xs:element xmlns:q14=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q14:To"" />
        <xs:element xmlns:q15=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q15:From"" />
        <xs:element xmlns:q16=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q16:ReplyTo"" />
        <xs:element xmlns:q17=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q17:senderDescription"" />
        <xs:element xmlns:q18=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q18:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""TestTransactionResponseMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""value"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""context"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q19=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q19:messageId"" />
        <xs:element xmlns:q20=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q20:To"" />
        <xs:element xmlns:q21=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q21:From"" />
        <xs:element xmlns:q22=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q22:ReplyTo"" />
        <xs:element xmlns:q23=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q23:senderDescription"" />
        <xs:element xmlns:q24=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q24:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""TestTransactionSubmitMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""name"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q25=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q25:messageId"" />
        <xs:element xmlns:q26=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q26:To"" />
        <xs:element xmlns:q27=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q27:From"" />
        <xs:element xmlns:q28=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q28:ReplyTo"" />
        <xs:element xmlns:q29=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q29:senderDescription"" />
        <xs:element xmlns:q30=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q30:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""TestPubSubRequestMessage"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""name"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q31=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q31:messageId"" />
        <xs:element xmlns:q32=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q32:To"" />
        <xs:element xmlns:q33=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q33:From"" />
        <xs:element xmlns:q34=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q34:ReplyTo"" />
        <xs:element xmlns:q35=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q35:senderDescription"" />
        <xs:element xmlns:q36=""http://mof.open/Messaging/DataContracts/1/0/"" minOccurs=""0"" ref=""q36:relatedMessageId"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public mof_open_MessagingTests_MessageContracts_1_0() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "TestDataRequestMessage";
                _RootElements[1] = "TestDataResponseMessage";
                _RootElements[2] = "TestTransactionRequestMessage";
                _RootElements[3] = "TestTransactionResponseMessage";
                _RootElements[4] = "TestTransactionSubmitMessage";
                _RootElements[5] = "TestPubSubRequestMessage";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestDataRequestMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestDataRequestMessage"})]
        public sealed class TestDataRequestMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestDataRequestMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestDataRequestMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestDataResponseMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestDataResponseMessage"})]
        public sealed class TestDataResponseMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestDataResponseMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestDataResponseMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestTransactionRequestMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestTransactionRequestMessage"})]
        public sealed class TestTransactionRequestMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestTransactionRequestMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestTransactionRequestMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestTransactionResponseMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestTransactionResponseMessage"})]
        public sealed class TestTransactionResponseMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestTransactionResponseMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestTransactionResponseMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestTransactionSubmitMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestTransactionSubmitMessage"})]
        public sealed class TestTransactionSubmitMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestTransactionSubmitMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestTransactionSubmitMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://mof.open/MessagingTests/MessageContracts/1/0/",@"TestPubSubRequestMessage")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TestPubSubRequestMessage"})]
        public sealed class TestPubSubRequestMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TestPubSubRequestMessage() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TestPubSubRequestMessage";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
    }
}
