using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Controls = System.Windows.Controls;
using Input = System.Windows.Input;
using static USort.Pages;
using static USort.App;
using System.Windows.Forms;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Settings_Page.xaml
    /// </summary>
    public partial class Settings_Page : Controls.Page
    {
        public Settings_Page()
        {
            InitializeComponent();
            //AU_CheckBox.IsChecked = Properties.Settings.Default.AutoUpd;
            FileExcep_ListView.ItemsSource = FileException;
            App.LanguageChanged += LanguageChanged;
            CultureInfo currLang = App.Language;
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                Controls.MenuItem menuLang = new Controls.MenuItem();
                if(Convert.ToString(lang) == "ru-RU")
                {
                    menuLang.Header = "Русский";
                }
                else if(Convert.ToString(lang) == "en-US")
                {
                    menuLang.Header = "English";
                }
                //menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;
            foreach (Controls.MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            Controls.MenuItem mi = sender as Controls.MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }
        }

        private void Upd_Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //using (WebClient wc = new WebClient())
                //{
                //    Updates updClass = new Updates();
                //    updClass = JsonConvert.DeserializeObject<Updates>(wc.DownloadString("http://net2fox.site/download/Update1.json"));
                //    if (updClass.LatestVersion != Properties.Settings.Default.Version && updClass.LatestVersion > Properties.Settings.Default.Version)
                //    {
                //        Text1.Text = "GDE OBNOVA SUKA";
                //        Updater upWin = new Updater(updClass.LatestVersion ,updClass.URL, updClass.Changelogs);
                //        upWin.ShowDialog();
                //    }
                //    else
                //    {
                //        if (App.Language.ToString() == "ru-RU")
                //        {
                //            MessageBox.Show($"У вас самая последняя версия!", "Обновление", MessageBoxButton.OK);
                //        }
                //        else if (App.Language.ToString() == "en-US")
                //        { 
                //            MessageBox.Show($"You have the latest version!", "Update", MessageBoxButton.OK);
                //        }
                //    }
                //}
            //}
            //catch (Exception ex)
            //{
                //Clipboard.SetText(ex.ToString());
            //}
        }

        private void AU_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Properties.Settings.Default.AutoUpd = Convert.ToBoolean(AU_CheckBox.IsChecked);
            //Properties.Settings.Default.Save();
        }

        private void AU_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Properties.Settings.Default.AutoUpd = Convert.ToBoolean(AU_CheckBox.IsChecked);
            //Properties.Settings.Default.Save();
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            if (FileExcep_ListView.SelectedItem == null && FileException.Count != 0)
            {
                FileException.RemoveAt(FileException.IndexOf(FileException.Last()));
            }
            else if(FileException.Count > 0)
            {
                FileException.RemoveAt(FileException.IndexOf(FileExcep_ListView.SelectedItem.ToString()));
            }
            FileExcep_ListView.ItemsSource = null;
            FileExcep_ListView.ItemsSource = FileException;
        }

        private void Planned_Button_Click(object sender, RoutedEventArgs e)
        {
            PlannedWin pw = new PlannedWin();
            pw.ShowDialog();
        }

        private void Select_Button_Click(object sender, RoutedEventArgs e)
        {
            using(OpenFileDialog selectFile = new OpenFileDialog())
            {
                selectFile.Multiselect = true;
                if (selectFile.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in selectFile.SafeFileNames)
                    {
                        if (FileException.Contains(file) == false)
                        {
                            FileException.Add(file);
                        }
                    }
                    FileExcep_ListView.ItemsSource = null;
                    FileExcep_ListView.ItemsSource = FileException;
                }
            }
        }

        private void Keyboard(object sender, Input.KeyEventArgs e)
        {
            if (e.Key == Input.Key.Delete && FileExcep_ListView.SelectedItem != null)
            {
                FileException.RemoveAt(FileException.IndexOf(FileExcep_ListView.SelectedItem.ToString()));
                FileExcep_ListView.ItemsSource = null;
                FileExcep_ListView.ItemsSource = FileException;
            }
        }
    }
}
