using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveGameForKids.utils
{
    class SettingsHandler
    {

        // Singleton instance of SettingsHandler
        private static SettingsHandler _instance;

        // Path to the file where settings are stored as XML
        private const string FILE_PATH = "settings.xml";

        /// <summary>
        /// Private constructor to prevent external instantiation and enforce singleton pattern.
        /// </summary>
        private SettingsHandler() { }

        /// <summary>
        /// Gets the round duration time in seconds
        /// </summary>
        /// <returns>The round duration time as integer in seconds</returns>
        public int GetRoundDurationInSeconds()
        {
            return DataHandler.GetInstance().ReadFromXML<int>(FILE_PATH, "roundDuration");
        }

        /// <summary>
        /// Provides a singleton instance of the SettingsHandler class.
        /// </summary>
        /// <returns>The singleton instance of SettingsHandler.</returns>
        public static SettingsHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SettingsHandler();
            }
            return _instance;
        }

    }
}
