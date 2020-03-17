using System;
using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading;

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
            GreetingLab.Text += "\nFirst, specify the path to the folder in which files you want to sort.";
            Version_Label.Content = App.version;
            try
            {
                //Here, a new instance of the class is created to write a static class in json (essentially a crutch)
                CrutchClass fc = new CrutchClass();
                //-----------------------------------------------------------------------------------------------------------

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
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
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
                    direct = 1;
                }
                else
                {
                    GreetingLab.Text = "It looks like you did not specify a folder. \nIf this error persists, try entering the path manually.";
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.Text = "There seems to be a critical error. The log was copied to the clipboard.";
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings settings_window = new Settings();
            settings_window.Owner = this;
            settings_window.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //This is all you need to check which values are in the static class
            GreetingLab.Text = "";
            foreach (var v in ClassFormats.DocFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ImageFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.PresentFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ArchiveFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ModelFormat)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ProgramFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.MusicFormats)
            {
                GreetingLab.Text += v;
            }
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
                    }
                    GreetingLab.Text = "Successfully!";
                }
                else
                {
                    GreetingLab.Text = "It looks like you did not specify a folder.";
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.Text = "There seems to be a critical error. The log was copied to the clipboard.";
            }
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            About about_window = new About();
            about_window.ShowDialog();
        }
    }

    
}
