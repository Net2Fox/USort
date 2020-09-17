using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using static USort.App;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using Clipboard = System.Windows.Clipboard;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        internal string path = null;
        private string fullDirectoryFile = null;

        public MainPage()
        {
            InitializeComponent();
            Version_Label.Content = version;
            
            try
            {
                //JSON Parsing or creating
                if (File.Exists($@"{Application.StartupPath}\Settings.json"))
                {
                    string json = File.ReadAllText($@"{Application.StartupPath}\Settings.json");
                    JSP = JsonConvert.DeserializeObject<JSONParser>(json);
                    CategoryList = JSP.Categories;
                    FileException = JSP.FileExceptions;
                    DirectoryPath.Text = path = Properties.Settings.Default.Path;
                }
                else
                {
                    JsonSerializer serializer = new JsonSerializer();


                    var SysLang = CultureInfo.CurrentCulture;
                    if (SysLang.ToString() == "ru-RU" || SysLang.ToString() == "en-US") //Проверка языка системы
                    {
                        App.Language = CultureInfo.CurrentCulture;
                        Properties.Settings.Default.DefaultLanguage = App.Language;
                        Properties.Settings.Default.Save();
                    }
                    if (SysLang.ToString() == "ru-RU")
                    {
                        CategoryList = new List<CategoryClass>
                        {
                            new CategoryClass("Документы", new ObservableCollection<string>() { ".txt", ".doc", ".docx", ".pdf", ".docm", ".dot", ".dotx", ".dotm", ".odt", ".rtf" }),
                            new CategoryClass("Картинки", new ObservableCollection<string>() { ".png", ".jpg", ".jpeg", ".tif", ".tiff", ".gif", ".bmp", ".dib", ".psd", ".webp", ".jpe", ".jfif", ".rle", ".tga"  }), 
                            new CategoryClass("Видео", new ObservableCollection<string>() { ".mp4", ".mkv" }), 
                            new CategoryClass("Программы", new ObservableCollection<string>() { ".exe", ".msi" }), 
                            new CategoryClass("Перезентации", new ObservableCollection<string>() { ".pptm", ".pptx", ".potx", ".potm", ".ppam", ".ppsx", ".ppsm", ".sldx", ".sldm", ".ppt", ".thmx" }),
                            new CategoryClass("Архивы", new ObservableCollection<string>() { ".zip", ".7z", ".rar", ".tar", ".tar-gz", ".tgz", ".zipx" }),
                            new CategoryClass("Модели", new ObservableCollection<string>() { ".obj", ".max", ".fbx", ".3ds", ".ai", ".dae", ".dwg", ".dxf", ".dff", ".mtl", ".txd" }),
                            new CategoryClass("Музыка", new ObservableCollection<string>() { ".mp3", ".m4a", ".wav", ".ogg", ".mpa", ".midi", ".mid", ".m3u", ".m3u8", ".flac" })
                        };
                    }
                    else if (SysLang.ToString() == "en-US")
                    {
                        CategoryList = new List<CategoryClass>
                        {
                            new CategoryClass("Documents", new ObservableCollection<string>() { ".txt", ".doc", ".docx", ".pdf", ".docm", ".dot", ".dotx", ".dotm", ".odt", ".rtf" }),
                            new CategoryClass("Images", new ObservableCollection<string>() { ".png", ".jpg", ".jpeg", ".tif", ".tiff", ".gif", ".bmp", ".dib", ".psd", ".webp", ".jpe", ".jfif", ".rle", ".tga"  }), 
                            new CategoryClass("Video", new ObservableCollection<string>() { ".mp4", ".mkv" }), 
                            new CategoryClass("Programs", new ObservableCollection<string>() { ".exe", ".msi" }), 
                            new CategoryClass("Presentations", new ObservableCollection<string>() { ".pptm", ".pptx", ".potx", ".potm", ".ppam", ".ppsx", ".ppsm", ".sldx", ".sldm", ".ppt", ".thmx" }),
                            new CategoryClass("Archives", new ObservableCollection<string>() { ".zip", ".7z", ".rar", ".tar", ".tar-gz", ".tgz", ".zipx" }),
                            new CategoryClass("Models", new ObservableCollection<string>() { ".obj", ".max", ".fbx", ".3ds", ".ai", ".dae", ".dwg", ".dxf", ".dff", ".mtl", ".txd" }),
                            new CategoryClass("Music", new ObservableCollection<string>() { ".mp3", ".m4a", ".wav", ".ogg", ".mpa", ".midi", ".mid", ".m3u", ".m3u8", ".flac" })
                        };
                    }
                    JSP.Categories = CategoryList;
                    FileException = new List<string> { };
                    JSP.FileExceptions = FileException;
                    using (StreamWriter sw = new StreamWriter($@"{Application.StartupPath}\Settings.json"))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, JSP);
                    }
                }
                //-----------------------------------------------------------------------------------------------------------
            }
            catch(Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                if(App.Language.ToString() == "ru-RU")
                {
                    GreetingLab.Text = "Видимо, ваш файл настроек устарел. К сожалению, вам придётся удалить его и настроить всё заново.";
                }
                else if (App.Language.ToString() == "en-US")
                {
                    GreetingLab.Text = "Apparently your settings file is out of date. Unfortunately, you have to remove it and configure everything again.";
                }
            }
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Greetings");
                FolderBrowserDialog openFolder = new FolderBrowserDialog();
                var result = openFolder.ShowDialog();
                if (result == DialogResult.OK)
                {
                    path = DirectoryPath.Text = openFolder.SelectedPath;
                    Properties.Settings.Default.Path = path;
                    Properties.Settings.Default.Save();
                }
                else if(result == DialogResult.Cancel && DirectoryPath.Text != Properties.Settings.Default.Path)
                {
                    GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_PathError");
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Error");
            }
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (path != null)
                {
                    DirectoryInfo files = new DirectoryInfo(path);
                    foreach (FileInfo file in files.GetFiles())
                    {
                        try
                        {
                            foreach (CategoryClass Category in CategoryList)
                            {
                                if(Category.Formats.Contains(file.Extension) && FileException.Contains(file.Name) == false)
                                {
                                    Directory.CreateDirectory($@"{path}\{Category.Name}\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\{Category.Name}\{file.Name}");
                                }
                            }
                        }
                        catch
                        {
                            //Это нужно чтобы обходить файлы, которые заняты другим процессом
                        }
                    }
                    GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Succ");
                }
                else
                {
                    GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_PathError");
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Error");
            }
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            About about_window = new About();
            about_window.ShowDialog();
        }
    }
}
