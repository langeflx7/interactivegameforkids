using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveGameForKids.utils
{
    /// <summary>
    /// Manages game settings, such as round duration and difficulty, using a singleton pattern.
    /// Settings are read from and written to an XML file.
    /// </summary>
    public class SettingsHandler
    {
        /// <summary>
        /// Singleton instance of the SettingsHandler class.
        /// </summary>
        private static SettingsHandler _instance;

        /// <summary>
        /// Path to the XML file where the game settings are stored.
        /// </summary>
        private const string FILE_PATH = "settings.xml";

        /// <summary>
        /// Duration of a round in seconds. This value is read from the settings XML file.
        /// </summary>
        public int roundDurationInSeconds { get; private set; }

        /// <summary>
        /// Difficulty level of the game. This value is read from the settings XML file.
        /// </summary>
        public Difficulty difficulty { get; private set; }

        /// <summary>
        /// Private constructor to enforce the singleton pattern and initialize settings.
        /// </summary>
        private SettingsHandler()
        {
            InitializeSettings();
        }

        /// <summary>
        /// Initializes the game settings by reading them from the XML file.
        /// </summary>
        private void InitializeSettings()
        {
            roundDurationInSeconds = ReadRoundDurationInSeconds();
            difficulty = ReadDifficulty();
        }

        /// <summary>
        /// Reads the round duration (in seconds) from the settings XML file.
        /// </summary>
        /// <returns>The round duration in seconds.</returns>
        private int ReadRoundDurationInSeconds()
        {
            return DataHandler.GetInstance().ReadFromXML<int>(FILE_PATH, "roundDuration");
        }

        /// <summary>
        /// Reads the difficulty level from the settings XML file and parses it into the `Difficulty` enum.
        /// </summary>
        /// <returns>The parsed `Difficulty` value. If parsing fails, the default value of `Difficulty` is returned.</returns>
        private Difficulty ReadDifficulty()
        {
            string difficultyString = DataHandler.GetInstance().ReadFromXML<string>(FILE_PATH, "difficulty");
            Enum.TryParse<Difficulty>(difficultyString, out Difficulty difficulty);
            return difficulty;
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
