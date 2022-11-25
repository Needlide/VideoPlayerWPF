using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoPlayerWPF.Controls
{
    internal class Player : Control
    {
        #region Dependency properties
        static readonly DependencyProperty _sourceProperty = DependencyProperty.RegisterAttached("Source", typeof(Uri), typeof(Player));
        static readonly DependencyProperty _volumeProperty = DependencyProperty.RegisterAttached("Volume", typeof(double), typeof(Player));
        static readonly DependencyProperty _balanceProperty = DependencyProperty.RegisterAttached("Balance", typeof(double), typeof(Player));
        static readonly DependencyProperty _isPlayingProperty = DependencyProperty.RegisterAttached("IsPlaying", typeof(bool), typeof(Player));
        static readonly DependencyProperty _loadedBehaviorProperty = DependencyProperty.RegisterAttached("LoadedBehaviorProperty", typeof(MediaState), typeof(Player));
        static readonly DependencyProperty _unloadedBehaviorProperty = DependencyProperty.RegisterAttached("UnloadedBehaviorProperty", typeof(MediaState), typeof(Player));
        static readonly DependencyProperty _positionProperty = DependencyProperty.RegisterAttached("Position", typeof(double), typeof(Player));
        static readonly DependencyProperty _maximumProperty = DependencyProperty.RegisterAttached("Maximum", typeof(double), typeof(Player));
        #endregion

        #region Properties
        internal Uri Source { get => (Uri)GetValue(_sourceProperty); set => SetValue(_sourceProperty, value); }
        internal double Volume { get => (double)GetValue(_volumeProperty); set => SetValue(_volumeProperty, value); }
        internal double Balance { get => (double)GetValue(_balanceProperty); set => SetValue(_balanceProperty, value); }
        internal double Position { get => (double)GetValue(_positionProperty); set => SetValue(_positionProperty, value); }
        internal double Maximum { get => (double)GetValue(_maximumProperty); set => SetValue(_maximumProperty, _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds); }
        internal bool IsPlaying { get => (bool)GetValue(_isPlayingProperty); set => SetValue(_isPlayingProperty, value); }
        internal MediaState LoadedBehavior { get => (MediaState)GetValue(_loadedBehaviorProperty); set => SetValue(_loadedBehaviorProperty, value); }
        internal MediaState UnloadedBehavior { get => (MediaState)GetValue(_unloadedBehaviorProperty); set => SetValue(_unloadedBehaviorProperty, value); }
        #endregion

        #region Events
        internal delegate void PlayerReadyHandler();
        delegate void PlayNextRequested(object sender, RoutedEventArgs e);
        delegate void PlayPreviousRequested(object sender, RoutedEventArgs e);
        internal event PlayerReadyHandler PlayerReady;
        event PlayNextRequested _playNextEvent;
        event PlayPreviousRequested _playPreviousEvent;
        #endregion

        #region Fields
        internal readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        #endregion

        #region Methods
        internal void Play()
        {
            IsPlaying = true;
            _mediaPlayer.Play(); 
        }

        internal void Pause()
        {
            IsPlaying = false;
            _mediaPlayer.Pause(); 
        }

        internal void HookEvents()
        {
            _mediaPlayer.MediaEnded += PlayNext;
            _mediaPlayer.MediaOpened += GetPlayerReady;
            _playNextEvent += OpenMedia;
            _playPreviousEvent += OpenMedia;
        }

        internal void OpenMedia(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaPlayer.Close();
                _mediaPlayer.Open(Source);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Link is not supported", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        internal void GetPlayerReady(object sender, EventArgs e)
        {
            VideoDrawing videoDrawing = new VideoDrawing
            {
                Player = _mediaPlayer,
                Rect = new Rect(0, 0, 1, 1),
            };

            DrawingBrush drawingBrush = new DrawingBrush
            {
                Drawing = videoDrawing,
                Viewbox = new Rect(0, 0, 1, 1),
                Viewport = new Rect(0, 0, 1, 1),
            };
            Background = drawingBrush;

            PlayerReady?.Invoke();
        }

        internal void PlayNext(object sender, EventArgs e)
        {
            SourceController.MoveNext();
            Source = SourceController.GetSource();
            _playNextEvent.Invoke(this, null);
        }

        internal void PlayPrevious()
        {
            SourceController.MovePrevious();
            Source = SourceController.GetSource();
            _playPreviousEvent.Invoke(this, null);
        }

        internal Duration GetDuration()
        {
            return _mediaPlayer.NaturalDuration;
        }

        internal double GetPosition()
        {
            return _mediaPlayer.Position.TotalSeconds;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawVideo(_mediaPlayer, new Rect(default, RenderSize));
        }
        #endregion

        #region SourceController
        internal SourceController SourceController = new SourceController();
        #endregion
    }
}
