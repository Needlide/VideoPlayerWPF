using System;
using System.Windows;
using System.Windows.Input;

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

        #region Player
        //private void AddPlayerToViewBox()
        //{
        //    _player.Width = viewbox.ActualWidth;
        //    _player.Height = viewbox.ActualHeight;
        //    viewbox.Child = _player;
        //    _player.InvalidateMeasure();
        //    _player.InvalidateArrange();
        //    _player.InvalidateVisual();
        //}
        #endregion

        private void _player_Initialized(object sender, EventArgs e)
        {
            _player.HookEvents();
        }
    }
}
