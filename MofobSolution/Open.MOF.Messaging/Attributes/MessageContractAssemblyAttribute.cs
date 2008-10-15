using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class MessageContractAssemblyAttribute : Attribute
    {
        public MessageContractAssemblyAttribute()
        {
        }
    }
}
