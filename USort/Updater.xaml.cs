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
        public Updater()
        {
            InitializeComponent();
            using (WebClient wc = new WebClient())
            {
                Uri down_Uri = new Uri("http://net2fox.kl.com.ua/Latest.exe");
                wc.DownloadFileAsync(down_Uri, $"{Environment.CurrentDirectory}/Latest.exe");
                wc.DownloadProgressChanged += (s, e) => { ProgressBar.Maximum = e.TotalBytesToReceive;
                    ProgressBar.Value = e.BytesReceived;
                    TextProgress.Text = e.ProgressPercentage.ToString(); 

                };
                wc.DownloadFileCompleted += (s, e) => { Process.Start($"{Environment.CurrentDirectory}/Latest.exe"); Environment.Exit(0); };
            }
        }
    }
}
