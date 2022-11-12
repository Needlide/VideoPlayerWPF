using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoPlayerWPF
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
        internal static readonly RoutedEvent _mediaFailedEvent;
        internal static readonly RoutedEvent _mediaOpenedEvent;
        internal static readonly RoutedEvent _mediaEndedEvent;
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
        MediaPlayer _mediaPlayer = new MediaPlayer();
        internal List<Uri> _sources = new List<Uri>();
        #endregion

        internal Player()
        {}

        internal void Play() { _mediaPlayer.Play(); }

        internal void Pause() { _mediaPlayer.Pause(); }

        internal void Close() { _mediaPlayer.Close(); }

        private void HookEvents()
        {

        }

        internal void GetPlayerReady(object sender, EventArgs e)
        {
            _mediaPlayer.Open(Source);
            VideoDrawing videoDrawing = new VideoDrawing
            {
                Player = _mediaPlayer
            };

            DrawingBrush drawingBrush = new DrawingBrush
            {
                Drawing = videoDrawing
            };
            Background = drawingBrush;
        }
    }
}
