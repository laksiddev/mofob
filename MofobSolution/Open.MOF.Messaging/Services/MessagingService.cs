using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public abstract class MessagingService : IDisposable
    {
        protected MessagingService() 
        {
        }


        public abstract void Dispose();

        //public static MessagingService CreateInstance(Type messageType)
        //{
        //    throw new NotImplementedException();
        //}

        public static MessagingService CreateInstance(string serviceConfigurationName)
        {
            ServiceConfigurationSettings settings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");
            if ((settings == null) || (settings.ServiceConfigurationItems.Count == 0))
            {
                //MessageLogger.LogWarningMessage("Data services were not configured in the application config file.");
                return null;
            }

            MessagingService serviceInstance = null;
            foreach (ServiceConfigurationElement item in settings.ServiceConfigurationItems)
            {
                System.Type requiredInterfaceType = null;
                switch (item.InterfaceType)
                {
                    case ServiceInterfaceType.MessageService:
                        requiredInterfaceType = typeof(IMessageService);
                        break;

                    case ServiceInterfaceType.ExceptionService:
                        requiredInterfaceType = typeof(IExceptionService);
                        break;

                    case ServiceInterfaceType.SubscriptionService:
                        requiredInterfaceType = typeof(ISubscriptionService);
                        break;
                }

                if (String.Compare(item.Name, serviceConfigurationName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    serviceInstance = TryCreateInstance(item.ServiceType, requiredInterfaceType);
                    if (serviceInstance != null)
                        break;
                }
            }

            return serviceInstance;
        }

        private static MessagingService TryCreateInstance(Type serviceType, System.Type requiredInterfaceType)
        {
            MessagingService serviceInstance = null;
            if ((typeof(MessagingService).IsAssignableFrom(serviceType)) && (requiredInterfaceType != null) && (requiredInterfaceType.IsAssignableFrom(serviceType)))
            {
                System.Reflection.ConstructorInfo constructorMethod = serviceType.GetConstructor(System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[0], null);
                if (constructorMethod != null)
                {
                    serviceInstance = (MessagingService)constructorMethod.Invoke(new object[0]);
                }
                else
                {
                    serviceInstance = (MessagingService)Activator.CreateInstance(serviceType);
                }
            }

            return serviceInstance;
        }
    }
}
