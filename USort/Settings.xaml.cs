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
using System.Windows.Media.Animation;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private bool advancne = false; //Если ложь - доп настройки не открыты, правда - доп настройки открыты
        Settings_Page sp = new Settings_Page();
        Advance_Settings_Page asp = new Advance_Settings_Page();

        public Settings()
        {
            InitializeComponent();
            Settings_Frame.Navigate(sp);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (advancne == false)
            {
                ADV.SetResourceReference(Button.ContentProperty, "l_AdvButton2");
                Settings_Frame.Navigate(asp);
                advancne = true;
            }
            else if (advancne == true)
            {
                ADV.SetResourceReference(Button.ContentProperty, "l_AdvButton");
                Settings_Frame.Navigate(sp);
                advancne = false;
            }
        }
    }
}
