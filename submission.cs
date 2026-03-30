using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://raw.githubusercontent.com/abinu2/CSE445-A4/main/NationalParks.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/abinu2/CSE445-A4/main/NationalParksErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/abinu2/CSE445-A4/main/NationalParks.xsd";

        public static void Main(string[] args)
        {
            // Verify valid XML against schema
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Verify error XML against schema
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Convert valid XML to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No errors are found" if XML is valid. Otherwise, return the desired exception message.
            string errors = "";

            try
            {
                // Load the schema and set up validation settings
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemas;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors += e.Message + "\n";
                };

                // Read through the XML to trigger validation
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                errors += ex.Message + "\n";
            }

            if (string.IsNullOrEmpty(errors))
                return "No errors are found";
            else
                return errors.TrimEnd();
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be deserializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            // Serialize the XML document to JSON
            string jsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
            return jsonText;
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}
