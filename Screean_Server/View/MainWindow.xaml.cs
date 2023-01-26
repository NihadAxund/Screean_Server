using Screean_Server.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace Screean_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] C = new byte[4]{100,0,0,0};
        int i = 1;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowsViewModel(this);
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (C[i] >(byte)240)
            {
                 i++;
                 if (i==4)
                 {
                    i = 1;
                    for (int i = 1; i < 4; i++)
                     C[i] = 0;  
                 }
            }
            else C[i] += 20;
            //Windows_Screen.BorderBrush = Brushes.Wheat;
            SolidColorBrush SCB = new SolidColorBrush(Color.FromArgb(C[0], C[1], C[2], C[3]));
            Windows_Screen.BorderBrush = SCB;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => DragMove();
    }
}
