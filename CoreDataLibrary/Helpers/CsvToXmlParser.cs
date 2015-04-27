using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreDataLibrary.Helpers
{
    public class CsvToXmlParser
    {
        private FileInfo m_csvFile;

        public CsvToXmlParser(FileInfo fileToConvert)
        {
            m_csvFile = fileToConvert;
            CreateXMLFile();
        }

        private void CreateXMLFile()
        {
            List<string> lines = System.IO.File.ReadAllLines(m_csvFile.FullName, Encoding.Default).ToList();
            List<string> headers = lines[0].Split('|').ToList();

            string fileDirectory = m_csvFile.DirectoryName + "\\";
            char[] charsToTrim = { '.', 'c', 's', 'v' };
            string xmlFileName = m_csvFile.Name.TrimEnd(charsToTrim);

            using (XmlWriter writer = XmlWriter.Create(fileDirectory + xmlFileName + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Properties");

                for (int i = 1; i < lines.Count - 1; i++)
                {
                    List<string> fields = lines[i].Split('|').ToList();

                    writer.WriteStartElement("Property");

                    for (int j = 0; j < headers.Count - 1; j++)
                    {
                        writer.WriteElementString(headers[j], fields[j].Replace("\0", string.Empty));
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
