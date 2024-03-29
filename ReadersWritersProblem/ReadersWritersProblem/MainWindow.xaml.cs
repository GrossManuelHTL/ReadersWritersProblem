﻿using System;
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
        private string[] art = { "images/art1.png", "images/art2.jpg", "images/art3.jpg", "images/art4.jpg", "images/art5.jpg" };
        private Random randomArt = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void ChangeImage(string imagePath)
        {
            BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            imageBox.Source = image;
        }
        private void Writer(Image image)
        {
            var rnd = new Random();
            int delay;
            string toPlace = art[randomArt.Next(0, 5)];
            for (int i = 0; i < 15; i++)
            {
                if (readersCount == 0)
                {
                    lock (lockObject)
                    {
                        Application.Current.Dispatcher.Invoke(() => ChangeImage(art[randomArt.Next(0, 5)]));   
                        writerCount++;
                        Dispatcher.Invoke(() =>
                        {
                            image.Margin = new Thickness(200, 0, 0, 0);
                        });

                        AppendLog($"Writer {writerCount} is writing. Shared Data before write: {sharedData}");
                        sharedData++;
                        delay = rnd.Next(500, 1500);
                        Thread.Sleep(delay);
                        AppendLog($"Writer finished writing. Shared Data after write: {sharedData}");

                        Dispatcher.Invoke(() =>
                        {
                            image.Margin = new Thickness(0, 0, 0, 0);
                        });
                    }

                    writerCount--;
                }
                Thread.Sleep(1000);
            }
        }

        private void Reader(Image image, int n)
        {
            var rnd = new Random();
            int delay;
            for (int i = 0; i < 15; i++)
            {
                
                    Thread.Sleep(400);
                    if (writerCount == 0)
                    {
                        lock (lockObject)
                        {
                            readersCount++;
                            Dispatcher.Invoke(() =>
                            {
                                ScaleTransform scaleTransform = new ScaleTransform(-1, 1);
                                image.RenderTransform = scaleTransform;
                            });
                            if (readersCount >= 1)
                            {
                                AppendLog($"Reader {n} is reading. Shared Data: {sharedData}");

                            }
                        }


                        delay = rnd.Next(300, 1000);
                        Thread.Sleep(delay);

                        lock (lockObject)
                        {
                            readersCount--;
                            if (readersCount == 0)
                            {
                                AppendLog("Last reader finished reading.");
                            }

                            Dispatcher.Invoke(() => { image.RenderTransform = Transform.Identity; });
                        }

                        delay = rnd.Next(300, 1000);
                        Thread.Sleep(delay);
                    }
            }
        }

        private void AppendLog(string message)
        {
            Dispatcher.Invoke(() => logTextBox.AppendText(message + Environment.NewLine));
        }

        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            Thread writer1 = new Thread(() => Writer(imageLeft1));
            Thread reader1 = new Thread(() => Reader(imageRight1, 1));
            Thread reader2 = new Thread(() => Reader(imageRight2, 2));
            Thread reader3 = new Thread(() => Reader(imageRight3,3));

            writer1.Start();
            reader1.Start();
            reader2.Start();
            reader3.Start();
        }
    }
}