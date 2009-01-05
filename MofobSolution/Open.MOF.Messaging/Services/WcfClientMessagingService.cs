using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public class WcfClientMessagingService : MessagingService
    {
        private Dictionary<Type, WcfEndpointDetails> _endpointLookup = null;
        private Dictionary<Type, ChannelFactory> _factoryInstances = null;

        protected WcfClientMessagingService(string bindingName) : base(bindingName)
        {
        }

        protected override MessagingResult PerformSubmitMessage(MessageBase message)
        {
            if (_endpointLookup == null)
            {
                _endpointLookup = InitializeLookup();
                _factoryInstances = new Dictionary<Type, ChannelFactory>();
            }

            if (!_endpointLookup.ContainsKey(message.GetType()))
                throw new MessagingException("The requested message type does not have a properly configured endpoint.");

            WcfEndpointDetails endpointDetails = _endpointLookup[message.GetType()];
            ConstructorInfo constructor = endpointDetails.FactoryConstructedType.GetConstructor(new Type[] { typeof(string) });
            MethodInfo createChannelMethod = endpointDetails.FactoryConstructedType.GetMethod("CreateChannel", new Type[0]);

            ChannelFactory factory = null;
            if (_factoryInstances.ContainsKey(endpointDetails.FactoryConstructedType))
            {
                factory = _factoryInstances[endpointDetails.FactoryConstructedType];
            }
            else
            {
                factory = (ChannelFactory)constructor.Invoke(new object[] { endpointDetails.BindingName });
                factory.Open();
                _factoryInstances.Add(endpointDetails.FactoryConstructedType, factory);
            }

            System.ServiceModel.Channels.IChannel channel = (System.ServiceModel.Channels.IChannel)createChannelMethod.Invoke(factory, new object[0]);
            channel.Open();
            object invokeResult = endpointDetails.InterfaceMethod.Invoke(channel, new object[] { message });
            channel.Close();

            MessageBase responseMessage = null;
            if (invokeResult != null)
            {
                if (invokeResult is MessageBase)
                {
                    responseMessage = (MessageBase)invokeResult;
                }
                else
                {
                    throw new MessagingException("The messaging service did not return a properly formatted message type.");
                }
            }
            return new MessagingResult(message, true, responseMessage);
        }

        public override ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (ServiceInterfaceType.DataService); }
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

            if (_endpointLookup != null)
            {
                _endpointLookup.Clear();
                _endpointLookup = null;
            }
        }

        private Dictionary<Type, WcfEndpointDetails> InitializeLookup()
        {
            Dictionary<Type, WcfEndpointDetails> interfaceLookup = new Dictionary<Type, WcfEndpointDetails>();

            WcfEndpointConfigurationSettings configurationSettings = (WcfEndpointConfigurationSettings)ConfigurationManager.GetSection("wcfEndpointConfiguration");
            foreach (WcfEndpointConfigurationElement item in configurationSettings.WcfEndpointConfigurationItems)
            {
                System.Type factoryGenericType = typeof(System.ServiceModel.ChannelFactory<>);
                System.Type factoryConstructedType = factoryGenericType.MakeGenericType(item.EndpointInterfaceType);

                foreach (MethodInfo interfaceMethod in item.EndpointInterfaceType.GetMethods())
                {
                    ParameterInfo[] parameters = interfaceMethod.GetParameters();
                    if (parameters.Length == 1)
                    {
                        System.Type messageParameterType = parameters[0].ParameterType;
                        if (typeof(MessageBase).IsAssignableFrom(messageParameterType))
                        {
                            WcfEndpointDetails endpointDetails = new WcfEndpointDetails(item.BindingName, item.EndpointInterfaceType, factoryConstructedType, interfaceMethod);
                            interfaceLookup.Add(messageParameterType, endpointDetails);
                        }
                    }
                }
            }

            return interfaceLookup;
        }
    }
}
