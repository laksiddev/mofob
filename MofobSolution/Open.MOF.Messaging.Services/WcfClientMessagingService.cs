using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public class WcfClientMessagingService : MessagingService
    {
        private Dictionary<Type, Dictionary<string, WcfEndpointDetails>> _endpointParameterLookup = null;
        private Dictionary<Type, ChannelFactory> _factoryInstances = null;
        private string _bindingType;

        protected WcfClientMessagingService(string bindingName) : base(bindingName)
        {
        }

        protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            Initialize();

            if (!_endpointParameterLookup.ContainsKey(message.GetType()))
                throw new MessagingException("The requested message type does not have a properly configured endpoint.");

            WcfEndpointDetails endpointDetails = null;
            Dictionary<string, WcfEndpointDetails> endpointActionLookup = _endpointParameterLookup[message.GetType()];
            if ((message.To != null) && (endpointActionLookup.ContainsKey(message.To.Action)))
            {
                if (WcfUtilities.DoesAddressMatchBinding(message.To.Uri, _bindingType))
                {
                    endpointDetails = endpointActionLookup[message.To.Action];
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
            if ((message.To != null) && (!String.IsNullOrEmpty(message.To.Uri)))
            {
                factory.Endpoint.Address = new EndpointAddress(message.To.Uri);
            }

            System.ServiceModel.Channels.IChannel channel = (System.ServiceModel.Channels.IChannel)createChannelMethod.Invoke(factory, new object[0]);
            channel.Open();
            object invokeResult = endpointDetails.InterfaceMethod.Invoke(channel, new object[] { message });
            channel.Close();

            FrameworkMessage responseMessage = null;
            if (invokeResult != null)
            {
                if (invokeResult is FrameworkMessage)
                {
                    responseMessage = (FrameworkMessage)invokeResult;
                }
                else
                {
                    throw new MessagingException("The messaging service did not return a properly formatted message type.");
                }
            }

            return new MessagingResult(message, true, responseMessage);
        }

        protected override ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (ServiceInterfaceType.DataService); }
        }

        protected override bool CanSupportMessage(FrameworkMessage message)
        {
            Initialize();
            Type messageType = message.GetType();
            if (_endpointParameterLookup.ContainsKey(messageType))
            {
                if (message.To != null)
                {
                    return ((_endpointParameterLookup[messageType].ContainsKey(message.To.Action)) && 
                        (WcfUtilities.DoesAddressMatchBinding(message.To.Uri, _bindingType)));
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public override void Dispose()
        {
            if (_factoryInstances != null)
            {
                foreach (Type factoryType in _factoryInstances.Keys)
                {
                    ChannelFactory factory = _factoryInstances[factoryType];
                    factory.Close();
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

            ChannelEndpointElement channelEndpoint = WcfUtilities.FindEndpointByName(_channelEndpointName);
            if (channelEndpoint == null)
            {
                EventLogUtility.LogWarningMessage(String.Format("Channel endpoint name could not be found: {0}", _channelEndpointName));
                //throw new MessagingConfigurationException(String.Format("Channel endpoint name could not be found: {0}", item.ChannelEndpointName));
                return;
            }

            _bindingType = channelEndpoint.Binding;
            Type channelInterfaceType = WcfUtilities.GetChannelInterfaceType(channelEndpoint);
            if (channelInterfaceType == null)
            {
                EventLogUtility.LogWarningMessage(String.Format("Channel endpoint type could not be found: {0}", _channelEndpointName));
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
                }
            }
        }
    }
}
