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
using System.Windows.Media.Animation;
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
        private int writerCount = 0;
        

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Writer()
        {
            var randomdings = new Random();
            int delay;
            for (int i = 0; i < 5; i++)
            {
                if (readersCount == 0)
                {
                    lock (lockObject)
                    {
                        writerCount++;
                        Dispatcher.Invoke(() =>
                        {
                            imageLeft.Margin = new Thickness(200, 0, 0, 0);
                        });

                        AppendLog($"Writer is writing. Shared Data before write: {sharedData}");
                        sharedData++;
                        delay = randomdings.Next(500, 1500);
                        Thread.Sleep(delay);
                        AppendLog($"Writer finished writing. Shared Data after write: {sharedData}");

                        Dispatcher.Invoke(() =>
                        {
                            imageLeft.Margin = new Thickness(0, 0, 0, 0);
                        });
                    }

                    writerCount++;
                }
                Thread.Sleep(1000);
            }
        }

        private void Reader(Image image)
        {
            var randommm = new Random();
            int delay;
            for (int i = 0; i < 5; i++)
            {
                
                    Thread.Sleep(500);
                    lock (lockObject)
                    {
                        readersCount++;
                        Dispatcher.Invoke(() =>
                        {
                            ScaleTransform scaleTransform = new ScaleTransform(-1, 1);
                            image.RenderTransform = scaleTransform;
                        });
                        if (readersCount == 1)
                        {
                            AppendLog($"First reader is reading. Shared Data: {sharedData}");

                        }
                    }

                    delay = randommm.Next(300, 1000);
                    Thread.Sleep(delay);

                    lock (lockObject)
                    {
                        readersCount--;
                        if (readersCount == 0)
                        {
                            AppendLog("Last reader finished reading.");
                        }
                        Dispatcher.Invoke(() =>
                        {
                            image.RenderTransform = Transform.Identity;
                        });
                    }

                    delay = randommm.Next(300, 1000);
                    Thread.Sleep(delay);
                
            }
        }

        private void AppendLog(string message)
        {
            Dispatcher.Invoke(() => logTextBox.AppendText(message + Environment.NewLine));
        }

        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            Thread writer1 = new Thread(Writer);
            Thread reader1 = new Thread(() => Reader(imageRight1));
            Thread reader2 = new Thread(() => Reader(imageRight2));

            writer1.Start();
            reader1.Start();
            reader2.Start();
        }
    }
}