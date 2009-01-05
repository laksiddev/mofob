using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Test;

namespace Open.MOF.Messaging.Test.WcfService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface ITestDataService
    {
        [OperationContract]
        TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage messsage);
    }
}
