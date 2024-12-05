using InteractiveGameForKids.utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace InteractiveGameForKids
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer;
        private DispatcherTimer pigTimer;
        private int roundDuration;
        private Random random = new Random();
        private Difficulty difficulty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var newBackground = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/SpielAfterStartBild.png")),
                Stretch = Stretch.UniformToFill
            };
            this.Background = newBackground;

            roundDuration = SettingsHandler.GetInstance().roundDurationInSeconds;
            difficulty = SettingsHandler.GetInstance().difficulty;

            StartMenu.Visibility = Visibility.Collapsed;
            GameArea.Visibility = Visibility.Visible;

            StartGame();
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.SetDifficulty(SettingsHandler.GetInstance().difficulty);
            settingsWindow.SetRoundDuration(SettingsHandler.GetInstance().roundDurationInSeconds);
            settingsWindow.DifficultyChanged += difficulty =>
            {
                SettingsHandler.GetInstance().UpdateDifficulty(difficulty);
            };

            settingsWindow.RoundDurationChanged += duration =>
            {
                SettingsHandler.GetInstance().UpdateRoundDurationInSeconds(duration);
            };
            settingsWindow.ShowDialog();
        }
    private void StartGame()
        {
            // Game timer for the round duration
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(roundDuration);
            gameTimer.Tick += (s, e) => EndGame();
            gameTimer.Start();

            // Pig timer for spawning pigs
            pigTimer = new DispatcherTimer();
            pigTimer.Interval = GetPigSpawnInterval(difficulty);
            pigTimer.Tick += (s, e) => SpawnPig();
            pigTimer.Start();
        }

        private TimeSpan GetPigSpawnInterval(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.EASY:
                    return TimeSpan.FromSeconds(2);
                case Difficulty.NORMAL:
                    return TimeSpan.FromSeconds(1.5);
                case Difficulty.HARD:
                    return TimeSpan.FromSeconds(1);
                default:
                    return TimeSpan.FromSeconds(2);
            }
        }

        private void SpawnPig()
        {
            Button pigButton = new Button
            {
                FontSize = 30,
                Background = Brushes.Pink,
                BorderBrush = Brushes.Black,
                
                BorderThickness = new Thickness(2),
                Width = 150,
                Height = 150
            };
            Image pigImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/SpielSchweinchenBild.png")),
                Stretch = Stretch.Uniform
            };
            pigButton.Content = pigImage;

            double x = random.Next(0, (int)(PigCanvas.ActualWidth - pigButton.Width));
            double y = random.Next(0, (int)(PigCanvas.ActualHeight - pigButton.Height));
            Canvas.SetLeft(pigButton, x);
            Canvas.SetTop(pigButton, y);

            pigButton.Click += (s, e) =>
            {
                PigCanvas.Children.Remove(pigButton);
            };

            PigCanvas.Children.Add(pigButton);

            int disappearTime;
            switch (difficulty)
            {
                case Difficulty.EASY:
                    disappearTime = 3000;
                    break;
                case Difficulty.NORMAL:
                    disappearTime = 2000;
                    break;
                case Difficulty.HARD:
                    disappearTime = 1000;
                    break;
                default:
                    disappearTime = 3000;
                    break;
            }

            DispatcherTimer pigDisappearTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(disappearTime)
            };
            pigDisappearTimer.Tick += (s, e) =>
            {
                PigCanvas.Children.Remove(pigButton);
                pigDisappearTimer.Stop();
            };
            pigDisappearTimer.Start();
        }

        private void EndGame()
        {
            gameTimer.Stop();
            pigTimer.Stop();
            PigCanvas.Children.Clear();
            StartMenu.Visibility = Visibility.Visible;
            GameArea.Visibility = Visibility.Collapsed;
        }
    }
}
