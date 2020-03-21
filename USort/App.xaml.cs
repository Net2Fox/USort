using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    ///
    public partial class App : Application
    {
        internal static string version = " Beta 0.2";

        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public App()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;

            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("ru-RU"));

            Language = USort.Properties.Settings.Default.DefaultLanguage;
        }

        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            USort.Properties.Settings.Default.DefaultLanguage = Language;
            USort.Properties.Settings.Default.Save();
        }
    }
    public static class ClassFormats
    {
        public static ObservableCollection<string> DocFormats = new ObservableCollection<string> { ".txt", ".doc", ".docx", ".pdf", ".docm", ".dot", ".dotx", ".dotm", ".odt", ".rtf" }; //Все расширения документов по умолчанию
        public static ObservableCollection<string> PresentFormats = new ObservableCollection<string> { ".pptm", ".pptx", ".potx", ".potm", ".ppam", ".ppsx", ".ppsm", ".sldx", ".sldm", ".ppt", ".thmx" }; //Все расширения презентаций по умолчанию
        public static ObservableCollection<string> ImageFormats = new ObservableCollection<string> { ".png", ".jpg", ".jpeg", ".tif", ".tiff", ".gif", ".bmp", ".dib", ".psd", ".webp", ".jpe", ".jfif", ".rle", ".tga" }; //Все расширения картинок по умолчанию
        public static ObservableCollection<string> ArchiveFormats = new ObservableCollection<string> { ".zip", ".7z", ".rar", ".tar", ".tar-gz", ".tgz", ".zipx" }; //Все расширения архивов по умолчанию
        public static ObservableCollection<string> ModelFormat = new ObservableCollection<string> { ".obj", ".max", ".fbx", ".3ds", ".ai", ".dae", ".dwg", ".dxf", ".dff", ".mtl", ".txd"}; //Все расширения 3Д моделей по умолчанию
        public static ObservableCollection<string> MusicFormats = new ObservableCollection<string> { ".mp3", ".m4a", ".wav", ".ogg", ".mpa", ".midi", ".mid", ".m3u", ".m3u8", ".flac", ".jpe", ".jfif", ".rle" }; //Все расширения музыки по умолчанию
        public static ObservableCollection<string> VideoFormats = new ObservableCollection<string> { ".mp4", ".mkv" }; //Все расширения видео по умолчанию
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
        public ObservableCollection<string> VideoFormats = new ObservableCollection<string> { };
    }
}
