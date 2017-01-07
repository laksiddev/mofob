using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class TwoWayResponseMessage : SimpleMessage
    {
        public override bool RequiresTwoWay
        {
            get { return false; }
        }
    }
}
