using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        public Uri MediaUri { get; set; }
        private Player _player;
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
        }
        #endregion

        #region Button click
        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            _player.PlayVideoFromFile();
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
                    _player.PlayVideoFromUri(MediaUri);
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
