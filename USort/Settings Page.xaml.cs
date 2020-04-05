using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using static USort.App;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Settings_Page.xaml
    /// </summary>
    public partial class Settings_Page : Page
    {
        public Settings_Page()
        {
            InitializeComponent();
            AU_CheckBox.IsChecked = Convert.ToBoolean(Properties.Settings.Default.AutoUpd);
            FileExcep_ListView.ItemsSource = FileException;
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
                    Properties.Settings.Default.DefaultLanguage = App.Language;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private async void Upd_Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        Updates updClass = new Updates();
                        updClass = JsonConvert.DeserializeObject<Updates>(wc.DownloadString("http://net2fox.site/download/Update.json"));
                        if (updClass.LastetVersion != Properties.Settings.Default.Version && updClass.LastetVersion > Properties.Settings.Default.Version)
                        {
                            MessageBoxResult result = MessageBoxResult.None;
                            if (App.Language.ToString() == "ru-RU")
                            {
                                result = MessageBox.Show($"Вышла новая версия! Желаете её скачать?", "Обновление", MessageBoxButton.YesNo);
                            }
                            else if (App.Language.ToString() == "en-US")
                            {
                                result = MessageBox.Show($"New version released! Would you like to download it?", "Update", MessageBoxButton.YesNo);
                            }

                            if (result == MessageBoxResult.Yes)
                            {
                                Updater updWin = new Updater();
                                updWin.Show();
                            }
                        }
                        else
                        {
                            if (App.Language.ToString() == "ru-RU")
                            {
                                MessageBox.Show($"У вас самая последняя версия!", "Обновление", MessageBoxButton.OK);
                            }
                            else if (App.Language.ToString() == "en-US")
                            { 
                                MessageBox.Show($"You have the latest version!", "Update", MessageBoxButton.OK);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Clipboard.SetText(ex.ToString());
                }
            });
        }

        private void AU_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoUpd = Convert.ToBoolean(AU_CheckBox.IsChecked);
            Properties.Settings.Default.Save();
        }

        private void AU_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoUpd = Convert.ToBoolean(AU_CheckBox.IsChecked);
            Properties.Settings.Default.Save();
        }

        private void Plus_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (FileException.Contains(FileExc_TextBox.Text) == false)
                {
                    FileException.Add(FileExc_TextBox.Text);
                }
                FileExcep_ListView.ItemsSource = null;
                FileExcep_ListView.ItemsSource = FileException;
                Pages.mp.JSP.Categories = CategoryList;
                Pages.mp.JSP.FileExceptions = FileException; 
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{System.Windows.Forms.Application.StartupPath}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, Pages.mp.JSP);
                }
            }
            catch
            {
                
            }
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FileExcep_ListView.SelectedItem == null)
                {
                    FileException.RemoveAt(FileException.IndexOf(FileException.Last()));
                }
                else
                {
                    FileException.RemoveAt(FileException.IndexOf(FileExcep_ListView.SelectedItem.ToString()));
                }
                FileExcep_ListView.ItemsSource = null;
                FileExcep_ListView.ItemsSource = FileException;
                Pages.mp.JSP.Categories = CategoryList;
                Pages.mp.JSP.FileExceptions = FileException; 
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{System.Windows.Forms.Application.StartupPath}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, Pages.mp.JSP);
                }
            }
            catch
            {
               
            }
        }
    }
}
