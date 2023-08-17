using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FileManager
{
    public class PathToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string extension = Path.GetExtension(value as string);
            if (!string.IsNullOrEmpty(extension))
            {
                switch (extension.ToLower())
                {
                    case ".mp3":
                    case ".wav":
                    case ".wma":
                        return "Images/music.png";
                    case ".xls":
                    case ".xlsx":
                        return "Images/excel.png";
                    case ".ppt":
                    case ".pptx":
                        return "Images/powerpoint.png";
                    case ".pdf":
                        return "Images/pdf.png";
                    case ".jpg":
                    case ".png":
                    case ".bmp":
                        return "Images/picture.png";
                    case ".7z":
                    case ".rar":
                    case ".zip":
                        return "Images/zip.png";
                    case ".doc": 
                    case ".docx": 
                        return "Images/word.png";
                    case ".":
                        return "Images/folder.png";
                    default:
                        return "Images/file.png";
                }
            }
            else
            {
                return "Images/folder.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
