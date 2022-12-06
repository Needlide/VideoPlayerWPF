using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoPlayerWPF.Controls
{
    internal class Player : Control
    {
        #region Dependency properties
        internal static readonly DependencyProperty _sourceProperty;
        internal static readonly DependencyProperty _isPlayingProperty;
        internal static readonly DependencyProperty _autoplayProperty;
        internal static readonly DependencyProperty _stretchProperty;
        internal static readonly DependencyProperty _stretchDirectionProperty;
        internal static readonly DependencyProperty _volumeProperty;
        internal static readonly DependencyProperty _balanceProperty;
        #endregion

        #region Properties
        internal Uri Source { get => (Uri)GetValue(_sourceProperty); set => SetValue(_sourceProperty, value); }
        internal bool IsPlaying { get => (bool)GetValue(_isPlayingProperty); set => SetValue(_isPlayingProperty, value); }
        internal bool Autoplay { get => (bool)GetValue(_autoplayProperty); set => SetValue(_autoplayProperty, value); }
        internal Stretch Stretch { get => (Stretch)GetValue(_stretchProperty); set => SetValue(_stretchProperty, value); }
        internal StretchDirection StretchDirection { get => (StretchDirection)GetValue(_stretchDirectionProperty); set => SetValue(_stretchDirectionProperty, value); }
        internal double Volume { get => (double)GetValue(_volumeProperty); set => SetValue(_volumeProperty, value); }
        internal double Balance { get => (double)GetValue(_balanceProperty); set => SetValue(_balanceProperty, value); }
        #endregion

        #region Constructor
        static Player()
        {
            _sourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(Player), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            _isPlayingProperty = DependencyProperty.Register("IsPlaying", typeof(bool), typeof(Player), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
            _autoplayProperty = DependencyProperty.Register("Autoplay", typeof(bool), typeof(Player), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
            _stretchProperty = Viewbox.StretchProperty.AddOwner(typeof(Player));
            _stretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(Player));
            _volumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(Player), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.None));
            _balanceProperty = DependencyProperty.Register("Balance", typeof(double), typeof(Player), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
            _stretchProperty.OverrideMetadata(typeof(Player), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
            _stretchDirectionProperty.OverrideMetadata(typeof(Player), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
        }
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

        internal void Stop()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Close();
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
                if(Autoplay)
                    _mediaPlayer.Play();
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
            _playNextEvent?.Invoke(this, null);
        }

        internal void PlayPrevious()
        {
            SourceController.MovePrevious();
            Source = SourceController.GetSource();
            _playPreviousEvent?.Invoke(this, null);
        }

        internal Duration GetDuration()
        {
            return _mediaPlayer.NaturalDuration;
        }

        internal double GetPosition()
        {
            return _mediaPlayer.Position.TotalSeconds;
        }

        internal void SetSpeedRatio(double speedRatio)
        {
            _mediaPlayer.SpeedRatio = speedRatio;
        }

        internal void SetBalance(double balance)
        {
            if (balance > 1)
                balance = 1;
            if(balance < 0)
                balance = 0;
            _mediaPlayer.Balance = balance;
        }

        internal void SetIsMuted(bool? isMuted)
        {
            _mediaPlayer.IsMuted = (bool)isMuted;
        }

        private Size MeasureArrange(Size inputSize)
        {
            if (_mediaPlayer == null)
                return default;

            Size contentSize = new Size(_mediaPlayer.NaturalVideoWidth, _mediaPlayer.NaturalVideoHeight);
            Size size = ComputeScaleFactor(inputSize, contentSize, Stretch, StretchDirection);
            return new Size(contentSize.Width * size.Width, contentSize.Height * size.Height);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return MeasureArrange(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return MeasureArrange(constraint);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //drawingContext.DrawVideo(_mediaPlayer, new Rect(default, RenderSize));
            drawingContext.DrawVideo(_mediaPlayer, new Rect(default, RenderSize));
        }

        Size ComputeScaleFactor(Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
        {
            double num = 1.0;
            double num2 = 1.0;
            bool flag = !double.IsPositiveInfinity(availableSize.Width);
            bool flag2 = !double.IsPositiveInfinity(availableSize.Height);
            if ((stretch == Stretch.Uniform || stretch == Stretch.UniformToFill || stretch == Stretch.Fill) && (flag || flag2))
            {
                num = (contentSize.Width == 0.0) ? 0.0 : (availableSize.Width / contentSize.Width);
                num2 = (contentSize.Height == 0.0) ? 0.0 : (availableSize.Height / contentSize.Height);
                if (!flag)
                {
                    num = num2;
                }
                else if (!flag2)
                {
                    num2 = num;
                }
                else
                {
                    switch (stretch)
                    {
                        case Stretch.Uniform:
                            {
                                double num4 = (num < num2) ? num : num2;
                                num = num2 = num4;
                                break;
                            }
                        case Stretch.UniformToFill:
                            {
                                double num3 = (num > num2) ? num : num2;
                                num = num2 = num3;
                                break;
                            }
                    }
                }

                switch (stretchDirection)
                {
                    case StretchDirection.UpOnly:
                        if (num < 1.0)
                        {
                            num = 1.0;
                        }

                        if (num2 < 1.0)
                        {
                            num2 = 1.0;
                        }

                        break;
                    case StretchDirection.DownOnly:
                        if (num > 1.0)
                        {
                            num = 1.0;
                        }

                        if (num2 > 1.0)
                        {
                            num2 = 1.0;
                        }

                        break;
                }
            }

            return new Size(num, num2);
        }
        #endregion

        #region SourceController
        internal SourceController SourceController = new SourceController();
        #endregion
    }
}
