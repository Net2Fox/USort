using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
//using System.Windows.Forms;
using Newtonsoft.Json;
using System.Windows.Controls;
using Clipboard = System.Windows.Clipboard;

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
                App.LanguageChanged += LanguageChanged;
                CultureInfo currLang = App.Language;
                menuLanguage.Items.Clear();
                foreach (var lang in App.Languages)
                {
                    MenuItem menuLang = new MenuItem();
                    menuLang.Header = lang.DisplayName;
                    menuLang.Tag = lang;
                    menuLang.IsChecked = lang.Equals(currLang);
                    menuLang.Click += ChangeLanguageClick;
                    menuLanguage.Items.Add(menuLang);
                }

                DocFormats_List.ItemsSource = ClassFormats.DocFormats;
                ImageFormats_List.ItemsSource = ClassFormats.ImageFormats;
                PresentFormats_List.ItemsSource = ClassFormats.PresentFormats;
                ArchiveFormats_List.ItemsSource = ClassFormats.ArchiveFormats;
                MusicFormats_List.ItemsSource = ClassFormats.MusicFormats;
                ModelFormats_List.ItemsSource = ClassFormats.ModelFormat;
                ProgramFormats_List.ItemsSource = ClassFormats.ProgramFormats;
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
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
                fc.ArchiveFormats = ClassFormats.ArchiveFormats;
                fc.ModelFormat = ClassFormats.ModelFormat;
                fc.ProgramFormats = ClassFormats.ProgramFormats;
                fc.MusicFormats = ClassFormats.MusicFormats;
                fc.VideoFormats = ClassFormats.VideoFormats;
                fc.Language = App.Language;
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Formatting = Formatting.Indented;
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
