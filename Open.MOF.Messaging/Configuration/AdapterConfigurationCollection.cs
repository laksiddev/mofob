using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    public class AdapterConfigurationCollection : ConfigurationElementCollection
    {
        public AdapterConfigurationCollection()
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
            return new AdapterConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AdapterConfigurationElement)element).Name;
        }

        public AdapterConfigurationElement this[int index]
        {
            get
            {
                return (AdapterConfigurationElement)BaseGet(index);
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

        new public AdapterConfigurationElement this[string Name]
        {
            get
            {
                return (AdapterConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(AdapterConfigurationElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(AdapterConfigurationElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(AdapterConfigurationElement element)
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
