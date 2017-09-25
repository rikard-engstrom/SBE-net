using System;
using System.Text;
using System.Xml;

namespace SBE.Core.Utils
{
    sealed class XmlHelper : IDisposable
    {
        private readonly XmlWriter writer;

        internal XmlHelper(string file)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                CheckCharacters = true,
            };

            writer = XmlWriter.Create(file, settings);
        }

        internal void StartDocument()
        {
            writer.WriteStartDocument();
        }

        internal void EndDocument()
        {
            writer.WriteEndDocument();
            writer.Flush();
        }

        internal void StartElement(string name)
        {
            writer.WriteStartElement(name);
        }
        
        internal void EndElement()
        {
            writer.WriteEndElement();
        }

        internal void CDataElementString(string name, string text)
        {
            writer.WriteStartElement(name);
            writer.WriteCData(text);
            writer.WriteEndElement();
        }

        internal void AttributeString(string name, string value)
        {
            writer.WriteAttributeString(name, value.ToString());
        }

        internal void ElementString(string name, string value)
        {
            writer.WriteElementString(name, value);
        }

        public void Dispose()
        {
            writer?.Dispose();
        }
    }
}
