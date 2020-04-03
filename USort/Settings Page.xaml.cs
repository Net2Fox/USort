using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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
    }
}
