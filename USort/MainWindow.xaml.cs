using System;
using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading;

namespace USort
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = null; //Путь к папке, указанный пользователем
        private string fullDirectoryFile = null; //Полный путь к файлу
        private sbyte direct = 0; //Переменная для проверки, была ли указана дериктория

        public MainWindow()
        {
            InitializeComponent();
            GreetingLab.Text += "\nДля начала укажите путь к папке, файлы в которой вы хотите отсортировать."; //Приветствие
            Version_Label.Content = App.version;
            try
            {
                //Крч тут происходит создание нового экземпляра класса для записи статического класса в json(по сути костыль)
                CrutchClass fc = new CrutchClass();
                //-----------------------------------------------------------------------------------------------------------

                if (File.Exists($@"{Environment.CurrentDirectory}\Settings.json")) //Если файл есть
                {
                    string json = File.ReadAllText($@"{Environment.CurrentDirectory}\Settings.json"); //Чтение всего, что есть в файле
                    fc = JsonConvert.DeserializeObject<CrutchClass>(json); //Парсинг json
                    //Присваивание значений не статическому классу
                    ClassFormats.DocFormats = fc.DocFormats; 
                    ClassFormats.PresentFormats = fc.PresentFormats;
                    ClassFormats.ImageFormats = fc.ImageFormats;
                    ClassFormats.ArchiveFormats = fc.ArchiveFormats;
                    ClassFormats.ModelFormat = fc.ModelFormat;
                    ClassFormats.ProgramFormats = fc.ProgramFormats;
                    ClassFormats.MusicFormats = fc.MusicFormats;
                    //-----------------------------------------
                }
                else //Если файла нет
                {

                    fc.DocFormats = ClassFormats.DocFormats;
                    fc.ImageFormats = ClassFormats.ImageFormats;
                    fc.PresentFormats = ClassFormats.PresentFormats;
                    fc.ArchiveFormats = ClassFormats.ArchiveFormats;
                    fc.ModelFormat = ClassFormats.ModelFormat;
                    fc.ProgramFormats = ClassFormats.ProgramFormats;
                    fc.MusicFormats = ClassFormats.MusicFormats;
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Settings.json"))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, fc); //Создание файла и запись в него
                    }
                }
            }
            catch(Exception ex)
            {
                Clipboard.SetText(ex.ToString()); //Ошибка. Лог копируется в буфер обмена
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Кнопка выбора пути
        {
            try
            {
                FolderBrowserDialog openFolder = new FolderBrowserDialog(); //Новый экземпляр диалогового окна
                if (openFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Проверка результата открытия диалогового окна
                {
                    path = DirectoryPath.Text = openFolder.SelectedPath; //Присваивание пути
                    direct = 1;
                }
                else //Если не указать папку
                {
                    GreetingLab.Text = "Похоже, вы не указали папку. \nЕсли эта ошибка повторится, попробуйте ввести путь вручную.";
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.Text = "Кажется, возникла критическая ошибка. Лог был скопирован в буфер обемна.";
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e) //Кнопка настроек
        {
            Settings settings_window = new Settings();
            settings_window.Owner = this; //Установка родительства
            settings_window.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Кнопка тестирования
        {
            //Это всё нужно для проверки, какие значения находятся в статическом классе
            GreetingLab.Text = "";
            foreach (var v in ClassFormats.DocFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ImageFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.PresentFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ArchiveFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ModelFormat)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.ProgramFormats)
            {
                GreetingLab.Text += v;
            }
            GreetingLab.Text += "\n";
            foreach (var v in ClassFormats.MusicFormats)
            {
                GreetingLab.Text += v;
            }
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e) //Кнопка сортировки
        {
            try
            {
                if (direct == 1)
                {
                    DirectoryInfo files = new DirectoryInfo(path); //Новый экземпляр DirectoryInfo с заданным путём
                    Progress1.Maximum = files.GetFiles().Length;
                    Progress1.Value = 0;
                    foreach (FileInfo file in files.GetFiles()) //Цикл прокрутки через все файлы в папке
                    {
                        foreach (string form in ClassFormats.DocFormats)//Сортировка документов
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Documents\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Documents\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.PresentFormats)//Сортировка презентаций
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Presentations\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Presentations\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.ImageFormats)//Сортировка картинок
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Images\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Images\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.ArchiveFormats)//Сортировка архивов
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Archives\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Archives\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.ModelFormat)//Сортировка 3Д моделей
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Models\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Models\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.ProgramFormats)//Сортировка программ/установщиков
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Programs\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Programs\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                        foreach (string form in ClassFormats.MusicFormats)//Сортировка музыки
                        {
                            if (file.Extension == form)
                            {
                                Directory.CreateDirectory($@"{path}\Music\"); //Создание папки, если её нет
                                fullDirectoryFile = $@"{file.DirectoryName}\{file.Name}"; //Установка полного пути до файла
                                File.Move(fullDirectoryFile, $@"{path}\Music\{file.Name}"); //Перемещение файла
                                Progress1.Value++;
                            }
                        }
                    }
                    GreetingLab.Text = "Успешно!";
                }
                else
                {
                    GreetingLab.Text = "Похоже, вы не указали папку.";
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
                GreetingLab.Text = "Кажется, возникла критическая ошибка. Лог был скопирован в буфер обемна.";
            }
        }

        private void About_Button_Click(object sender, RoutedEventArgs e) //Кнопка о программе
        {
            About about_window = new About();
            about_window.ShowDialog();
        }
    }

    
}
