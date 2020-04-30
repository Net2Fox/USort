using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using static USort.Pages;
using System.Configuration;

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
                        updClass = JsonConvert.DeserializeObject<Updates>(wc.DownloadString("http://net2fox.site/download/Update1.json"));
                        if (updClass.LatestVersion != Properties.Settings.Default.Version && updClass.LatestVersion > Properties.Settings.Default.Version)
                        {
                            Updater upWin = new Updater(updClass.LatestVersion, updClass.URL, updClass.Changelogs);
                            upWin.ShowDialog();
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
            mp.Settings_Button.Click += (s, e) => { 
                Main.Navigate(sp); 
                mp.GreetingLab.SetResourceReference(TextBlock.TextProperty, "l_Greetings"); 
            };
            sp.Advance_Button.Click += (s, e) => { Main.Navigate(asp); };
            sp.Back_Button.Click += (s, e) => { Main.Navigate(mp); };
            asp.Back_Button.Click += (s, e) => { Main.Navigate(sp); };
        }
    }
}
