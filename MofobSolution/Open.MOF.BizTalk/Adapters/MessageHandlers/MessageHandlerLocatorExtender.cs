using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using Open.MOF.Messaging.Adapters;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    public class MessageHandlerLocatorExtender : ILocatorExtender
    {
        #region ILocatorExtender Members

        public void InitializeLocatorExtender(Microsoft.Practices.Unity.IUnityContainer container)
        {
            IMessageItineraryMapper mapper = new ServiceOrientedMessageItineraryMapper();
            container.RegisterInstance<IMessageItineraryMapper>(mapper, new ContainerControlledLifetimeManager());
        }

        #endregion
    }
}
