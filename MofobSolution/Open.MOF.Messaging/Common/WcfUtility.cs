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

    public static class WcfUtility
    {
        private static ServiceModelSectionGroup _serviceModelGroup;
        private static Dictionary<string, Type> _serviceContracts;
        private static Dictionary<string, Type> _messageTypes;

        public static ChannelEndpointElement FindEndpointByName(string channelEndpointName)
        {
            Initialize();

            for (int i = 0; i < _serviceModelGroup.Client.Endpoints.Count; i++)
            {
                if (_serviceModelGroup.Client.Endpoints[i].Name == channelEndpointName)
                    return _serviceModelGroup.Client.Endpoints[i];
            }

            return null;
        }

        public static Type GetChannelInterfaceType(ChannelEndpointElement channelEndpoint)
        {
            Initialize();

            if (_serviceContracts.ContainsKey(channelEndpoint.Contract))
                return _serviceContracts[channelEndpoint.Contract];

            return null;
        }

        public static bool DoesAddressMatchBinding(string addressUri, string bindingType)
        {
            if (((addressUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) ||
                (addressUri.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))) &&
                ((bindingType == "wsHttpBinding") || (bindingType == "basicHttpBinding")))
            {
                return true;
            }
            else if ((addressUri.StartsWith("net.tcp://", StringComparison.CurrentCultureIgnoreCase)) &&
                (bindingType == "netTcpBinding"))
            {
                return true;
            }

            return false;
        }

        public static Type FrameworkMessageTypeLookup(string messageXmlType)
        {
            if (_messageTypes.ContainsKey(messageXmlType))
                return _messageTypes[messageXmlType];

            return null;
        }

        private static void Initialize()
        {
            if (_serviceModelGroup == null)
            {
                _serviceModelGroup = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            }

            if ((_serviceContracts == null) || (_messageTypes == null))
            {
                _serviceContracts = new Dictionary<string, Type>();
                _messageTypes = new Dictionary<string, Type>();

                // look through alreay loaded assemblies
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in loadedAssemblies)
                {
                    BrowseAssembly(assembly, _serviceContracts, _messageTypes);
                }

                // look through assemblies in the application directory but not yet loaded
                string path = System.IO.Path.GetDirectoryName(Assembly.GetAssembly(typeof(MessagingAdapter)).CodeBase).Replace(@"file:\", "");
                string[] dllFiles = Directory.GetFiles(path, "*.dll");

                foreach (string dllFile in dllFiles)
                {
                    bool isDllLoaded = false;
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
                        if (String.Compare(assemblyFileInfo.FullName, dllFile, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            isDllLoaded = true;
                            break;
                        }
                    }

                    if (!isDllLoaded)
                    {
                        Assembly newAssembly = Assembly.LoadFile(dllFile);
                        BrowseAssembly(newAssembly, _serviceContracts, _messageTypes);
                    }
                }
            }
        }

        private static void BrowseAssembly(Assembly assembly, Dictionary<string, Type> serviceContracts, Dictionary<string, Type> messageTypes)
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
    }
}
