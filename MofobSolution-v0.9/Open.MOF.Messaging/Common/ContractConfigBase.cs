using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Reflection;

using Open.MOF.Messaging.Adapters;

namespace Open.MOF.Messaging
{
    internal abstract class ContractConfigBase : IContractConfig
    {
        protected static ServiceModelSectionGroup _serviceModelGroup;
        protected static Dictionary<string, Type> _serviceContracts;
        protected static Dictionary<string, Type> _messageTypes;

        public ChannelEndpointElement FindEndpointByName(string channelEndpointName)
        {
            for (int i = 0; i < _serviceModelGroup.Client.Endpoints.Count; i++)
            {
                if (_serviceModelGroup.Client.Endpoints[i].Name == channelEndpointName)
                    return _serviceModelGroup.Client.Endpoints[i];
            }

            return null;
        }

        public Type GetChannelInterfaceType(ChannelEndpointElement channelEndpoint)
        {
            if (_serviceContracts.ContainsKey(channelEndpoint.Contract))
                return _serviceContracts[channelEndpoint.Contract];

            return null;
        }

        public Type FrameworkMessageTypeLookup(string messageXmlType)
        {
            if (_messageTypes.ContainsKey(messageXmlType))
                return _messageTypes[messageXmlType];

            return null;
        }

        protected abstract void Initialize();

        public static IContractConfig GetContractConfigInstance()
        {
            ContractConfigBase contractConfig = null;
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    contractConfig = new WebContractConfig();
                }
                else
                {
                    contractConfig = new ClientContractConfig();
                }
            }
            catch (Exception)
            {
                // If there was an exception trying to access HttpContext, then it's probably a Client application
                contractConfig = new ClientContractConfig();
            }

            if (contractConfig != null)
            {
                contractConfig.Initialize();
                return (IContractConfig)contractConfig;
            }

            return null;
        }

        protected void InitializeServiceLookups()
        {
            _serviceContracts = new Dictionary<string, Type>();
            _messageTypes = new Dictionary<string, Type>();

            // look through alreay loaded assemblies
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in loadedAssemblies)
            {
                try
                {
                    BrowseAssembly(assembly, _serviceContracts, _messageTypes);
                }
                catch (System.Reflection.ReflectionTypeLoadException) 
                {
                    // This error could happen in a partially trusted environment... just skip it and move on
                    continue;
                } 
            }

            // look through assemblies in the application directory but not yet loaded
            string path = System.IO.Path.GetDirectoryName(Assembly.GetAssembly(typeof(MessagingAdapter)).CodeBase).Replace(@"file:\", "");
            string[] dllFiles = Directory.GetFiles(path, "*.dll");

            foreach (string dllFile in dllFiles)
            {
                bool isDllLoaded = false;
                foreach (Assembly assembly in loadedAssemblies)
                {
                    try
                    {
                        FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
                        if (String.Compare(assemblyFileInfo.FullName, dllFile, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            isDllLoaded = true;
                            break;
                        }
                    }
                    catch (NotSupportedException)
                    {
                        // HACK: dynamic assemblies do not have a (disk) location and will throw an error
                        // don't care about those assemblies anyway
                        continue;
                    }
                    catch (System.ArgumentException)
                    {
                        // This error could happen in a partially trusted environment... just skip it and move on
                        continue;
                    }
                }

                if (!isDllLoaded)
                {
                    Assembly newAssembly = Assembly.LoadFile(dllFile);
                    BrowseAssembly(newAssembly, _serviceContracts, _messageTypes);
                }
            }
        }

        protected void BrowseAssembly(Assembly assembly, Dictionary<string, Type> serviceContracts, Dictionary<string, Type> messageTypes)
        {
            try
            {
                Type[] assemblyTypes = assembly.GetTypes();
                foreach (Type type in assemblyTypes)
                {
                    if (type.IsInterface)
                    {
                        ServiceContractAttribute[] attributes = (ServiceContractAttribute[])type.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                        if ((attributes != null) && (attributes.Length > 0))
                        {
                            string configurationName;
                            if (!String.IsNullOrEmpty(attributes[0].ConfigurationName))
                            {
                                configurationName = attributes[0].ConfigurationName;
                            }
                            else
                            {
                                configurationName = type.FullName;
                            }

                            if (!serviceContracts.ContainsKey(configurationName))
                            {
                                serviceContracts.Add(configurationName, type);
                            }
                            else
                            {
                                EventLogUtility.LogWarningMessage("Multiple service contracts with identical names were located.  The following contract will be ignored: " + configurationName + " in type " + type.FullName + " from assembly " + assembly.FullName);
                            }
                        }
                    }

                    if (typeof(FrameworkMessage).IsAssignableFrom(type))
                    {
                        string messageXmlType = FrameworkMessage.GetMessageXmlType(type);
                        if (!messageTypes.ContainsKey(messageXmlType))
                            messageTypes.Add(messageXmlType, type);
                    }
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = EventLogUtility.FormatExceptionMessage(ex);
                EventLogUtility.LogWarningMessage(String.Format("An exception occurred during the browsing of assemblies. This error is considered non-critical but may result in reduced functionality. Error details:\r\n{0}", exceptionMessage));
            }
        }
    }
}
