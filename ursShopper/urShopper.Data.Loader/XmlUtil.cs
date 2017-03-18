using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace urShopper.Data.Loader
{
    public class XmlUtil
    {

        public static string ConvertObjectsToXml<T>(List<T> paramList)
        {
            try
            {
                var doc = new XDocument();
                var serializer = new System.Xml.Serialization.XmlSerializer(paramList.GetType());
                var writer = doc.CreateWriter();
                serializer.Serialize(writer, paramList);
                writer.Close();
                return doc.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public static List<T> ConvertXmlToObjects<T>(string RequestXml, string RootElementName = null)
        { 
            try
            {
                var typeName = typeof(T).ToString();
                if (string.IsNullOrEmpty(RootElementName))
                    typeName = typeName.Substring(typeName.LastIndexOfAny(new char[] { '.' }) + 1);
                else
                    typeName = RootElementName;

                var rootName = string.Format("{0}s", typeName);
                var doc = XDocument.Parse(RequestXml);
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootName));
                var reader = doc.CreateReader();
                var result = (List<T>)serializer.Deserialize(reader);
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
