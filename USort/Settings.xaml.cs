using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        

        public Settings()
        {
            InitializeComponent();
            try
            {
                DocFormats_List.ItemsSource = ClassFormats.DocFormats;
                ImageFormats_List.ItemsSource = ClassFormats.ImageFormats;
                PresentFormats_List.ItemsSource = ClassFormats.PresentFormats;
                ArchiveFormats_List.ItemsSource = ClassFormats.ArchiveFormats;
                MusicFormats_List.ItemsSource = ClassFormats.MusicFormats;
                ModelFormats_List.ItemsSource = ClassFormats.ModelFormat;
                ProgramFormats_List.ItemsSource = ClassFormats.ProgramFormats;
                Formats1_Label.Content = "Documents";
                Formats2_Label.Content = "Images";
                Formats3_Label.Content = "Presentations";
                Formats4_Label.Content = "Archives"; 
                Formats5_Label.Content = "Music";
                Formats6_Label.Content = "Programs";
                Formats7_Label.Content = "Models";
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CrutchClass fc = new CrutchClass();
                fc.DocFormats = ClassFormats.DocFormats;
                fc.ImageFormats = ClassFormats.ImageFormats;
                fc.PresentFormats = ClassFormats.PresentFormats;

                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, fc);
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
        }
    }
}
