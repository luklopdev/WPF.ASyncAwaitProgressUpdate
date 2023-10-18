using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.ASyncAwaitProgressUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object progressLockObj = new object();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>((value) =>
            {
                progressBar.Value = value;
                txtCaption.Text = $"Running... {value}%";
            });

            await Task.Run(() => LoopThroughNumbers(progress));

            txtCaption.Text = "Completed";
        }

        private void LoopThroughNumbers(IProgress<int> progress)
        {
            lock(progressLockObj)
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    var percentComplete = (i * 100) / 100;
                    progress.Report(percentComplete);
                }
            }
        }
    }
}
