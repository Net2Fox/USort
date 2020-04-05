using System;
using System.Globalization;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using static USort.Pages;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Auto Update
            if (Properties.Settings.Default.AutoUpd == true)
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
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Clipboard.SetText(e.ToString());
                }
            }
            //-----------------------------------------------------------------------------------------------------------

            mp = new MainPage();
            sp = new Settings_Page();
            asp = new Advance_Settings_Page();

            Main.Navigate(mp);
            mp.Settings_Button.Click += (s, e) => { Main.Navigate(sp); };
            sp.Advance_Button.Click += (s, e) => { Main.Navigate(asp); };
            sp.Back_Button.Click += (s, e) => { Main.Navigate(mp); };
            asp.Back_Button.Click += (s, e) => { Main.Navigate(sp); };
        }
    }
}
