using System.Windows;
using InteractiveGameForKids.utils;

namespace InteractiveGameForKids
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartMenu.Visibility = Visibility.Collapsed;
            GameArea.Visibility = Visibility.Visible;
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.SetDifficulty(SettingsHandler.GetInstance().difficulty);
            settingsWindow.SetRoundDuration(SettingsHandler.GetInstance().roundDurationInSeconds);
            settingsWindow.DifficultyChanged += difficulty =>
            {
                SettingsHandler.GetInstance().UpdateDifficulty(difficulty);
                MessageBox.Show($"Schwierigkeit geändert zu: {difficulty}");
            };

            settingsWindow.RoundDurationChanged += duration =>
            {
                SettingsHandler.GetInstance().UpdateRoundDurationInSeconds(duration);
                MessageBox.Show($"Rundendauer geändert zu: {duration} Sekunden");
            };
            settingsWindow.ShowDialog();
        }
    }
}
