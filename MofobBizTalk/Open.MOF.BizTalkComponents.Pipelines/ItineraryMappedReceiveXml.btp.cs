namespace Open.MOF.BizTalkComponents.Pipelines
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class ItineraryMappedReceiveXml : Microsoft.BizTalk.PipelineOM.ReceivePipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>f66b9f5e-43ff-4f5f-ba46-885348ae1b4e</CategoryId>  <FriendlyName>Receive</FriendlyName>"+
"  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Decode\" minOccurs=\""+
"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4103-4cce-4536-83fa-4a5040674ad6\" />      <Component"+
"s />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"2\" Name=\"Disassemble\" "+
"minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"FirstMatch\" stageId=\"9d0e4105-4cce-4536-83fa-4a5040674ad6\" "+
"/>      <Components>        <Component>          <Name>Microsoft.BizTalk.Component.XmlDasmComp,Micro"+
"soft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35<"+
"/Name>          <ComponentName>XML disassembler</ComponentName>          <Description>Streaming XML "+
"disassembler</Description>          <Version>1.0</Version>          <Properties>            <Propert"+
"y Name=\"EnvelopeSpecNames\">              <Value xsi:type=\"xsd:string\" />            </Property>     "+
"       <Property Name=\"EnvelopeSpecTargetNamespaces\">              <Value xsi:type=\"xsd:string\" />  "+
"          </Property>            <Property Name=\"DocumentSpecNames\">              <Value xsi:type=\"x"+
"sd:string\" />            </Property>            <Property Name=\"DocumentSpecTargetNamespaces\">      "+
"        <Value xsi:type=\"xsd:string\" />            </Property>            <Property Name=\"AllowUnrec"+
"ognizedMessage\">              <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>    "+
"        <Property Name=\"ValidateDocument\">              <Value xsi:type=\"xsd:boolean\">false</Value> "+
"           </Property>            <Property Name=\"RecoverableInterchangeProcessing\">              <V"+
"alue xsi:type=\"xsd:boolean\">false</Value>            </Property>            <Property Name=\"HiddenPr"+
"operties\">              <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,DocumentSpecTarget"+
"Namespaces</Value>            </Property>          </Properties>          <CachedDisplayName>XML dis"+
"assembler</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Component>  "+
"    </Components>    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"3\" Name=\""+
"Validate\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e410d-4cce-4536-83fa-4a5040674ad"+
"6\" />      <Components />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"4"+
"\" Name=\"ResolveParty\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e410e-4cce-4536-83fa"+
"-4a5040674ad6\" />      <Components>        <Component>          <Name>Open.MOF.BizTalkComponents.Pip"+
"elineComponents.ESBItineraryMessageMapper,Open.MOF.BizTalkComponents.PipelineComponents, Version=1.0"+
".0.0, Culture=neutral, PublicKeyToken=1c9b95553e22a25e</Name>          <ComponentName>ESB Itinerary "+
"Message Mapper</ComponentName>          <Description>Resolver to map itinerary name based on message"+
" type via the itinerary headers.</Description>          <Version>1.0</Version>          <Properties "+
"/>          <CachedDisplayName>ESB Itinerary Message Mapper</CachedDisplayName>          <CachedIsMa"+
"naged>true</CachedIsManaged>        </Component>        <Component>          <Name>Microsoft.Practic"+
"es.ESB.Itinerary.PipelineComponents.ItinerarySelector,Microsoft.Practices.ESB.Itinerary.PipelineComp"+
"onents, Version=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</Name>          <Component"+
"Name>ESB Itinerary Selector</ComponentName>          <Description>Selects itinerary via any resolver"+
". Set up resolver connection string to specify itinerary resolution method.</Description>          <"+
"Version>2.1</Version>          <Properties>            <Property Name=\"ResolverConnectionString\">   "+
"           <Value xsi:type=\"xsd:string\">ITINERARY-STATIC:\\\\headerRequired=false;</Value>            "+
"</Property>            <Property Name=\"ItineraryFactKey\">              <Value xsi:type=\"xsd:string\">"+
"Resolver.Itinerary</Value>            </Property>            <Property Name=\"IgnoreErrorKey\">       "+
"       <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>            <Property Name="+
"\"ValidateItinerary\">              <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>"+
"          </Properties>          <CachedDisplayName>ESB Itinerary Selector</CachedDisplayName>      "+
"    <CachedIsManaged>true</CachedIsManaged>        </Component>        <Component>          <Name>Mi"+
"crosoft.Practices.ESB.PipelineComponents.Dispatcher,Microsoft.Practices.ESB.PipelineComponents, Vers"+
"ion=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</Name>          <ComponentName>ESB Dis"+
"patcher</ComponentName>          <Description>BizTalk ESB Dispatcher Processes Itinerary, Routing an"+
"d Transform  Requests</Description>          <Version>2.1</Version>          <Properties>           "+
" <Property Name=\"Enabled\">              <Value xsi:type=\"xsd:boolean\">true</Value>            </Prop"+
"erty>            <Property Name=\"Endpoint\">              <Value xsi:type=\"xsd:string\" />            "+
"</Property>            <Property Name=\"MapName\">              <Value xsi:type=\"xsd:string\" />       "+
"     </Property>            <Property Name=\"Validate\">              <Value xsi:type=\"xsd:boolean\">tr"+
"ue</Value>            </Property>            <Property Name=\"RoutingServiceName\">              <Valu"+
"e xsi:type=\"xsd:string\">Microsoft.Practices.ESB.Services.Routing</Value>            </Property>     "+
"       <Property Name=\"TransformServiceName\">              <Value xsi:type=\"xsd:string\">Microsoft.Pr"+
"actices.ESB.Services.Transform</Value>            </Property>          </Properties>          <Cache"+
"dDisplayName>ESB Dispatcher</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>    "+
"    </Component>      </Components>    </Stage>  </Stages></Document>";
        
        private const string _versionDependentGuid = "1d232ddb-0d93-487f-8330-5b7f93352923";
        
        public ItineraryMappedReceiveXml()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4105-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.firstRecognized);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.BizTalk.Component.XmlDasmComp,Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"EnvelopeSpecNam"+
"es\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"EnvelopeSpecTargetNamesp"+
"aces\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecNames\">   "+
"   <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecTargetNamespaces\"> "+
"     <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"AllowUnrecognizedMessage\">   "+
"   <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name=\"ValidateDocument\"> "+
"     <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name=\"RecoverableInterc"+
"hangeProcessing\">      <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name="+
"\"HiddenProperties\">      <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,DocumentSpecTarge"+
"tNamespaces</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
            stage = this.AddStage(new System.Guid("9d0e410e-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp1 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Open.MOF.BizTalkComponents.PipelineComponents.ESBItineraryMessageMapper,Open.MOF.BizTalkComponents.PipelineComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1c9b95553e22a25e");;
            if (comp1 is IPersistPropertyBag)
            {
                string comp1XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties /></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp1XmlProperties);;
                ((IPersistPropertyBag)(comp1)).Load(pb, 0);
            }
            this.AddComponent(stage, comp1);
            IBaseComponent comp2 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.Practices.ESB.Itinerary.PipelineComponents.ItinerarySelector,Microsoft.Practices.ESB.Itinerary.PipelineComponents, Version=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp2 is IPersistPropertyBag)
            {
                string comp2XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ResolverConnect"+
"ionString\">      <Value xsi:type=\"xsd:string\">ITINERARY-STATIC:\\\\headerRequired=false;</Value>    </"+
"Property>    <Property Name=\"ItineraryFactKey\">      <Value xsi:type=\"xsd:string\">Resolver.Itinerary"+
"</Value>    </Property>    <Property Name=\"IgnoreErrorKey\">      <Value xsi:type=\"xsd:boolean\">false"+
"</Value>    </Property>    <Property Name=\"ValidateItinerary\">      <Value xsi:type=\"xsd:boolean\">fa"+
"lse</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp2XmlProperties);;
                ((IPersistPropertyBag)(comp2)).Load(pb, 0);
            }
            this.AddComponent(stage, comp2);
            IBaseComponent comp3 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.Practices.ESB.PipelineComponents.Dispatcher,Microsoft.Practices.ESB.PipelineComponents, Version=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp3 is IPersistPropertyBag)
            {
                string comp3XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"Enabled\">      "+
"<Value xsi:type=\"xsd:boolean\">true</Value>    </Property>    <Property Name=\"Endpoint\">      <Value "+
"xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"MapName\">      <Value xsi:type=\"xsd:strin"+
"g\" />    </Property>    <Property Name=\"Validate\">      <Value xsi:type=\"xsd:boolean\">true</Value>  "+
"  </Property>    <Property Name=\"RoutingServiceName\">      <Value xsi:type=\"xsd:string\">Microsoft.Pr"+
"actices.ESB.Services.Routing</Value>    </Property>    <Property Name=\"TransformServiceName\">      <"+
"Value xsi:type=\"xsd:string\">Microsoft.Practices.ESB.Services.Transform</Value>    </Property>  </Pro"+
"perties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp3XmlProperties);;
                ((IPersistPropertyBag)(comp3)).Load(pb, 0);
            }
            this.AddComponent(stage, comp3);
        }
        
        public override string XmlContent
        {
            get
            {
                return _strPipeline;
            }
        }
        
        public override System.Guid VersionDependentGuid
        {
            get
            {
                return new System.Guid(_versionDependentGuid);
            }
        }
    }
}
