using System;
using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Controls;
using System.Collections.Generic;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = null;
        private string fullDirectoryFile = null;
        private sbyte direct = 0; //Variable to check if a directory has been specified

        public MainWindow()
        {
            InitializeComponent();
            Version_Label.Content = App.version;
            try
            {
                //Here, a new instance of the class is created to write a static class in json (essentially a crutch)
                CrutchClass fc = new CrutchClass();
                //-----------------------------------------------------------------------------------------------------------
                var SysLang = CultureInfo.CurrentCulture;
                if(SysLang.ToString() == "ru-RU" || SysLang.ToString() == "en-US") //Проверка языка системы
                {
                    App.Language = CultureInfo.CurrentCulture;
                }

                if (File.Exists($@"{Environment.CurrentDirectory}\Settings.json")) 
                {
                    string json = File.ReadAllText($@"{Environment.CurrentDirectory}\Settings.json");
                    fc = JsonConvert.DeserializeObject<CrutchClass>(json);
                    //Assigning values to a non-static class
                    ClassFormats.DocFormats = fc.DocFormats; 
                    ClassFormats.PresentFormats = fc.PresentFormats;
                    ClassFormats.ImageFormats = fc.ImageFormats;
                    ClassFormats.ArchiveFormats = fc.ArchiveFormats;
                    ClassFormats.ModelFormat = fc.ModelFormat;
                    ClassFormats.ProgramFormats = fc.ProgramFormats;
                    ClassFormats.MusicFormats = fc.MusicFormats;
                    ClassFormats.VideoFormats = fc.VideoFormats;
                    DirectoryPath.Text = Properties.Settings.Default.Path;
                    //-----------------------------------------
                }
                else
                {
                    fc.DocFormats = ClassFormats.DocFormats;
                    fc.ImageFormats = ClassFormats.ImageFormats;
                    fc.PresentFormats = ClassFormats.PresentFormats;
                    fc.ArchiveFormats = ClassFormats.ArchiveFormats;
                    fc.ModelFormat = ClassFormats.ModelFormat;
                    fc.ProgramFormats = ClassFormats.ProgramFormats;
                    fc.MusicFormats = ClassFormats.MusicFormats;
                    fc.VideoFormats = ClassFormats.VideoFormats;
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, fc);
                    }
                }
            }
            catch(Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog openFolder = new FolderBrowserDialog();
                if (openFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = DirectoryPath.Text = openFolder.SelectedPath;
                    USort.Properties.Settings.Default.Path = path;
                    USort.Properties.Settings.Default.Save();
                    direct = 1;
                }
                else
                {
                    GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_PathError");

                    //if (App.Language.ToString() == "ru-RU")
                    //{
                    //    GreetingLab.Text = "Кажется, вы не выбрали папку.";
                    //}
                    //else if(App.Language.ToString() == "en-US")
                    //{
                    //    GreetingLab.Text = "It looks like you did not specify a folder.";
                    //}
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Error");
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings settings_window = new Settings();
            settings_window.Owner = this;
            settings_window.ShowDialog();
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (direct == 1)
                {
                    DirectoryInfo files = new DirectoryInfo(path);
                    Progress1.Maximum = files.GetFiles().Length;
                    Progress1.Value = 0;
                    foreach (FileInfo file in files.GetFiles())
                    {
                        try
                        {
                            foreach (string form in ClassFormats.DocFormats)
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Documents\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Documents\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.PresentFormats)//Presentation sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Presentations\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Presentations\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.ImageFormats)//Image sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Images\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Images\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.ArchiveFormats)//Archive sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Archives\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Archives\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.ModelFormat)//3D Models sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Models\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Models\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.ProgramFormats)//Programs/installers sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Programs\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Programs\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.MusicFormats)//Music sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Music\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Music\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                            foreach (string form in ClassFormats.VideoFormats)//Video sort
                            {
                                if (file.Extension == form)
                                {
                                    Directory.CreateDirectory($@"{path}\Video\");
                                    fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}";
                                    File.Move(fullDirectoryFile, $@"{path}\Video\{file.Name}");
                                    Progress1.Value++;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            
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
