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

namespace FileManager
{
    /// <summary>
    /// Interaction logic for FileDetailsWindow.xaml
    /// </summary>
    public partial class FileDetailsWindow : Window
    {
        public FileDetailsWindow(string path)
        {
            InitializeComponent();

            string extension = System.IO.Path.GetExtension(path as string);

            if (extension == ".jpg" || extension == ".png" || extension == ".bmp")
            {
                imageViewer.Visibility = Visibility.Visible;
                imageViewer.Source = new BitmapImage(new Uri(path));
            }
            else if (extension == ".mp3" || extension == ".wav" || extension == ".wma")
            {
                audioPlayer.Visibility = Visibility.Visible;
                audioPlayer.Source = new Uri(path);
                audioPlayer.Play();
            }
            else if (extension == ".mp4" || extension == ".avi" || extension == ".wmv")
            {
                videoPlayer.Visibility = Visibility.Visible;
                videoPlayer.Source = new Uri(path);
                videoPlayer.Play();
            }
            else if (extension == ".pdf")
            {
                pdfViewer.Visibility = Visibility.Visible;
                pdfViewer.Navigate(new Uri(path));
            }
            else if (extension == ".doc" || extension == ".docx")
            {
                wordViewer.Visibility = Visibility.Visible;
                wordViewer.Navigate(new Uri(path));
            }
            else if (extension == ".xls" || extension == ".xlsx")
            {
                excelViewer.Visibility = Visibility.Visible;
                excelViewer.Navigate(new Uri(path));
            }
            else if (extension == ".ppt" || extension == ".pptx")
            {
                powerPointViewer.Visibility = Visibility.Visible;
                powerPointViewer.Navigate(new Uri(path));
            }
        }
    }
}
