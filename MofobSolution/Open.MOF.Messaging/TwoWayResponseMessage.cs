using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    internal class TwoWayResponseMessage : SimpleMessage
    {
        public override bool RequiresTwoWay
        {
            get { return false; }
        }
    }
}
