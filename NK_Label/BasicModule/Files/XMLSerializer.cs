using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.IO;

namespace BasicModule.Files
{
    internal class XMLSerializer
    {
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

        #endregion //Save

        #region Open

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

        #endregion //Open
    }
}
