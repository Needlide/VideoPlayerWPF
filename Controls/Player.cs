using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoPlayerWPF.Controls
{
    internal class Player : Control
    {
        #region Dependency properties
        internal static readonly DependencyProperty _sourceProperty = DependencyProperty.RegisterAttached("Source", typeof(Uri), typeof(Player));
        internal static readonly DependencyProperty _volumeProperty = DependencyProperty.RegisterAttached("Volume", typeof(double), typeof(Player));
        internal static readonly DependencyProperty _balanceProperty = DependencyProperty.RegisterAttached("Balance", typeof(double), typeof(Player));
        internal static readonly DependencyProperty _isMutedProperty = DependencyProperty.RegisterAttached("IsMuted", typeof(bool), typeof(Player));
        internal static readonly DependencyProperty _loadedBehaviorProperty = DependencyProperty.RegisterAttached("LoadedBehaviorProperty", typeof(MediaState), typeof(Player));
        internal static readonly DependencyProperty _unloadedBehaviorProperty = DependencyProperty.RegisterAttached("UnloadedBehaviorProperty", typeof(MediaState), typeof(Player));
        internal static readonly DependencyProperty _position = DependencyProperty.RegisterAttached("Position", typeof(double), typeof(Player));
        internal static readonly DependencyProperty _maximum = DependencyProperty.RegisterAttached("Maximum", typeof(double), typeof(Player));
        #endregion

        #region Properties
        internal Uri Source { get => (Uri)GetValue(_sourceProperty); set => SetValue(_sourceProperty, value); }
        internal double Volume { get => (double)GetValue(_volumeProperty); set => SetValue(_volumeProperty, value); }
        internal double Balance { get => (double)GetValue(_balanceProperty); set => SetValue(_balanceProperty, value); }
        internal double Position { get => (double)GetValue(_position); set => SetValue(_position, value); }
        internal double Maximum { get => (double)GetValue(_maximum); set => SetValue(_maximum, _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds); }
        internal bool IsMuted { get => (bool)GetValue(_isMutedProperty); set => SetValue(_isMutedProperty, value); }
        internal MediaState LoadedBehavior { get => (MediaState)GetValue(_loadedBehaviorProperty); set => SetValue(_loadedBehaviorProperty, value); }
        internal MediaState UnloadedBehavior { get => (MediaState)GetValue(_unloadedBehaviorProperty); set => SetValue(_unloadedBehaviorProperty, value); }
        #endregion

        #region Events
        internal delegate void PlayerReadyHandler();
        internal event PlayerReadyHandler PlayerReady;
        #endregion

        #region Fields
        internal readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        #endregion

        #region Methods
        internal void Play()
        { 
            _mediaPlayer.Play(); 
        }

        internal void Pause()
        { 
            _mediaPlayer.Pause(); 
        }

        internal void HookEvents()
        {
            _mediaPlayer.MediaEnded += PlayNext;
            _mediaPlayer.MediaOpened += GetPlayerReady;
        }

        internal void OpenMedia(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Open(Source);
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
                Stretch = Stretch.Fill,
                Viewbox = new Rect(0, 0, 1, 1),
                Viewport = new Rect(0, 0, 1, 1)
            };
            Background = drawingBrush;

            PlayerReady?.Invoke();
        }

        internal void PlayNext(object sender, EventArgs e)
        {
            SourceController.MoveNext();
            Source = SourceController.GetSource();
        }

        internal void PlayPrevious(object sender, EventArgs e)
        {
            SourceController.MovePrevious();
            Source = SourceController.GetSource();
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
        public SourceController SourceController = new SourceController();
        #endregion
    }
}
