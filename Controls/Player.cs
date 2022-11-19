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
        #endregion

        #region Properties
        public Uri Source { get => (Uri)GetValue(_sourceProperty); set => SetValue(_sourceProperty, value); }
        public double Volume { get => (double)GetValue(_volumeProperty); set => SetValue(_volumeProperty, value); }
        public double Balance { get => (double)GetValue(_balanceProperty); set => SetValue(_balanceProperty, value); }
        public bool IsMuted { get => (bool)GetValue(_isMutedProperty); set => SetValue(_isMutedProperty, value); }
        public MediaState LoadedBehavior { get => (MediaState)GetValue(_loadedBehaviorProperty); set => SetValue(_loadedBehaviorProperty, value); }
        public MediaState UnloadedBehavior { get => (MediaState)GetValue(_unloadedBehaviorProperty); set => SetValue(_unloadedBehaviorProperty, value); }
        #endregion

        #region Fields
        readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        internal List<Uri> _sources = new List<Uri>();
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

        internal void Close()
        { 
            _mediaPlayer.Close(); 
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
