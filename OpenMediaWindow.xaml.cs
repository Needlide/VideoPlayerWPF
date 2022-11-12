using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        List<Uri> _sources = new List<Uri>();
        #endregion

        #region Events
        readonly EventHandler _filesSelected;
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
            _filesSelected += _player.GetPlayerReady;
        }
        #endregion

        #region Button click
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
                foreach(string file in fileDialog.FileNames)
                {
                    _sources.Add(new Uri(file));
                }
                _player._sources = _sources;
                _filesSelected.Invoke(this, null);
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
