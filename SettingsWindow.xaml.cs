using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InteractiveGameForKids.utils;

namespace InteractiveGameForKids
{
    public partial class SettingsWindow : Window
    {
        public event Action<Difficulty> DifficultyChanged;
        public event Action<int> RoundDurationChanged;

        public SettingsWindow()
        {
            InitializeComponent();

            LoadDefaults();
            RegisterSignals();
        }

        private void LoadDefaults()
        {
            SetDifficulty(SettingsHandler.GetInstance().difficulty);
            SetRoundDuration(SettingsHandler.GetInstance().roundDurationInSeconds);
        }

        private void RegisterSignals()
        {
            DifficultyChanged += difficulty =>
            {
                SettingsHandler.GetInstance().UpdateDifficulty(difficulty);
            };

            RoundDurationChanged += duration =>
            {
                SettingsHandler.GetInstance().UpdateRoundDurationInSeconds(duration);
            };
        }

        public void SetDifficulty(Difficulty currentDifficulty)
        {
            foreach (ComboBoxItem item in DifficultyComboBox.Items)
            {
                if (item.Content.ToString() == currentDifficulty.ToString())
                {
                    DifficultyComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        public void SetRoundDuration(int duration)
        {
            RoundDurationTextBox.Text = duration.ToString();
        }
        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DifficultyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (Enum.TryParse(selectedItem.Content.ToString(), out Difficulty difficulty))
                {
                    DifficultyChanged?.Invoke(difficulty);
                }
            }
        }
        private void RoundDurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(RoundDurationTextBox.Text, out int duration))
            {
                RoundDurationChanged?.Invoke(duration);
                RoundDurationTextBox.Background = Brushes.White;
            }
            else
            {
                if(RoundDurationTextBox.Text.Length == 0)
                {
                    RoundDurationChanged?.Invoke(0);
                }
                else
                {
                    RoundDurationTextBox.Background = Brushes.Red;
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
