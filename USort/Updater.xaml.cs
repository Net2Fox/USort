using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Updater.xaml
    /// </summary>
    public partial class Updater : Window
    {
        string UR;
        public Updater(double LatestVersion, string URL, string Changelog)
        {
            InitializeComponent();
            UR = URL;
            if(App.Language.ToString() == "ru-RU")
            {
                UpdaterText.Text = $"Найдена новая версия USort {LatestVersion.ToString().Replace(",", ".")}.\n{Changelog}\nЖелаете обновится?";
            }
            else if(App.Language.ToString() == "en-US")
            {
                UpdaterText.Text = $"New version of USort {LatestVersion.ToString().Replace(",", ".")} found.\n{Changelog}\nWould you like to upgrade?";
            }
        }

        private void No_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Yes_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(UR);
        }
    }
}
