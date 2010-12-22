namespace Open.MOF.BizTalk.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"messageId", @"MessageEndpoint", @"To", @"From", @"ReplyTo", @"senderDescription", @"relatedMessageId"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"Open.MOF.BizTalk.Schemas.schemas_microsoft_com_2003_10_Serialization", typeof(global::Open.MOF.BizTalk.Schemas.schemas_microsoft_com_2003_10_Serialization))]
    public sealed class mof_open_Messaging_DataContracts_1_0 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://mof.open/Messaging/DataContracts/1/0/"" elementFormDefault=""qualified"" targetNamespace=""http://mof.open/Messaging/DataContracts/1/0/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""Open.MOF.BizTalk.Schemas.schemas_microsoft_com_2003_10_Serialization"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element xmlns:q1=""http://schemas.microsoft.com/2003/10/Serialization/"" name=""messageId"" nillable=""true"" type=""q1:guid"" />
  <xs:complexType name=""MessageEndpoint"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""uri"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""action"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""MessageEndpoint"" nillable=""true"" type=""tns:MessageEndpoint"" />
  <xs:element name=""To"" nillable=""true"" type=""tns:MessageEndpoint"" />
  <xs:element name=""From"" nillable=""true"" type=""tns:MessageEndpoint"" />
  <xs:element name=""ReplyTo"" nillable=""true"" type=""tns:MessageEndpoint"" />
  <xs:element name=""senderDescription"" nillable=""true"" type=""xs:string"" />
  <xs:element xmlns:q2=""http://schemas.microsoft.com/2003/10/Serialization/"" name=""relatedMessageId"" nillable=""true"" type=""q2:guid"" />
</xs:schema>";
        
        public mof_open_Messaging_DataContracts_1_0() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [7];
                _RootElements[0] = "messageId";
                _RootElements[1] = "MessageEndpoint";
                _RootElements[2] = "To";
                _RootElements[3] = "From";
                _RootElements[4] = "ReplyTo";
                _RootElements[5] = "senderDescription";
                _RootElements[6] = "relatedMessageId";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"messageId")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"messageId"})]
        public sealed class messageId : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public messageId() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "messageId";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"MessageEndpoint")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"MessageEndpoint"})]
        public sealed class MessageEndpoint : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public MessageEndpoint() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "MessageEndpoint";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"To")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"To"})]
        public sealed class To : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public To() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "To";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"From")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"From"})]
        public sealed class From : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public From() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "From";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"ReplyTo")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ReplyTo"})]
        public sealed class ReplyTo : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ReplyTo() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ReplyTo";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"senderDescription")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"senderDescription"})]
        public sealed class senderDescription : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public senderDescription() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "senderDescription";
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
        
        [Schema(@"http://mof.open/Messaging/DataContracts/1/0/",@"relatedMessageId")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"relatedMessageId"})]
        public sealed class relatedMessageId : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public relatedMessageId() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "relatedMessageId";
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
