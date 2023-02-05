using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Shapes;

namespace Screean_Server.View
{
    /// <summary>
    /// Interaction logic for FullHd.xaml
    /// </summary>
    public partial class FullHd : Window
    {
        private Socket Client { get; set; }
        private bool Isokay { get; set; } = true;
        public FullHd(Socket socket)
        {
            InitializeComponent();
            Client= socket;
            Task.Run(() => { Connection(); });
        }
        private void ExitMethod()
        {
            Isokay = false;
            this.Close();
        }
        public void ExitConnection()
        {
            Isokay = false;
        }
        private void Connection()
        {
            int length = 0;
            while (Isokay)
            {
                try
                {
                    var bytes = new byte[350000];
                    length = Client.Receive(bytes);
                   // MessageBox.Show(length.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        var bitmapImage = new BitmapImage();
                        byte_txt.Content = length.ToString();
                        try
                        {
                            using (var memoryStream = new MemoryStream(bytes, 0, length))
                            {
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = memoryStream;
                                bitmapImage.EndInit();
                                bitmapImage.Freeze();
                                memoryStream.Close();
                            }
                            HD_Screen.Background = new ImageBrush(bitmapImage);

                        }
                        catch (Exception)
                        {
                        }
                    }));

                }
                catch (Exception)
                {
                    ExitMethod();
                    break;
                    
                }
            }
        }
    }
}
