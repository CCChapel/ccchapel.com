using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CCC.Formatters.Xml
{
    public class NamespacedXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        const string xmlType = "application/xml";
        const string xmlType2 = "text/xml";

        public XmlSerializerNamespaces Namespaces { get; private set; }

        Dictionary<Type, XmlSerializer> Serializers { get; set; }

        public NamespacedXmlMediaTypeFormatter()
            : base()
        {
            this.Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add("itunes", "http://www.itunes.com/dtds/podcast-1.0.dtd");
            this.Serializers = new Dictionary<Type, XmlSerializer>();
        }

        public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            lock (this.Serializers)
            {
                if (!Serializers.ContainsKey(type))
                {
                    var serializer = new XmlSerializer(type);
                    //we add a new serializer for this type
                    this.Serializers.Add(type, serializer);
                }
            }

            return Task.Factory.StartNew(() =>
            {
                XmlSerializer serializer;
                lock (this.Serializers)
                {
                    serializer = Serializers[type];
                }

                var xmlWriter = new XmlTextWriter(writeStream, Encoding.UTF8);
                xmlWriter.Namespaces = true;
                serializer.Serialize(xmlWriter, value, this.Namespaces);
            });
        }
    }
}