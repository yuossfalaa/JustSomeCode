using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace JustSomeCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int imageIndex = 0;


        public MainWindow()
        {
            InitializeComponent();
            MainGrid.Visibility = Visibility.Collapsed;
            this.MaxHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;
            //Cancel Animation in Debug mode
            #if DEBUG
            imageIndex = 148;
            #endif

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            timer.Tick += TimerTick;
            timer.Start();
        }
        //Starting Animation 
        private void TimerTick(object sender, EventArgs e)
        {
            if (imageIndex <= 9)
            {
                Logo.Source = new BitmapImage(
                new Uri("pack://application:,,,/Resources/Logo/Logo_0000" + imageIndex + ".png"));
                imageIndex++;
            }
            else if (imageIndex <= 99)
            {
                Logo.Source = new BitmapImage(
                new Uri("pack://application:,,,/Resources/Logo/Logo_000" + imageIndex + ".png"));
                imageIndex++;
            }
            else if (imageIndex >= 99)
            {
                Logo.Source = new BitmapImage(
                new Uri("pack://application:,,,/Resources/Logo/Logo_00" + imageIndex + ".png"));
                imageIndex++;
            }
            if (imageIndex == 149)
            {

                Logo.Visibility = Visibility.Collapsed;
                MainGrid.Visibility = Visibility.Visible;
                timer.Stop();

            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.1));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {

                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

    }
}
