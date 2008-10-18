using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class ExceptionDetailConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(System.Exception).IsAssignableFrom(sourceType))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            ExceptionDetail result = null;
            if (typeof(System.Exception).IsAssignableFrom(value.GetType()))
            {
                Exception itemToConvert = (Exception)value;
                ExceptionDetail innerDetail = null;
                if (itemToConvert.InnerException != null)
                {
                    innerDetail = (ExceptionDetail)ConvertFrom(context, culture, itemToConvert.InnerException);
                }

                string targetSite = ((itemToConvert.TargetSite != null) ? itemToConvert.TargetSite.Name : String.Empty);
                result = new ExceptionDetail(itemToConvert.Message, itemToConvert.GetType().FullName, itemToConvert.Source, targetSite, itemToConvert.StackTrace, innerDetail);
            }

            return result;
        }
   }
}
