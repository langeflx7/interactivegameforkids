using System;
using System.IO;
using System.Xml;

namespace InteractiveGameForKids.utils
{
    /// <summary>
    /// Provides singleton-based data management for saving, loading, and retrieving key-value pairs using XML serialization.
    /// </summary>
    public class DataHandler
    {
        // Singleton instance of DataHandler
        private static DataHandler _instance;

        /// <summary>
        /// Private constructor to enforce the singleton pattern and prevent external instantiation.
        /// </summary>
        private DataHandler() {}

        /// <summary>
        /// Saves a key-value pair to an XML file. If the key already exists, the value will be updated.
        /// </summary>
        /// <typeparam name="T">The type of the value to be saved (e.g., string, int, bool).</typeparam>
        /// <param name="filePath">The path to the XML file where the key-value pair will be saved.</param>
        /// <param name="key">The key for the value to be saved.</param>
        /// <param name="value">The value associated with the specified key.</param>
        /// <returns>Returns true if the operation is successful; otherwise, false.</returns>
        public bool SaveInXML<T>(string filePath, string key, T value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    xmlDoc.Load(filePath); // Load existing file
                }
                else
                {
                    // Create a new document if the file does not exist
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = xmlDoc.CreateElement("KeyValuePairs");
                    xmlDoc.AppendChild(xmlDeclaration);
                    xmlDoc.AppendChild(root);
                }

                // Get the root element
                XmlElement rootElement = xmlDoc.DocumentElement;

                // Check if the key already exists
                XmlNode existingNode = rootElement.SelectSingleNode($"//{key}");
                if (existingNode != null)
                {
                    // Update the value if the key exists
                    existingNode.InnerText = value.ToString();
                }
                else
                {
                    // Create and add a new element if the key does not exist
                    XmlElement newElement = xmlDoc.CreateElement(key);
                    newElement.InnerText = value.ToString();
                    rootElement.AppendChild(newElement);
                }

                // Save the file
                xmlDoc.Save(filePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to XML: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Reads a value of the specified type from an XML file using a given key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be read (e.g., string, int, bool).</typeparam>
        /// <param name="filePath">The path to the XML file to be read.</param>
        /// <param name="key">The key associated with the value to retrieve.</param>
        /// <returns>
        /// Returns the value associated with the specified key, converted to the specified type.
        /// If the key is not found or an error occurs, the default value of the specified type is returned.
        /// </returns>
        public T ReadFromXML<T>(string filePath, string key)
        {
            try
            {
                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified XML file was not found.");
                }

                // Load the XML document
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                // Search for the key in the root element
                XmlElement rootElement = xmlDoc.DocumentElement;
                XmlNode node = rootElement.SelectSingleNode($"//{key}");

                if (node == null)
                {
                    throw new Exception($"The key '{key}' was not found in the XML file.");
                }

                // Read the value and convert it to the specified type
                string value = node.InnerText;
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from XML: {ex.Message}");
                if(key.Equals("roundDuration"))
                {
                    int roundDurationDefault = 10;
                    SaveInXML<int>(filePath, key, roundDurationDefault);
                    return (T)Convert.ChangeType(roundDurationDefault, typeof(T));
                }
                return default(T); // Return the default value for the type T (e.g., null, 0, false)
            }
        }

        /// <summary>
        /// Provides a singleton instance of the DataHandler class.
        /// </summary>
        /// <returns>The singleton instance of the DataHandler.</returns>
        public static DataHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataHandler();
            }
            return _instance;
        }
    }
}
