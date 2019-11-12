using DocumentFormat.OpenXml.CustomXmlDataProperties;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using System;

namespace FormGenerator
{
    public class WordFormReportGenerator
    {
        public Stream GenerateDocument<TData>(string templateName, TData data)
        {
            var fullTemplateName = "FormGenerator.Templates." + templateName + ".docx";

            var assembly = GetType().Assembly;
            var templateStream = assembly.GetManifestResourceStream(fullTemplateName);
            using (templateStream)
            {
                return GenerateDocument(templateStream, data);
            }
        }

        private Stream GenerateDocument<TData>(Stream template, TData data)
        {
            byte[] serializedDataArray;

            using (var serializedDataStream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(TData));
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(serializedDataStream, data, namespaces);
                serializedDataArray = serializedDataStream.ToArray();
            }

            return GenerateDocument(template, serializedDataArray);
        }

        private Stream GenerateDocument(Stream template, byte[] serializedDataArray)
        {
            var outputDocumentBuffer = new MemoryStream();
            template.CopyTo(outputDocumentBuffer);
            outputDocumentBuffer.Seek(0, SeekOrigin.Begin);

            using (var outputDocument = WordprocessingDocument.Open(outputDocumentBuffer, true))
            {
                var xmlPart = GetAppropriateXmlPart(outputDocument, new MemoryStream(serializedDataArray, false));
                xmlPart.FeedData(new MemoryStream(serializedDataArray, false));
                outputDocument.Close();
            }

            outputDocumentBuffer.Seek(0, SeekOrigin.Begin);
            return outputDocumentBuffer;
        }

        private CustomXmlPart GetAppropriateXmlPart(WordprocessingDocument wordDocument, Stream serializedData)
        {
            var doc = XDocument.Load(serializedData);
            var namespaces = new HashSet<string>(doc.Descendants().Select(element => element.Name.NamespaceName).Distinct());

            foreach (var xmlPart in wordDocument.MainDocumentPart.CustomXmlParts)
            {
                var schemaRefs = xmlPart.CustomXmlPropertiesPart.DataStoreItem.SchemaReferences;
                foreach (SchemaReference schemaRef in schemaRefs)
                {
                    if (namespaces.Contains(schemaRef.Uri.Value))
                    {
                        return xmlPart;
                    }
                }
            }

            throw new Exception("Appropriate XmlPart is not found");
        }
    }
}