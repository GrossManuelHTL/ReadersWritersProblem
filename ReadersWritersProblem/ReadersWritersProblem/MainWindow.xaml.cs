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

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly double speed = 1; // Geschwindigkeit der Bewegung
        private readonly double targetY = 100; // Y-Position, bis zu der sich die Bilder bewegen sollen
        private bool movingLeft = true; // Richtung der linken Bewegung
        private bool movingRight = true; // Richtung der rechten Bewegung

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Timer für die Bildbewegung einrichten
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveImage(imageLeft, speed, -speed, ref movingLeft);
            MoveImage(imageRight, -speed, -speed, ref movingRight);
        }

        private void MoveImage(Image image, double deltaX, double deltaY, ref bool moving)
        {
            double currentX = Canvas.GetLeft(image);
            double currentY = Canvas.GetTop(image);
            
            if (moving && currentY > targetY)
            {
                MoveImage(image, deltaX, deltaY);
            }
            else
            {
                moving = false;
                MoveImage(image, -deltaX, deltaY);
            }
            
            if (!moving && currentY <= 0)
            {
                moving = true;
            }
        }

        private void MoveImage(Image image, double deltaX, double deltaY)
        {
            double newX = Canvas.GetLeft(image) + deltaX;
            double newY = Canvas.GetTop(image) + deltaY;
            Canvas.SetLeft(image, newX);
            Canvas.SetTop(image, newY);
        }

    }
}