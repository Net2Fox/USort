using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Application = System.Windows.Application;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    ///
    public partial class App : Application
    { 
        internal static string version = " Beta 0.4"; //Отображение версии
        internal static int indexIn; //Индекс категории в List
        internal static List<CategoryClass> CategoryList; //List для экземпляров категорий
        internal static bool creating;

                                        //Localization
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
        //----------------------------------------------------------------------------------------
    }

    public class ClassFormats //Нужен для парсинга старых настроек
    {
        public ObservableCollection<string> DocFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> ImageFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> ArchiveFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> ModelFormat = new ObservableCollection<string> {  };
        public ObservableCollection<string> MusicFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> VideoFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> ProgramFormats = new ObservableCollection<string> {  };
        public ObservableCollection<string> PresentFormats = new ObservableCollection<string> {  };
        public CultureInfo Language;
    }

    public class CategoryClass //Шаблон категории
    {
        public string Name { get; set; }
        public ObservableCollection<string> Formats = new ObservableCollection<string> { };

        public CategoryClass(string N, ObservableCollection<string> F)
        {
            Name = N;
            Formats = F;
        }

        [JsonConstructor] //Конструктор для JSON
        public CategoryClass(string N, ObservableCollection<string> F, string DP)
        {
            Name = N;
            Formats = F;
        }
    }

    public class CategoryClass2 //Мусорный класс :(
    {
        public string Name { get; set; }
        public string Formats { get; set; }

        public CategoryClass2(string N, string F)
        {
            Name = N;
            Formats = F;
        }
    }

    public class Updates //Класс для парсинга обновлений
    {
        public double LastetVersion;
    }
}
