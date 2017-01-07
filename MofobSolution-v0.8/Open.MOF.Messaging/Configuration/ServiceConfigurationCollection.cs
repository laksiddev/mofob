using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    public class ServiceConfigurationCollection : ConfigurationElementCollection
    {
        public ServiceConfigurationCollection()
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
            return new ServiceConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfigurationElement)element).Name;
        }

        public ServiceConfigurationElement this[int index]
        {
            get
            {
                return (ServiceConfigurationElement)BaseGet(index);
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

        new public ServiceConfigurationElement this[string Name]
        {
            get
            {
                return (ServiceConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(ServiceConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(ServiceConfigurationElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ServiceConfigurationElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Name);
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
