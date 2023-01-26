using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace Screean_Server.View
{
    /// <summary>
    /// Interaction logic for Client_Uc.xaml
    /// </summary>
    public partial class Client_Uc : UserControl
    {
        public int Select_Index { get; set; } = -1;
        public bool Isokay { get; set; } = true;
        public Socket _socket { get; set; }
        public Client_Uc(int count,Socket socket)
        {
            InitializeComponent();
            Select_Index = count;
            _socket = socket;
          //  MessageBox.Show(count.ToString());
        }




        private void Screen_Img_Click(object sender, RoutedEventArgs e)
        {
            Isokay = false;
            FullHd EKRAN = new FullHd(_socket);
            if (!EKRAN.ShowDialog().Value)
            {
                EKRAN.ExitConnection();
                Isokay = true;

            }
        }
    }
}
