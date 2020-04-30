using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using static USort.App;
using System.Windows.Controls;
using Newtonsoft.Json;
using Clipboard = System.Windows.Clipboard;
using TextBox = System.Windows.Controls.TextBox;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для CategoryEditor.xaml
    /// </summary>
    public partial class CategoryEditor : Window
    {
        public CategoryEditor()
        {
            InitializeComponent();
            if (creating == false)
            {
                if (App.Language.ToString() == "ru-RU")
                {
                    this.Title = "Редактировать";
                }
                else if (App.Language.ToString() == "en-US")
                {
                    this.Title = "Edit";
                }
                Help_Text.SetResourceReference(TextBlock.TextProperty, "l_Help");
                Name_TextBox.Text = CategoryList[indexIn].Name;
                foreach (string str in  CategoryList[indexIn].Formats)
                {
                    Formats_TextBox.Text += $"*{str}";
                }
            }
            else
            {
                if (App.Language.ToString() == "ru-RU")
                {
                    this.Title = "Добавить";
                }
                else if (App.Language.ToString() == "en-US")
                {
                    this.Title = "Add";
                }
                Name_TextBox.SetResourceReference(TextBox.TextProperty, "l_BoxName");
                Formats_TextBox.SetResourceReference(TextBox.TextProperty, "l_BoxFormats");
            }
        }

        private void CategorySave_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (creating == false)
                {
                    var str2 = Formats_TextBox.Text.Split('*');
                    CategoryList[indexIn].Formats.Clear();
                    foreach (string s in str2)
                    {
                        if (s != "")
                        {
                            CategoryList[indexIn].Formats.Add(s);
                        }
                    }
                }
                else
                {
                    CategoryClass newCategory = new CategoryClass(Name_TextBox.Text, new ObservableCollection<string>());
                    var str2 = Formats_TextBox.Text.Split('*');
                    foreach (string s in str2)
                    {
                        if (s != "")
                        {
                            newCategory.Formats.Add(s);
                        }
                    }
                    CategoryList.Add(newCategory);
                }
                JSP.Categories = CategoryList;
                JSP.FileExceptions = FileException;
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, JSP);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (creating == true)
            {
                TextBox tx = (TextBox)sender;
                tx.Text = "";
            }
        }
    }
}
