using System;
using System.Runtime.CompilerServices;
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
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            _player.PlayerReady += CreateDispatcherTimer;
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

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += SetSliderParameters;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
        }
        #endregion

        #region Slider
        private void SetSliderParameters(object sender, EventArgs e)
        {
            durationSlider.Minimum = 0;
            durationSlider.Maximum = _player.GetDuration().TimeSpan.TotalSeconds;
            durationSlider.Value = _player.GetPosition();
            durationSlider.UpdateLayout();
        }
        #endregion

        private void _player_Initialized(object sender, EventArgs e)
        {
            _player.HookEvents();
        }

        private void durationSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _player._mediaPlayer.Position = TimeSpan.FromSeconds(durationSlider.Value);
        }

        private void durationSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _player._mediaPlayer.Position += (e.Delta > 0) ? TimeSpan.FromSeconds(0.1) : TimeSpan.FromSeconds(-0.1);
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _player._mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_player.IsPlaying)
                _player.Play();
            else
                _player.Pause();
        }

        private void grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_player._mediaPlayer.IsMuted)
                _player._mediaPlayer.IsMuted = false;
            else
                _player._mediaPlayer.IsMuted = true;
        }
    }
}
