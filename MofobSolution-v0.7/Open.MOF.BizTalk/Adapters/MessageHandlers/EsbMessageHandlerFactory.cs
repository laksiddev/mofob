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
                handler = TryCreateHandler(channelEndpointName);
            }

            if (handler == null)
                throw new MessagingConfigurationException("ESB Channel Endpoint is not of a supported interface type.");

            return handler;
        }

        //private static IEsbMessageHandler CreateHandler(ChannelEndpointElement channel) 
        //{
        //    //if (_handlerTypeLookup == null)
        //    //{
        //    //    _handlerTypeLookup = new Dictionary<string,Type>();

        //    //    IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
        //    //    IMessageItineraryMapper mapper = new ServiceOrientedMessageItineraryMapper();
        //    //    container.RegisterInstance<IMessageItineraryMapper>(mapper, new ContainerControlledLifetimeManager());
        //    //}

        //    IEsbMessageHandler handler = TryGetHandler(channel);
        //    if (handler == null)
        //        throw new MessagingConfigurationException("Error configuring BizTalk ESB Endpoint.  The channel endpoint does not appear to support a known ESB messaging contract.");

        //    return handler;

        //    ////bool isGenericItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.ItineraryServicesGenericOneWay", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    ////bool isStaticItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.ItineraryServicesStaticOneWay", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    //bool isTwoWayItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.IProcessRequestResponse", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    //bool isOneWayItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayServiceInstance.IProcessRequest", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    //bool isQueuedOneWayItineraryEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayServiceInstance.IProcessRequest", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    //bool isExceptionHandlingEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.IExceptionHandling", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    //bool isQueuedExceptionHandlingEndpoint = (channel.Contract.IndexOf("Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.IExceptionHandling", StringComparison.CurrentCultureIgnoreCase) != -1);
        //    ////bool isQueuedContract = (channel.Contract.IndexOf("Queued", StringComparison.CurrentCultureIgnoreCase) != -1);

        //    ////if (isGenericItineraryEndpoint)
        //    ////{
        //    ////    //if (isQueuedContract)
        //    ////    //{
        //    ////    //    //return new GenericItineraryQueuedEsbMessageHandler(channel.Name);
        //    ////    //}
        //    ////    //else
        //    ////    //{
        //    ////    //    //return new GenericItineraryEsbMessageHandler(channel.Name);
        //    ////    //}
        //    ////    return null;
        //    ////}
        //    ////else if (isStaticItineraryEndpoint)
        //    ////{
        //    ////    if (isQueuedContract)
        //    ////    {
        //    ////        //return new StaticItineraryQueuedEsbMessageHandler(channel.Name);
        //    ////    }
        //    ////    else
        //    ////    {
        //    ////        //return new StaticItineraryEsbMessageHandler(channel.Name);
        //    ////    }
        //    ////}
        //    //if (isTwoWayItineraryEndpoint)
        //    //{
        //    //    return new TwoWayItineraryEsbMessageHandler(channel.Name);
        //    //}
        //    //else if (isOneWayItineraryEndpoint)
        //    //{
        //    //    return new OneWayItineraryEsbMessageHandler(channel.Name);
        //    //}
        //    //else if (isQueuedOneWayItineraryEndpoint)
        //    //{
        //    //    return new OneWayItineraryQueuedEsbMessageHandler(channel.Name);
        //    //}
        //    //else if (isExceptionHandlingEndpoint)
        //    //{
        //    //    return new ExceptionEsbMessageHandler(channel.Name);
        //    //}
        //    //else if (isQueuedExceptionHandlingEndpoint)
        //    //{
        //    //    return new ExceptionQueuedEsbMessageHandler(channel.Name);
        //    //}
        //    //else
        //    //    throw new MessagingConfigurationException("Error configuring BizTalk ESB Endpoint.  The channel endpoint does not appear to support a known ESB messaging contract.");
        //}

        private static IEsbMessageHandler TryCreateHandler(string channelEndpointName)
        {
            IEsbMessageHandler handler = null;

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
                }
            }

            //Assembly thisAssembly = Assembly.GetExecutingAssembly();
            //foreach (Type testHandlerType in thisAssembly.GetTypes())
            //{
            //    if (typeof(IEsbMessageHandler).IsAssignableFrom(testHandlerType))
            //    {
            //        ConstructorInfo constructor = testHandlerType.GetConstructor(new Type[] { typeof(string) });
            //        if (constructor != null)
            //        {
            //            IEsbMessageHandler tempHandler = null;
            //            try
            //            {
            //                tempHandler = (IEsbMessageHandler)constructor.Invoke(new object[] { channel.Name });
            //            }
            //            catch (Exception) { }

            //            if ((tempHandler != null) &&
            //                (tempHandler.EndpointConctactName == channel.Contract))
            //            {
            //                IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            //                container.RegisterInstance<IEsbMessageHandler>(channel.Name, tempHandler, new ContainerControlledLifetimeManager());

            //                //_handlerTypeLookup.Add(channel.Name, testHandlerType);

            //                handler = tempHandler;
            //                break;
            //            }
            //        }
            //    }
            //}

            return handler;
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
