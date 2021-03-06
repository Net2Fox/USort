﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Application = System.Windows.Application;
using Apl = System.Windows.Forms.Application;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    ///
    public partial class App : Application
    { 
        internal static int indexIn; //Индекс категории в List
        internal static List<CategoryClass> CategoryList; //List для экземпляров категорий
        internal static List<string> FileException; //Исключения файлов из сортировки
        internal static string LastPath;
        internal static bool creating;
        internal static string version = "Stable 1.0";
        internal static JSONParser JSP = new JSONParser();


        //Параметры запуска
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                //Тихий режим. Нужен для того, чтобы совершать сортировку без запуска окна программы. 
                if (arg == "sort")
                {
                    string json = File.ReadAllText($@"{Apl.StartupPath}\Settings.json");
                    JSP = JsonConvert.DeserializeObject<JSONParser>(json);
                    CategoryList = JSP.Categories;
                    FileException = JSP.FileExceptions;
                    DirectoryInfo files = new DirectoryInfo(e.Args[1]);
                    foreach (FileInfo file in files.GetFiles())
                    {
                        try
                        {
                            foreach (CategoryClass Category in CategoryList)
                            {
                                if (Category.Formats.Contains(file.Extension) && FileException.Contains(file.Name) == false)
                                {
                                    Directory.CreateDirectory($@"{e.Args[1]}\{Category.Name}\");
                                    string fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{e.Args[1]}\{Category.Name}\{file.Name}");
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            //Это нужно чтобы обходить файлы, которые заняты другим процессом
                        }
                    }
                    this.Shutdown();
                }
                //------------------------------------------------------------------
                else
                {
                    LastPath = arg;
                }
            }
        }
        //------------------------------------------------------------------


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

            Language = CultureInfo.GetCultureInfo("en-US"); //Стандартный язык
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
                        dict.Source = new Uri(String.Format("Languages/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Languages/lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Languages/lang.")
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

        public void App_LanguageChanged(Object sender, EventArgs e)
        {
            
        }
        //----------------------------------------------------------------------------------------
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
    }

  
    public class JSONParser //Класс для создания и чтения JSON
    {
        public List<CategoryClass> Categories;
        public List<string> FileExceptions;
        public CultureInfo Lang = CultureInfo.GetCultureInfo("en-US");
        public string LastPath = "";
    }

    public class Updates //Класс для парсинга обновлений
    {
        public double LatestVersion;
        public string URL;
        public string Changelogs;
    }
}
