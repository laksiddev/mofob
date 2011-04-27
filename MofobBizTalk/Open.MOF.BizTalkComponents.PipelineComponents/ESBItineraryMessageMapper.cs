namespace Open.MOF.BizTalkComponents.PipelineComponents
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Drawing;
    using System.Resources;
    using System.Reflection;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.BizTalk.Message.Interop;
    using Microsoft.BizTalk.Component.Interop;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Messaging;
    
    
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("bf80409c-9547-448a-9d96-931b9690b902")]
    [ComponentCategory(CategoryTypes.CATID_PartyResolver)]
    public class ESBItineraryMessageMapper : Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {
        private const string __messageTypeContextPropertyName = "MessageType";
        private const string __messageTypeContextPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
        private const string __itineraryDescriptionContextPropertyName = "ItineraryDescription";
        private const string __itineraryDescriptionContextPropertyNamespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary";
        private const string __esbInstallPath32bit = @"C:\Program Files\Microsoft BizTalk ESB Toolkit 2.1\esb.config";
        private const string __esbInstallPath64bit = @"C:\Program Files (x86)\Microsoft BizTalk ESB Toolkit 2.1\esb.config";
        private const string __itineraryLookupSql = "SELECT ItineraryName, ItineraryVersion FROM dbo.MessageItineraryMapping WHERE MessageDescriptor=@MessageDescriptor";
        private const string __messageTypeParameterName = "@MessageDescriptor";
        
        private System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("Open.MOF.BizTalkComponents.PipelineComponents.ESBItineraryMessageMapper", Assembly.GetExecutingAssembly());
        
        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return resourceManager.GetString("COMPONENTNAME", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get
            {
                return resourceManager.GetString("COMPONENTVERSION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return resourceManager.GetString("COMPONENTDESCRIPTION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        #endregion
        
        #region IPersistPropertyBag members
        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classid">
        /// Class ID of the component
        /// </param>
        public void GetClassID(out System.Guid classid)
        {
            classid = new System.Guid("bf80409c-9547-448a-9d96-931b9690b902");
        }
        
        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }
        
        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, int errlog)
        {
        }
        
        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
        }
        
        #region utility functionality
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (System.ArgumentException )
            {
                return val;
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
            return val;
        }
        
        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }
        #endregion
        #endregion
        
        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return ((System.Drawing.Bitmap)(this.resourceManager.GetObject("COMPONENTICON", System.Globalization.CultureInfo.InvariantCulture))).GetHicon();
            }
        }
        
        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public System.Collections.IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add("This is a compiler error");
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion
        
        #region IComponent members
        /// <summary>
        /// Implements IComponent.Execute method.
        /// </summary>
        /// <param name="pc">Pipeline context</param>
        /// <param name="inmsg">Input message</param>
        /// <returns>Original input message</returns>
        /// <remarks>
        /// IComponent.Execute method is used to initiate
        /// the processing of the message in this pipeline component.
        /// </remarks>
        public Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            if (inmsg.Context.Read(__itineraryDescriptionContextPropertyName, __itineraryDescriptionContextPropertyNamespace) == null)
            {
                if (inmsg.Context.Read(__messageTypeContextPropertyName, __messageTypeContextPropertyNamespace) is string)
                {
                    string messageType = (string)inmsg.Context.Read(__messageTypeContextPropertyName, __messageTypeContextPropertyNamespace);

                    string itineraryName;
                    string itineraryVersion;

                    ReadItineraryForMessage(messageType, out itineraryName, out itineraryVersion);

                    if (!String.IsNullOrEmpty(itineraryName))
                    {
                        string itineraryDescriptionXml = GetItineraryDescriptionXml(itineraryName, itineraryVersion);
                        inmsg.Context.Write(__itineraryDescriptionContextPropertyName, __itineraryDescriptionContextPropertyNamespace, itineraryDescriptionXml);
                    }
                }
            }

            return inmsg;
        }
        #endregion

        private static Dictionary<string, string[]> _itineraryCatalog = null;
        private void ReadItineraryForMessage(string messageType, out string itineraryName, out string itineraryVersion)
        {
            if (_itineraryCatalog == null)
                _itineraryCatalog = new Dictionary<string, string[]>();

            if (_itineraryCatalog.ContainsKey(messageType))
            {
                itineraryName = _itineraryCatalog[messageType][0];
                itineraryVersion = _itineraryCatalog[messageType][1];
                return;
            }

            itineraryName = null;
            itineraryVersion = null;

            string itineraryConnectionString = GetEsbItineraryConnectionString();

            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(itineraryConnectionString))
            {
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(__itineraryLookupSql, sqlConnection);
                System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(__messageTypeParameterName, System.Data.SqlDbType.NVarChar, 1024);
                sqlParameter.Value = messageType;
                sqlCommand.Parameters.Add(sqlParameter);

                sqlConnection.Open();
                using (System.Data.SqlClient.SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            itineraryName = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            itineraryVersion = reader.GetString(1);
                    }
                }
            }

            if (itineraryName != null)
            {
                string[] itineraryDescriptors = new string[2];
                itineraryDescriptors[0] = itineraryName;
                itineraryDescriptors[1] = itineraryVersion;

                _itineraryCatalog.Add(messageType, itineraryDescriptors);
            }
        }

        private string GetItineraryDescriptionXml(string itineraryName, string itineraryVersion)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("ItineraryDescription", "http://schemas.microsoft.biztalk.practices.esb.com/itinerary"));

            XmlElement childElement = doc.CreateElement("Name", "http://schemas.microsoft.biztalk.practices.esb.com/itinerary");
            childElement.InnerText = itineraryName;
            doc.DocumentElement.AppendChild(childElement);

            if (!String.IsNullOrEmpty(itineraryVersion))
            {
                childElement = doc.CreateElement("Version", "http://schemas.microsoft.biztalk.practices.esb.com/itinerary");
                childElement.InnerText = itineraryVersion;
                doc.DocumentElement.AppendChild(childElement);
            }

            return doc.OuterXml;
        }

        private static string _itineraryConnectionString = null;
        private string GetEsbItineraryConnectionString()
        {
            if (_itineraryConnectionString == null)
            {
                // HACK Replace with code to find esb config file dynamically
                string esbInstallPath = __esbInstallPath32bit;
                if (!System.IO.File.Exists(esbInstallPath))
                {
                    esbInstallPath = __esbInstallPath64bit;
                    if (!System.IO.File.Exists(esbInstallPath))
                    {
                        throw new ApplicationException("Could not locate the ESB Toolkit configuration file.");
                    }
                }

                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(new System.Configuration.ConfigurationFileMap(esbInstallPath));
                if ((config != null) && (config.Sections["connectionStrings"] != null))
                {
                    System.Configuration.DefaultSection section = (System.Configuration.DefaultSection)config.Sections["connectionStrings"];

                    XmlDocument configDoc = new XmlDocument();
                    configDoc.LoadXml(section.SectionInformation.GetRawXml());
                    XmlNode node = configDoc.SelectSingleNode(@"/connectionStrings/add[@name='ItineraryDb']/@connectionString");
                    if ((node != null) && (node is XmlAttribute))
                    {
                        _itineraryConnectionString = ((XmlAttribute)node).Value;
                    }
                }
                if (String.IsNullOrEmpty(_itineraryConnectionString))
                {
                    throw new ApplicationException("Could not locate the ESB itinerary connection string.");
                }
            }

            return _itineraryConnectionString;
        }
    }
}
