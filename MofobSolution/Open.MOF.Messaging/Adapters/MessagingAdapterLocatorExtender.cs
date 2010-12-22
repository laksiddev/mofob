using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Adapters
{
    public class MessagingAdapterLocatorExtender : ILocatorExtender
    {
        #region ILocatorExtender Members

        public void InitializeLocatorExtender(Microsoft.Practices.Unity.IUnityContainer container)
        {
            AdapterConfigurationSettings configurationSettings = (AdapterConfigurationSettings)ConfigurationManager.GetSection("messagingAdapterConfiguration");
            IMessagingResolver resolver = new MessagingResolver();

            foreach (AdapterConfigurationElement item in configurationSettings.AdapterConfigurationItems)
            {
                // HACK RegisterType does not work with non-parameterless constructors
                // So we have to new-up our own instances using reflection and use RegisterInstance insted

                IMessagingAdapter adapter = TryCreateInstance(item);
                if (adapter != null)
                {
                    container.RegisterInstance<IMessagingAdapter>(item.Name, adapter, new ContainerControlledLifetimeManager());
                    AdapterInterfaceType interfaceType = MessagingAdapter.AdapterInterfaceLookup(item.AdapterInterfaceName);
                    resolver.RegisterInstanceName(interfaceType, item.Name, item.PreferenceNumber, adapter.GetType());
                }
            }

            container.RegisterInstance<IMessagingResolver>(resolver);
        }

        #endregion

        private static IMessagingAdapter TryCreateInstance(AdapterConfigurationElement item)
        {
            if (item.AdapterType == null)
            {
                EventLogUtility.LogWarningMessage(String.Format("WARNING:  There was a problem locating the type definition for the type with the name: {0}", item.AdapterTypeName));
                return null;
            }

            IMessagingAdapter adapterInstance = null;
            if (typeof(IMessagingAdapter).IsAssignableFrom(item.AdapterType))
            {
                System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                System.Reflection.ConstructorInfo constructorMethod = item.AdapterType.GetConstructor(flags, null, new Type[] { typeof(string) }, null);
                if (constructorMethod != null)
                {
                    adapterInstance = (IMessagingAdapter)constructorMethod.Invoke(new object[] { item.ChannelEndpointName });
                }
                else
                {
                    adapterInstance = (IMessagingAdapter)Activator.CreateInstance(item.AdapterType, flags, null, new object[] { item.ChannelEndpointName }, System.Globalization.CultureInfo.CurrentCulture, null);
                }
            }

            AdapterInterfaceType interfaceType = MessagingAdapter.AdapterInterfaceLookup(item.AdapterInterfaceName);
            if ((adapterInstance == null) || (!adapterInstance.CanSupportInterface(interfaceType)))
                return null;

            return adapterInstance;
        }
    }
}
