﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class TwoWayMessage : SimpleMessage
    {
        public override bool RequiresTwoWay
        {
            get { return true; }
        }
    }
}