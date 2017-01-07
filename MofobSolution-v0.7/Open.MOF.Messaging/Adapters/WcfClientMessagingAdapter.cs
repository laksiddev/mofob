using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Adapters
{
    public class WcfClientMessagingAdapter : MessagingAdapter
    {
        private Dictionary<Type, Dictionary<string, WcfEndpointDetails>> _endpointParameterLookup = null;
        private Dictionary<Type, ChannelFactory> _factoryInstances = null;
        private string _bindingType;

        protected WcfClientMessagingAdapter(string bindingName) : base(bindingName)
        {
        }

        protected override bool NativeAsyncSupported
        {
            get { return false; }
        }

        public override string AdapterContext
        {
            get 
            {
                string adapterType = this.GetType().AssemblyQualifiedName;
                return String.Format("<AdapterContext><Adapter type=\"{0}\" endpointName=\"{1}\" /></AdapterContext>", adapterType, _channelEndpointName); 
            }
        }

        protected override MessagingState PerformSubmitMessage(SimpleMessage requestMessage)
        {
            if (requestMessage is FrameworkMessage)
            {
                return PerformSubmitFrameworkMessage((FrameworkMessage)requestMessage);
            }
            else
            {
                return PerformSubmitSimpleMessage(requestMessage);
            }
        }

        private MessagingState PerformSubmitFrameworkMessage(FrameworkMessage requestMessage)
        {
            Initialize();

            if (!_endpointParameterLookup.ContainsKey(requestMessage.GetType()))
                throw new MessagingException("The requested message type does not have a properly configured endpoint.");

            WcfEndpointDetails endpointDetails = null;
            Dictionary<string, WcfEndpointDetails> endpointActionLookup = _endpointParameterLookup[requestMessage.GetType()];
            if ((requestMessage.To != null) && (endpointActionLookup.ContainsKey(requestMessage.To.Action)))
            {
                if (WcfUtility.DoesAddressMatchBinding(requestMessage.To.Uri, _bindingType))
                {
                    endpointDetails = endpointActionLookup[requestMessage.To.Action];
                }
            }
            else if (endpointActionLookup.Count > 0)
            {
                foreach (string action in endpointActionLookup.Keys)
                {
                    endpointDetails = endpointActionLookup[action];
                    break;
                }
            }
            if (endpointDetails == null)
                throw new MessagingConfigurationException("The Wcf Endpoint was not configured properly for the requested transport channel.");

            ConstructorInfo constructor = endpointDetails.FactoryConstructedType.GetConstructor(new Type[] { typeof(string) });
            MethodInfo createChannelMethod = endpointDetails.FactoryConstructedType.GetMethod("CreateChannel", new Type[0]);

            ChannelFactory factory = null;
            if (_factoryInstances.ContainsKey(endpointDetails.FactoryConstructedType))
            {
                factory = _factoryInstances[endpointDetails.FactoryConstructedType];
            }
            else
            {
                factory = (ChannelFactory)constructor.Invoke(new object[] { endpointDetails.ChannelEndpointName });
                factory.Open();
                _factoryInstances.Add(endpointDetails.FactoryConstructedType, factory);
            }
            if ((requestMessage.To != null) && (!String.IsNullOrEmpty(requestMessage.To.Uri)))
            {
                factory.Endpoint.Address = new EndpointAddress(requestMessage.To.Uri);
            }

            System.ServiceModel.Channels.IChannel channel = (System.ServiceModel.Channels.IChannel)createChannelMethod.Invoke(factory, new object[0]);
            channel.Open();
            object invokeResult = endpointDetails.InterfaceMethod.Invoke(channel, new object[] { requestMessage });
            channel.Close();

            MessageHandlingSummary handlingSummary = new MessageHandlingSummary(true, false, false, AdapterContext);
            FrameworkMessage responseMessage;
            if (invokeResult != null)
            {
                if (invokeResult is FrameworkMessage)
                {
                    responseMessage = (FrameworkMessage)invokeResult;
                    handlingSummary.ResponseReceived = true;
                }
                else
                {
                    throw new MessagingException("The messaging service did not return a properly formatted message type.");
                }
            }
            else
            {
                responseMessage = new MessageSubmittedResponse();
                responseMessage.RelatedMessageId = requestMessage.MessageId;
                handlingSummary.ResponseReceived = false;            
            }

            return new MessagingState(requestMessage, handlingSummary, responseMessage);
        }

        private MessagingState PerformSubmitSimpleMessage(SimpleMessage requestMessage)
        {
            Initialize();

            object channelMethodArgument = null;
            string endpointAction = String.Empty;
            Dictionary<string, WcfEndpointDetails> endpointActionLookup = null;
            if (_endpointParameterLookup.ContainsKey(typeof(string)))
            {
                endpointActionLookup = _endpointParameterLookup[typeof(string)];
                foreach (string action in endpointActionLookup.Keys)
                {
                    endpointAction = action;
                    break;
                }

                channelMethodArgument = requestMessage.ToXmlString();
            }
            else if (_endpointParameterLookup.ContainsKey(typeof(System.ServiceModel.Channels.Message)))
            {
                endpointActionLookup = _endpointParameterLookup[typeof(System.ServiceModel.Channels.Message)];
                foreach (string action in endpointActionLookup.Keys)
                {
                    endpointAction = action;
                    break;
                }

                channelMethodArgument = CreateMessageFromXmlDocument(requestMessage.ToXmlDocument(), endpointAction);
            }
            if (endpointActionLookup == null)
                throw new MessagingConfigurationException("The Wcf Endpoint was not configured properly for the requested transport channel.");

            WcfEndpointDetails endpointDetails = endpointActionLookup[endpointAction];

            ConstructorInfo constructor = endpointDetails.FactoryConstructedType.GetConstructor(new Type[] { typeof(string) });
            MethodInfo createChannelMethod = endpointDetails.FactoryConstructedType.GetMethod("CreateChannel", new Type[0]);

            ChannelFactory factory = null;
            if (_factoryInstances.ContainsKey(endpointDetails.FactoryConstructedType))
            {
                factory = _factoryInstances[endpointDetails.FactoryConstructedType];
            }
            else
            {
                factory = (ChannelFactory)constructor.Invoke(new object[] { endpointDetails.ChannelEndpointName });
                factory.Open();
                _factoryInstances.Add(endpointDetails.FactoryConstructedType, factory);
            }

            System.ServiceModel.Channels.IChannel channel = (System.ServiceModel.Channels.IChannel)createChannelMethod.Invoke(factory, new object[0]);
            channel.Open();
            object invokeResult = endpointDetails.InterfaceMethod.Invoke(channel, new object[] { channelMethodArgument });
            channel.Close();

            MessageHandlingSummary handlingSummary = new MessageHandlingSummary(true, false, false, AdapterContext);
            SimpleMessage responseMessage;
            if (invokeResult != null)
            {
                if (invokeResult is string)
                {
                    responseMessage = new TwoWayResponseMessage();
                    responseMessage.LoadContent((string)invokeResult);
                    handlingSummary.ResponseReceived = true;
                }
                else if (invokeResult is System.ServiceModel.Channels.Message)
                {
                    XmlDictionaryReader reader = ((System.ServiceModel.Channels.Message)invokeResult).GetReaderAtBodyContents();
                    XmlDocument requestBody = new XmlDocument();
                    requestBody.Load(reader);

                    responseMessage = new TwoWayResponseMessage();
                    responseMessage.LoadContent(requestBody);
                    handlingSummary.ResponseReceived = true;
                }
                else
                {
                    throw new MessagingException("The messaging service did not return a properly formatted message type.");
                }
            }
            else
            {
                responseMessage = new MessageSubmittedResponse();
                handlingSummary.ResponseReceived = false;
            }

            return new MessagingState(requestMessage, handlingSummary, responseMessage);
        }

        protected override AdapterInterfaceType SuportedAdapterInterfaces
        {
            get { return (AdapterInterfaceType.DataService); }
        }

        protected internal override bool CanSupportMessage(SimpleMessage message)
        {
            Initialize();

            MessageBehavior behavior = message.GetMessageBehavior();
            bool messageHasValidBehavior = ((behavior == MessageBehavior.DataReporting) || (behavior == MessageBehavior.TransactionsSupported));

            Type messageType = message.GetType();
            bool messageDefinedInEndpointContract = false;
            if (message is FrameworkMessage)
            {
                messageDefinedInEndpointContract = _endpointParameterLookup.ContainsKey(messageType);
            }
            else if (message is SimpleMessage)
            {
                messageDefinedInEndpointContract = (_endpointParameterLookup.ContainsKey(typeof(string)) || _endpointParameterLookup.ContainsKey(typeof(System.ServiceModel.Channels.Message)));
            }

            bool messageHasCompatibleAddress = true;
            if ((message is FrameworkMessage) && (((FrameworkMessage)message).To != null) && (_endpointParameterLookup.ContainsKey(messageType)))
            {
                messageHasCompatibleAddress = ((_endpointParameterLookup[messageType].ContainsKey(((FrameworkMessage)message).To.Action)) && 
                    (WcfUtility.DoesAddressMatchBinding(((FrameworkMessage)message).To.Uri, _bindingType)));
            }

            // Until all use cases are determined, we do not need to worry about Response Messages
            //bool messageSupportsTwoWay = message.RequiresTwoWay;

            bool isMessageSupported = ((messageHasValidBehavior) && (messageDefinedInEndpointContract) && (messageHasCompatibleAddress));

            return (isMessageSupported);
        }

        public override void Dispose()
        {
            if (_factoryInstances != null)
            {
                foreach (Type factoryType in _factoryInstances.Keys)
                {
                    ChannelFactory factory = _factoryInstances[factoryType];
                    if (factory.State == CommunicationState.Opened)
                    {
                        // HACK to prevent Exceptions when closing connection to hide other exceptions
                        // See http://msdn.microsoft.com/en-us/library/aa355056.aspx
                        try
                        {
                            factory.Close();
                        }
                        catch (CommunicationException)
                        {
                            factory.Abort();
                        }
                        catch (TimeoutException)
                        {
                            factory.Abort();
                        }
                        catch (Exception)
                        {
                            factory.Abort();
                            throw;
                        }
                    }
                    ((IDisposable)factory).Dispose();
                }

                _factoryInstances.Clear();
                _factoryInstances = null;
            }

            if (_endpointParameterLookup != null)
            {
                _endpointParameterLookup.Clear();
                _endpointParameterLookup = null;
            }
        }

        private void Initialize()
        {
            if (_endpointParameterLookup != null)
                return;

            _factoryInstances = new Dictionary<Type, ChannelFactory>();
            _endpointParameterLookup = new Dictionary<Type, Dictionary<string, WcfEndpointDetails>>();

            ChannelEndpointElement channelEndpoint = WcfUtility.FindEndpointByName(_channelEndpointName);
            if (channelEndpoint == null)
            {
                EventLogUtility.LogWarningMessage(String.Format("Channel endpoint name could not be found: {0}.  Are you sure this endpoint name has a corresponding WCF <client> configuration element?", _channelEndpointName));
                //throw new MessagingConfigurationException(String.Format("Channel endpoint name could not be found: {0}", item.ChannelEndpointName));
                return;
            }

            _bindingType = channelEndpoint.Binding;
            Type channelInterfaceType = WcfUtility.GetChannelInterfaceType(channelEndpoint);
            if (channelInterfaceType == null)
            {
                EventLogUtility.LogWarningMessage(String.Format("Channel endpoint type could not be found: {0}.  Are you sure the assembly for the CLR type for this endpoint name is accessible in the application directory?", _channelEndpointName));
                //throw new MessagingConfigurationException(String.Format("Channel endpoint type could not be found: {0}", item.ChannelEndpointName));
                return;
            }

            System.Type factoryGenericType = typeof(System.ServiceModel.ChannelFactory<>);
            System.Type factoryConstructedType = factoryGenericType.MakeGenericType(channelInterfaceType);

            foreach (MethodInfo interfaceMethod in channelInterfaceType.GetMethods())
            {
                ParameterInfo[] parameters = interfaceMethod.GetParameters();
                if (parameters.Length == 1)
                {
                    System.Type messageParameterType = parameters[0].ParameterType;
                    if (typeof(FrameworkMessage).IsAssignableFrom(messageParameterType))
                    {
                        string action = String.Empty;
                        OperationContractAttribute[] attributes = (OperationContractAttribute[])interfaceMethod.GetCustomAttributes(typeof(OperationContractAttribute), false);
                        if ((attributes != null) && (attributes.Length > 0) && (attributes[0].Action != null))
                        {
                            action = attributes[0].Action;
                        }

                        Dictionary<string, WcfEndpointDetails> endpointActionLookup;
                        if (_endpointParameterLookup.ContainsKey(messageParameterType))
                        {
                            endpointActionLookup = _endpointParameterLookup[messageParameterType];
                        }
                        else
                        {
                            endpointActionLookup = new Dictionary<string, WcfEndpointDetails>();
                            _endpointParameterLookup.Add(messageParameterType, endpointActionLookup);
                        }

                        WcfEndpointDetails endpointDetails = new WcfEndpointDetails(_channelEndpointName, channelInterfaceType, factoryConstructedType, interfaceMethod);
                        endpointActionLookup.Add(action, endpointDetails);
                    }
                    else if ((messageParameterType == typeof(string)) || 
                        (messageParameterType == typeof(System.ServiceModel.Channels.Message)))
                    {
                        if (!_endpointParameterLookup.ContainsKey(messageParameterType))
                        {
                            string action = String.Empty;
                            OperationContractAttribute[] attributes = (OperationContractAttribute[])interfaceMethod.GetCustomAttributes(typeof(OperationContractAttribute), false);
                            if ((attributes != null) && (attributes.Length > 0) && (attributes[0].Action != null))
                            {
                                action = attributes[0].Action;
                            }

                            Dictionary<string, WcfEndpointDetails> endpointActionLookup = new Dictionary<string, WcfEndpointDetails>(); ;
                            WcfEndpointDetails endpointDetails = new WcfEndpointDetails(_channelEndpointName, channelInterfaceType, factoryConstructedType, interfaceMethod);
                            endpointActionLookup.Add(action, endpointDetails);
                            _endpointParameterLookup.Add(messageParameterType, endpointActionLookup);
                        }
                    }
                }
            }
        }

        public static System.ServiceModel.Channels.Message CreateMessageFromXmlDocument(XmlDocument doc, string action)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            XmlTextWriter xw = new XmlTextWriter(ms, Encoding.UTF8);
            doc.WriteTo(xw);
            xw.Flush();
            ms.Position = 0;

            XmlTextReader body = new XmlTextReader(ms);
            System.ServiceModel.Channels.Message resultMessage = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap12WSAddressing10, action, body);

            return resultMessage;
        }
    }
}
