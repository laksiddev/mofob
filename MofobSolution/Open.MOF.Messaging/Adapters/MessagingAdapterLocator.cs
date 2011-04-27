using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Adapters
{
    public class MessagingAdapterLocator : ServiceLocatorImplBase, IDisposable
    {
        private static MessagingAdapterLocator _locatorStaticInstance;
        public static IServiceLocator GetLocatorInstance()
        {
            if (_locatorStaticInstance == null)
            {
                _locatorStaticInstance = new MessagingAdapterLocator();
                _locatorStaticInstance.InitializeLocator();
            }

            return _locatorStaticInstance;
        }

        private IUnityContainer _container = null;
       
        public MessagingAdapterLocator()
        {
        }

        private void InitializeLocator()
        {
            _container = new UnityContainer();

            ILocatorExtender extender = new MessagingAdapterLocatorExtender();
            extender.InitializeLocatorExtender(_container);

            extender = new Callback.CallbackLocatorExtender();
            extender.InitializeLocatorExtender(_container);

            // The rest of this discovery process broke in SharePoint
            //// look through alreay loaded assemblies
            //Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            //foreach (Assembly assembly in loadedAssemblies)
            //{
            //    BrowseAssembly(assembly, _container);
            //}

            //// look through assemblies in the application directory but not yet loaded
            //string path = System.IO.Path.GetDirectoryName(Assembly.GetAssembly(typeof(MessagingAdapter)).CodeBase).Replace(@"file:\", "");
            //string[] dllFiles = Directory.GetFiles(path, "*.dll");

            //foreach (string dllFile in dllFiles)
            //{
            //    bool isDllLoaded = false;
            //    foreach (Assembly assembly in loadedAssemblies)
            //    {
            //        FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
            //        if (String.Compare(assemblyFileInfo.FullName, dllFile, StringComparison.CurrentCultureIgnoreCase) == 0)
            //        {
            //            isDllLoaded = true;
            //            break;
            //        }
            //    }

            //    if (!isDllLoaded)
            //    {
            //        Assembly newAssembly = Assembly.LoadFile(dllFile);
            //        BrowseAssembly(newAssembly, _container);
            //    }
            //}

            ServiceLocator.SetLocatorProvider(new ServiceLocatorProvider(MessagingAdapterLocator.GetLocatorInstance));
        }

        //private void BrowseAssembly(Assembly assembly, IUnityContainer container)
        //{
        //    Type[] assemblyTypes = assembly.GetTypes();
        //    foreach (Type type in assemblyTypes)
        //    {
        //        if (typeof(ILocatorExtender).IsAssignableFrom(type))
        //        {
        //            ConstructorInfo constructor = type.GetConstructor(new Type[0]);
        //            if (constructor != null)
        //            {
        //                ILocatorExtender locatorExtender = (ILocatorExtender)constructor.Invoke(new object[0]);
        //                locatorExtender.InitializeLocatorExtender(container);
        //            }
        //        }
        //    }
        //}
        
        /// <summary>
        ///             When implemented by inheriting classes, this method will do the actual work of resolving
        ///             the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            try
            {
                return _container.Resolve(serviceType, key);
            }
            catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore - return null */ }

            return null;
        }

        /// <summary>
        ///             When implemented by inheriting classes, this method will do the actual work of
        ///             resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore - return null */ }

            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _container = null;
        }

        #endregion
    }
}
