using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class EsbMessageHandlerFactory
    {
        //private static Dictionary<string, System.Type> _handlerTypeLookup;
        private static Dictionary<string, ConstructorInfo> _handlerConstructorLookup;

        public static IEsbMessageHandler GetHandlerInstance(string channelEndpointName)
        {
            IEsbMessageHandler handler = null;
            if (String.IsNullOrEmpty(channelEndpointName))
                throw new MessagingConfigurationException("ESB Channel Endpoint Name was not found in the application settings.");

            handler = ServiceLocator.Current.GetInstance<IEsbMessageHandler>(channelEndpointName);

            if (handler == null)
            {
                if (!TryCreateHandler(channelEndpointName, out handler))
                    throw new MessagingConfigurationException("ESB Channel Endpoint is not of a supported interface type.");
            }

            return handler;
        }

        private static bool TryCreateHandler(string channelEndpointName, out IEsbMessageHandler handler)
        {
            handler = null;
                        
            ChannelEndpointElement channel = WcfUtility.FindEndpointByName(channelEndpointName);
            if (channel == null)
                throw new MessagingConfigurationException("ESB Channel Endpoint for the defined name was not properly configured in application settings.");

            if (_handlerConstructorLookup == null)
                InitializeHandlerConstructorLookup();

            if (_handlerConstructorLookup.ContainsKey(channel.Contract))
            {
                ConstructorInfo constructor = _handlerConstructorLookup[channel.Contract];
                handler = (IEsbMessageHandler)constructor.Invoke(new object[] { channel.Name });

                if (handler != null)
                {
                    IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
                    container.RegisterInstance<IEsbMessageHandler>(channel.Name, handler, new ContainerControlledLifetimeManager());

                    return true;
                }
            }

            return false;
        }

        private static void InitializeHandlerConstructorLookup()
        {
            _handlerConstructorLookup = new Dictionary<string,ConstructorInfo>();

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            foreach (Type testHandlerType in thisAssembly.GetTypes())
            {
                if (typeof(IEsbMessageHandler).IsAssignableFrom(testHandlerType))
                {
                    ConstructorInfo constructor = testHandlerType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        IEsbMessageHandler tempHandler = null;
                        try
                        {
                            tempHandler = (IEsbMessageHandler)constructor.Invoke(new object[0]);
                        }
                        catch (Exception) { }

                        if ((tempHandler != null) &&
                            (!String.IsNullOrEmpty(tempHandler.EndpointConctactName)))
                        {
                            constructor = testHandlerType.GetConstructor(new Type[] { typeof(string) });
                            if (constructor != null)
                                _handlerConstructorLookup.Add(tempHandler.EndpointConctactName, constructor);
                        }
                    }
                }
            }
        }
    }
}
