using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace VideoPlayerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        DispatcherTimer timer = new DispatcherTimer();
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            _player.PlayerReady += CreateDispatcherTimer;
            OpenMediaWindow.FilesSelectedEvent += EnableNextButton;
        }
        #endregion

        #region Buttons clicks
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.Source != null)
            {
                PlayButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Visible;
                _player.Play();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            _player.Pause();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.SourceController.Position > 0)
            {
                _player.Pause();
                PauseButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                if(_player.SourceController.Position == 1)
                    PreviousButton.IsEnabled = false;
                _player.PlayPrevious();
                NextButton.IsEnabled = true;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            _player.PlayNext(this, null);
            PreviousButton.IsEnabled = true;
            if (_player.SourceController.Position == (_player.SourceController.Count - 1))
            {
                NextButton.IsEnabled = false;
            }
        }

        private void MediaOpenBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenMediaWindow openMediaWindow = new OpenMediaWindow(_player);
            openMediaWindow.Show();
        }
        #endregion

        #region Timer
        private void CreateDispatcherTimer()
        {
            timer.Tick += SetSliderParameters;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }
        #endregion

        #region Slider
        private void SetSliderParameters(object sender, EventArgs e)
        {
            if (_player.GetDuration().HasTimeSpan)
            {
                durationSlider.Minimum = 0;
                durationSlider.Maximum = _player.GetDuration().TimeSpan.TotalSeconds;
                durationSlider.Value = _player.GetPosition();
            }
        }
        #endregion

        #region Methods attached to actions
        private void _player_Initialized(object sender, EventArgs e)
        {
            _player.HookEvents();
        }

        private void durationSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            timer.Stop();
        }

        private void durationSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _player._mediaPlayer.Position = TimeSpan.FromSeconds(durationSlider.Value);
            timer.Start();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _player._mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_player.IsPlaying)
            {
                _player.Play();
                PlayButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Visible;
            }
            else
            {
                _player.Pause();
                PauseButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
            }
        }

        private void grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_player._mediaPlayer.IsMuted)
                _player._mediaPlayer.IsMuted = false;
            else
                _player._mediaPlayer.IsMuted = true;
        }

        #endregion

        private void EnableNextButton()
        {
            if (_player.SourceController.Count > 1)
                NextButton.IsEnabled = true;
        }
    }
}
