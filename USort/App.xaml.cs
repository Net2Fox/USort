using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    ///
    public partial class App : Application
    {
        internal static string version = "Beta 0.1";
    }
    public static class ClassFormats
    {
        public static ObservableCollection<string> DocFormats = new ObservableCollection<string> { ".txt", ".doc", ".docx", ".pdf", ".docm", ".dot", ".dotx", ".dotm", ".odt", ".rtf" }; //Все расширения документов по умолчанию
        public static ObservableCollection<string> PresentFormats = new ObservableCollection<string> { ".pptm", ".pptx", ".potx", ".potm", ".ppam", ".ppsx", ".ppsm", ".sldx", ".sldm", ".ppt", ".thmx" }; //Все расширения презентаций по умолчанию
        public static ObservableCollection<string> ImageFormats = new ObservableCollection<string> { ".png", ".jpg", ".jpeg", ".tif", ".tiff", ".gif", ".bmp", ".dib", ".psd", ".webp", ".jpe", ".jfif", ".rle", ".tga" }; //Все расширения картинок по умолчанию
        public static ObservableCollection<string> ArchiveFormats = new ObservableCollection<string> { ".zip", ".7z", ".rar", ".tar", ".tar-gz", ".tgz", ".zipx" }; //Все расширения архивов по умолчанию
        public static ObservableCollection<string> ModelFormat = new ObservableCollection<string> { ".obj", ".max", ".fbx", ".3ds", ".ai", ".dae", ".dwg", ".dxf" }; //Все расширения 3Д моделей по умолчанию
        public static ObservableCollection<string> MusicFormats = new ObservableCollection<string> { ".mp3", ".m4a", ".wav", ".ogg", ".mpa", ".midi", ".mid", ".m3u", ".m3u8", ".flac", ".jpe", ".jfif", ".rle" }; //Все расширения музыки по умолчанию
        public static ObservableCollection<string> ProgramFormats = new ObservableCollection<string> { ".exe", ".msi" }; //Все расширения программ по умолчанию
    }

    public class CrutchClass //Класс для записи в JSON
    {
        public ObservableCollection<string> DocFormats = new ObservableCollection<string> { };
        public ObservableCollection<string> PresentFormats = new ObservableCollection<string> { };
        public ObservableCollection<string> ImageFormats = new ObservableCollection<string> { };
        public ObservableCollection<string> ArchiveFormats = new ObservableCollection<string> { };
        public ObservableCollection<string> ModelFormat = new ObservableCollection<string> { };
        public ObservableCollection<string> MusicFormats = new ObservableCollection<string> { };
        public ObservableCollection<string> ProgramFormats = new ObservableCollection<string> { };
        public double Ver = 0.1; //Версия программы для записи в JSON
    }
}
