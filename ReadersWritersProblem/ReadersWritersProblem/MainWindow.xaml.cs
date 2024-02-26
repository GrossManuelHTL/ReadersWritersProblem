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

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Writer()
        {
            for (int i = 0; i < 5; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    imageLeft.Margin = new Thickness(100, 0, 0, 0);
                });
                lock (lockObject)
                {

                    AppendLog($"Writer is writing. Shared Data before write: {sharedData}");
                    sharedData++;
                    AppendLog($"Writer finished writing. Shared Data after write: {sharedData}");

                }
                Dispatcher.Invoke(() =>
                {
                    imageLeft.Margin = new Thickness(50, 0, 0, 0);
                });
                Thread.Sleep(1000);
            }
        }

        private void Reader(Image image)
        {
            for (int i = 0; i < 5; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    ScaleTransform scaleTransform = new ScaleTransform(-1, 1);
                    image.RenderTransform = scaleTransform;
                });
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

                Dispatcher.Invoke(() =>
                {
                    image.RenderTransform = Transform.Identity;
                });
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
            Thread reader1 = new Thread(() => Reader(imageRight1));
            Thread reader2 = new Thread(() => Reader(imageRight2));

            writer1.Start();
            reader1.Start();
            reader2.Start();
        }

        /* private void AnimationArtist()
         {
             imageLeft.Margin = new Thickness(100, 250, 0, 0);
         }*/

    }
}