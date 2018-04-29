using BasicModule.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Xml.Serialization;
namespace BasicModule.Utils
{
    internal class XMLSerializer
    {
        static readonly string DICTIONARY = "Dictionary";
        static readonly string KEY = "Key";
        static readonly string VALUE = "Value";
        static readonly string KEYVALUEPAIR = KEY+VALUE+ "Pair";

        #region Save

        internal static bool Serialize(object obj, string path, ref string message)
        {
            try
            {
                using (var writer = XmlWriter.Create(path, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }))
                {
                    var xnameSpace = new XmlSerializerNamespaces();
                    xnameSpace.Add("", "");
                    var serializer = new XmlSerializer(obj.GetType(), "");
                    serializer.Serialize(writer, obj, xnameSpace);
                    return true;
                }
            }
            catch (Exception e)
            {
                message = e.Message + Environment.NewLine + e.StackTrace;
            }
            return false;
        }

        public static XElement DictionaryToXml(ObservableDictionary<string, string> inputDictionary)
        {
            var ret = new XElement(DICTIONARY);
            foreach (var pair in inputDictionary)
            {
                XElement inner = new XElement(KEYVALUEPAIR);
                inner.Add(new XAttribute(KEY, pair.Key));
                inner.Add(new XAttribute(VALUE, pair.Value));
                ret.Add(inner);
            }
            return ret;
        }

        #endregion Save

        #region Open

        internal static int GetFileVersion(string path, ref string message)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    reader.ReadLine();
                    var line = reader.ReadLine();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(line);

                    var dataList = xmlDoc.GetElementsByTagName("FileVersion");
                    return int.Parse(dataList[0].InnerXml); ;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return 0;
        }

        internal static object Deserializer(Type type, string path, ref string message)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    XmlSerializer deserializer = new XmlSerializer(type);
                    return deserializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return null;
        }

        public static ObservableDictionary<string, string> XmlToDictionary(XElement inputXElement)
        {
            var ret = new ObservableDictionary<string, string>();

            foreach (XElement element in inputXElement.Elements())
                ret.Add(element.Attribute(KEY).Value, element.Attribute(VALUE).Value);

            return ret;
        }

        #endregion Open
    }
}