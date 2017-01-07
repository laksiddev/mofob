using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Reflection;

namespace Open.MOF.Messaging
{
    internal class WebSecureContractConfig : ContractConfigBase
    {
        protected override void Initialize()
        {
            System.Configuration.Configuration config;
            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            _serviceModelGroup = ServiceModelSectionGroup.GetSectionGroup(config);

            _serviceContracts = new Dictionary<string, Type>();
            _messageTypes = new Dictionary<string, Type>();
        }
    }
}
