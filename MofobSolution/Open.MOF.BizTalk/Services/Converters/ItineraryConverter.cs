using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.BizTalk.Services
{
    public class ItineraryConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Open.MOF.Messaging.MessagingEndpoint))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(Open.MOF.Messaging.MessagingEndpoint))
            {
                Open.MOF.Messaging.MessagingEndpoint toEndpoint = (Open.MOF.Messaging.MessagingEndpoint)value;

                Open.MOF.BizTalk.Services.Proxy.Itinerary itinerary = new Open.MOF.BizTalk.Services.Proxy.Itinerary();

                // TODO provide converter implementation

                return itinerary;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
