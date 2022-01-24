using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChibikaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double MinLeft = 0;
        double MinTop = 0;

        bool isGrounded = true;
        public MainWindow()
        {
            InitializeComponent();
            var rightScreen = System.Windows.Forms.Screen.AllScreens[0];
            var rightWorkingArea = rightScreen.WorkingArea;
            MinTop = (rightWorkingArea.Bottom - Height);
            MinLeft = rightWorkingArea.Left;
            Top = MinTop;
            Left = new Random().Next(100, 1500);
            Debug.WriteLine("Top: " + Top);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var randomWork = new Random();
            this.Dispatcher.Invoke(() =>
            {
                    Task.Run(() =>
                    {
                        var nextWork = DateTime.Now + TimeSpan.FromSeconds(randomWork.Next(5, 15));

                        Debug.WriteLine(nextWork.ToString());
                        Debug.WriteLine(DateTime.Now);
                        while (true)
                        {
                            if (DateTime.Now >= nextWork)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    if (this.Top == MinTop) isGrounded = true; else { isGrounded = false; }
                                Debug.WriteLine(isGrounded);
                                if (isGrounded)
                                {
                                    var randomWork_ = new Random();
                                    int rw = randomWork_.Next(0, 3);
                                    switch (rw)
                                    {
                                        case 0: // Nothing
                                            Debug.WriteLine(rw);
                                            break;
                                        case 1:
                                            Debug.WriteLine(rw);
                                            moveTo(randomWork_.Next(0, 1500), 0);
                                            break;
                                        case 2:
                                            Debug.WriteLine(rw);
                                            jump();
                                            break;
                                    }
                                }

                                nextWork = DateTime.Now + TimeSpan.FromSeconds(randomWork.Next(5, 15));
                                Debug.WriteLine(nextWork.ToString());

                                });
                            }
                        }

                        Task.Delay(250).Wait();
                    });
            });
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            monika.Source = new BitmapImage(new Uri("pack://application:,,,/Chibi/m_sticker_2.png"));
            this.BeginAnimation(TopProperty, null);
            this.DragMove();
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.BeginAnimation(LeftProperty, null);
            monika.Source = new BitmapImage(new Uri("pack://application:,,,/Chibi/m_sticker_1.png"));
            var rightScreen = System.Windows.Forms.Screen.AllScreens[0];
            var rightWorkingArea = rightScreen.WorkingArea;

            var falling = new DoubleAnimation();
            falling.From = this.Top;
            falling.To = MinTop;
            falling.Duration = new Duration(TimeSpan.FromSeconds(1));

            this.BeginAnimation(TopProperty, falling);
        }
        public void moveTo(int x, int y)
        {
            var falling = new DoubleAnimation();
            falling.From = this.Left;
            falling.To = x;
            falling.Duration = new Duration(TimeSpan.FromSeconds(10));

            this.BeginAnimation(LeftProperty, falling);
        }
        public void jump()
        {
            var falling = new DoubleAnimation();
            falling.From = this.Top;
            falling.To = (MinTop - new Random().Next(50,100));
            falling.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            falling.Completed += (sender, args) =>
            {
                var falling1 = new DoubleAnimation();
                falling1.From = this.Top;
                falling1.To = MinTop;
                falling1.Duration = new Duration(TimeSpan.FromSeconds(1));

                this.BeginAnimation(TopProperty, falling1);
            };
            this.BeginAnimation(TopProperty, falling);
        }

    }
}
