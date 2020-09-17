using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static USort.App;
using Application = System.Windows.Forms.Application;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для Advance_Settings.xaml
    /// </summary>
    public partial class Advance_Settings_Page : Page
    {
        public Advance_Settings_Page()
        {
            InitializeComponent();
            string name = "";
            string form = "";
            List<CategoryClass2> Cate2 = new List<CategoryClass2> { };
            foreach (CategoryClass category in CategoryList)
            {
                foreach (string formats in category.Formats)
                {
                    name = category.Name;
                    form += $" {formats};";
                }
                CategoryClass2 te = new CategoryClass2(name, form);
                Cate2.Add(te);
                form = ""; 
                name = "";
            }
            ListTest.ItemsSource = Cate2;
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            creating = true;
            CategoryEditor ce_win = new CategoryEditor();
            ce_win.Closing += (s, e) => 
            { 
                string name = "";
                string form = "";
                List<CategoryClass2> Cate2 = new List<CategoryClass2> { };
                foreach (CategoryClass category in CategoryList)
                {
                    foreach (string formats in category.Formats)
                    {
                        name = category.Name;
                        form += $" {formats};";
                    }
                    CategoryClass2 te = new CategoryClass2(name, form);
                    Cate2.Add(te);
                    form = ""; 
                    name = "";
                }
                ListTest.ItemsSource = Cate2;
            };
            ce_win.ShowDialog();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CategoryClass2 SelectedCate = (CategoryClass2)ListTest.SelectedItem;
                foreach (CategoryClass Cate in CategoryList)
                {
                    if (Cate.Name == SelectedCate.Name)
                    {
                        indexIn = CategoryList.IndexOf(Cate);
                    }
                }
                CategoryList.RemoveAt(indexIn);
                string name = "";
                string form = "";
                List<CategoryClass2> Cate2 = new List<CategoryClass2> { };
                foreach (CategoryClass category in CategoryList)
                {
                    foreach (string formats in category.Formats)
                    {
                        name = category.Name;
                        form += $" {formats};";
                    }
                    CategoryClass2 te = new CategoryClass2(name, form);
                    Cate2.Add(te);
                    form = ""; 
                    name = "";
                }
                ListTest.ItemsSource = Cate2;
                JsonSerializer serializer = new JsonSerializer();
                JSP.Categories = CategoryList;
                JSP.FileExceptions = FileException;
                using (StreamWriter sw = new StreamWriter($@"{Application.StartupPath}\Settings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, JSP);
                }
            }
            catch
            {

            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                creating = false;
                CategoryClass2 SelectedCate = (CategoryClass2)ListTest.SelectedItem;
                foreach (CategoryClass Cate in CategoryList)
                {
                    if (Cate.Name == SelectedCate.Name)
                    {
                        indexIn = CategoryList.IndexOf(Cate);
                    }
                }
                CategoryEditor ce_win = new CategoryEditor();
                ce_win.Closing += (s, e) => 
                { 
                    string name = "";
                    string form = "";
                    List<CategoryClass2> Cate2 = new List<CategoryClass2> { };
                    foreach (CategoryClass category in CategoryList)
                    {
                        foreach (string formats in category.Formats)
                        {
                            name = category.Name;
                            form += $" {formats};";
                        }
                        CategoryClass2 te = new CategoryClass2(name, form);
                        Cate2.Add(te);
                        form = ""; 
                        name = "";
                    }
                    ListTest.ItemsSource = Cate2;
                };
                ce_win.ShowDialog();
            }
            catch
            {
            }
        }
    }
}
