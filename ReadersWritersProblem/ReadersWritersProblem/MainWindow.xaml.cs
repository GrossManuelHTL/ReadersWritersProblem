using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReadersWritersProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int sharedData = 0;
        private int readersCount = 0;
        private readonly object lockObject = new object();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Writer()
        {
            for (int i = 0; i < 5; i++)
            {
                lock (lockObject)
                {
                    AppendLog($"Writer is writing. Shared Data before write: {sharedData}");
                    sharedData++;
                    AppendLog($"Writer finished writing. Shared Data after write: {sharedData}");
                }

                Thread.Sleep(1000);
            }
        }

        private void Reader()
        {
            for (int i = 0; i < 5; i++)
            {
                lock (lockObject)
                {
                    readersCount++;
                    if (readersCount == 1)
                    {
                        AppendLog($"First reader is reading. Shared Data: {sharedData}");
                    }
                }
                
                Thread.Sleep(50);

                lock (lockObject)
                {
                    readersCount--;
                    if (readersCount == 0)
                    {
                        AppendLog("Last reader finished reading.");
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void AppendLog(string message)
        {
            Dispatcher.Invoke(() => logTextBox.AppendText(message + Environment.NewLine));
        }

        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            Thread writer1 = new Thread(Writer);
            Thread reader1 = new Thread(Reader);
            Thread reader2 = new Thread(Reader);

            writer1.Start();
            reader1.Start();
            reader2.Start();
        }
    }
}