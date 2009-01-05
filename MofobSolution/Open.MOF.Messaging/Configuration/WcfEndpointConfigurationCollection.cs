using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    public class WcfEndpointConfigurationCollection : ConfigurationElementCollection
    {
        public WcfEndpointConfigurationCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WcfEndpointConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((WcfEndpointConfigurationElement)element).BindingName;
        }

        public WcfEndpointConfigurationElement this[int index]
        {
            get
            {
                return (WcfEndpointConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public WcfEndpointConfigurationElement this[string Name]
        {
            get
            {
                return (WcfEndpointConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(WcfEndpointConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(WcfEndpointConfigurationElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(WcfEndpointConfigurationElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.BindingName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
