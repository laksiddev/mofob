using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Open.MOF.SchemaReferenceUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE SchemaReferenceUtil path");
                return;
            }

            string pathToFiles = args[0];
            string[] filenames = Directory.GetFiles(pathToFiles, "*.xsd");
            Dictionary<XmlDocument, string> schemaDocuments = new Dictionary<XmlDocument,string>();
            Dictionary<string, string> schemaLookup = new Dictionary<string,string>();
            Dictionary<string, string> schemaAdd = new Dictionary<string, string>();
            foreach (string filename in filenames)
            {
                FileInfo fileinfo = new FileInfo(filename);
                XmlDocument doc = new XmlDocument();
                schemaDocuments.Add(doc, filename);
                doc.Load(filename);
                XmlNode docElement = doc.DocumentElement;
                XmlAttribute tnsAttribute = docElement.Attributes["xmlns:tns"];
                if (tnsAttribute != null)
                {
                    string schemaName = tnsAttribute.Value;
                    if (!schemaLookup.ContainsKey(schemaName))
                        schemaLookup.Add(schemaName, fileinfo.Name);
                    else
                        schemaAdd.Add(schemaName, fileinfo.Name);
                }
            }

            foreach (XmlDocument doc in schemaDocuments.Keys)
            {
                bool wasSchemaChanged = false;
                XmlNode docElement = doc.DocumentElement;
                foreach (XmlNode node in docElement.ChildNodes)
                {
                    if (node.Name == "xs:import")
                    {
                        string schemaName = node.Attributes["namespace"].Value;
                        if (node.Attributes["schemaLocation"] != null)
                        {
                            node.Attributes["schemaLocation"].Value = schemaLookup[schemaName];
                        }
                        else
                        {
                            XmlAttribute schemaLocationAttribute = doc.CreateAttribute("schemaLocation");
                            schemaLocationAttribute.Value = schemaLookup[schemaName];
                            node.Attributes.Append(schemaLocationAttribute);
                        }

                        wasSchemaChanged = true;
                    }
                }

                if (wasSchemaChanged)
                    doc.Save(schemaDocuments[doc]);
            }

            schemaDocuments.Clear();
        }
    }
}
