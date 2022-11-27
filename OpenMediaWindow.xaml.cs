using System;
using System.Windows;
using System.Windows.Forms;
using VideoPlayerWPF.Controls;

namespace VideoPlayerWPF
{
    /// <summary>
    /// Interaction logic for OpenMediaWindow.xaml
    /// </summary>
    public partial class OpenMediaWindow : Window
    {
        #region Fields
        Uri MediaUri { get; set; }
        Player _player;
        #endregion

        #region Events
        readonly EventHandler<RoutedEventArgs> _filesSelected;
        internal delegate void FilesSelectedHandler();
        internal static FilesSelectedHandler FilesSelectedEvent;
        #endregion

        #region Constructors
        public OpenMediaWindow()
        {
            InitializeComponent();
        }

        internal OpenMediaWindow(Player player)
        {
            InitializeComponent();
            _player = player;
            _filesSelected += _player.OpenMedia;
        }
        #endregion

        #region Buttons clicks
        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select videos for playback",
                Multiselect = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos),
                CheckFileExists = true,
                Filter = "Video Files|*.mp4;*.mpeg|All Files (*.*)|*.*",
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "mp4"
            };

            if(fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(string path in fileDialog.FileNames)
                {
                    _player.SourceController.Sources.Add(new Uri(path));
                }
                _player.Source = _player.SourceController.GetSource();
                _filesSelected?.Invoke(this, null);
                FilesSelectedEvent?.Invoke();
            }
            Close();
        }

        private void OkResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlBox.Text))
            {
                try
                {
                    MediaUri = new Uri(UrlBox.Text);
                    Close();
                    _player.Source = MediaUri;
                    _filesSelected?.Invoke(this, null);
                }
                catch (UriFormatException)
                {
                    System.Windows.MessageBox.Show("Enter a valid URL!", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CancelResultButton_Click(object sender, RoutedEventArgs e) => Close();
        #endregion
    }
}
