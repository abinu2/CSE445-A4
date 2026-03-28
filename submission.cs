using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Submission
    {
        // Update these URLs after deploying to GitHub Pages
        public static string xmlURL = "Your XML URL"; // Q1.2
        public static string xmlErrorURL = "Your Error XML URL"; // Q1.3
        public static string xsdURL = "Your XSD URL"; // Q1.1

        public static void Main(string[] args)
        {
            // Q3.1: Verify valid XML against schema
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Q3.2: Verify invalid XML against schema
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Q3.3: Convert valid XML to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1: Validates XML against XSD schema
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errors = "";

            // Load schema
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xsdUrl);

            // Configure reader with schema validation
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas = schemas;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += (sender, e) =>
            {
                errors += e.Message + "\n";
            };

            try
            {
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (XmlException ex)
            {
                errors += ex.Message + "\n";
            }

            if (string.IsNullOrEmpty(errors))
                return "No errors are found";
            else
                return errors.TrimEnd();
        }

        // Q2.2: Converts XML to JSON using Newtonsoft.Json
        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            // SerializeXmlNode handles attributes with @ prefix and repeated elements as arrays
            string jsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
            return jsonText;
        }
    }
}
