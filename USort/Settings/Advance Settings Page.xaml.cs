using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
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
            string name = null;
            string formats = null;
            List<ClassForList> List = new List<ClassForList> { };
            foreach (CategoryClass category in CategoryList)
            {
                foreach (string format in category.Formats)
                {
                    formats += $" {format};";
                }
                name = category.Name;
                ClassForList Cate = new ClassForList(name, formats);
                List.Add(Cate);
                name = null;
                formats = null;
            }
            CateList.ItemsSource = List;
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            creating = true;
            CategoryEditor ce_win = new CategoryEditor();
            ce_win.Closing += (s, e) => 
            {
                string name = null;
                string formats = null;
                List<ClassForList> List = new List<ClassForList> { };
                foreach (CategoryClass category in CategoryList)
                {
                    foreach (string format in category.Formats)
                    {
                        formats += $"{format}; ";
                    }
                    name = category.Name;
                    ClassForList Cate = new ClassForList(name, formats);
                    List.Add(Cate);
                    name = null;
                    formats = null;
                }
                CateList.ItemsSource = List;
            };
            ce_win.ShowDialog();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void Delete()
        {
            
            if (CateList.SelectedItem != null)
            {
                ClassForList SelectedCate = (ClassForList)CateList.SelectedItem;
                foreach (CategoryClass Cate in CategoryList)
                {
                    if (Cate.Name == SelectedCate.Name)
                    {
                        indexIn = CategoryList.IndexOf(Cate);
                    }
                }
                CategoryList.RemoveAt(indexIn);
                string name = null;
                string formats = null;
                List<ClassForList> List = new List<ClassForList> { };
                foreach (CategoryClass category in CategoryList)
                {
                    foreach (string format in category.Formats)
                    {
                        formats += $" {format};";
                    }
                    name = category.Name;
                    ClassForList Cate = new ClassForList(name, formats);
                    List.Add(Cate);
                    name = null;
                    formats = null;
                }
                CateList.ItemsSource = List;
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            if(CateList.SelectedItem != null)
            {
                creating = false;
                ClassForList SelectedCate = (ClassForList)CateList.SelectedItem;
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
                    string name = null;
                    string formats = null;
                    List<ClassForList> List = new List<ClassForList> { };
                    foreach (CategoryClass category in CategoryList)
                    {
                        foreach (string format in category.Formats)
                        {
                            name = category.Name;
                            formats += $" {format};";
                        }
                        ClassForList Cate = new ClassForList(name, formats);
                        List.Add(Cate);
                        formats = "";
                        name = "";
                    }
                    CateList.ItemsSource = List;
                };
                ce_win.ShowDialog();
            }
        }

        private void Keyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Edit();
            }
            else if(e.Key == Key.Delete)
            {
                Delete();
            }
        }
    }

    public class ClassForList //Класс для создания списка в настройках
    {
        public string Name { get; set; }
        public string Formats { get; set; }

        public ClassForList(string N, string F)
        {
            Name = N;
            Formats = F;
        }
    }
}
