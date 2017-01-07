using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using Open.MOF.Messaging.Adapters;

namespace Open.MOF.Messaging.Callback
{
    public class CallbackLocatorExtender : ILocatorExtender
    {
        #region ILocatorExtender Members

        public void InitializeLocatorExtender(IUnityContainer container)
        {
            WcfCallbackHost callbackHost = new WcfCallbackHost();
            container.RegisterInstance<ICallbackHost>(callbackHost, new ContainerControlledLifetimeManager());

            WcfPubSubCallbackAdapter callbackAdapter = new WcfPubSubCallbackAdapter();
            container.RegisterInstance<IPubSubCallbackAdapter>(callbackAdapter, new ContainerControlledLifetimeManager());
        }

        #endregion
    }
}
