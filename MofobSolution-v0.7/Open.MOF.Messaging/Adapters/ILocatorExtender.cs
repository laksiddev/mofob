using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Open.MOF.Messaging.Adapters
{
    public interface ILocatorExtender
    {
        void InitializeLocatorExtender(IUnityContainer container);
    }
}
