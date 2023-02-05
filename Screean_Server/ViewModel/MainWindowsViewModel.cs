using Screean_Server.Commnad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Reflection;
using System.Windows.Controls;
using Screean_Server.View;
using System.Threading;

namespace Screean_Server.ViewModel
{
    public class MainWindowsViewModel : BaseViewModel
    {
        private MainWindow _MW { get; set; }
        private Task t1 { get; set; }
        public RelayCommand Exit_btn {get; set; }
        private List<Socket> _Sockets = new List<Socket>();
        private List<Button> _btn = new List<Button>();
        public MainWindowsViewModel(MainWindow MW) {
            _MW = MW;
            Exit_btn = new RelayCommand(CanExit);
            
            t1 = new Task(Connection, TaskCreationOptions.LongRunning);
            t1.Start();
          //  Task.Run(() => { Connection(); });
        }
        private void CanExit (object parametr)
        {
            _MW.Close();
        }
        private Client_Uc NewUC(int i,Socket socket)
        {
            Client_Uc btn = new Client_Uc(i,socket);
            btn.Width = 160; btn.Height = 120;
            btn.Margin = new Thickness(27, 15, 22, 20);
            return btn;
        }
        private void Connection()
        {
            var idAddres = IPAddress.Any;
            int port = 27009;
            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    var EP = new IPEndPoint(idAddres, port);
                    socket.Bind(EP);
                    socket.Listen(10);
                    try
                    {
                        while (true)
                        {
                            var client = socket.Accept();
                            lock (client)
                            {
                                Task.Run(() =>
                                {
                               
                                    var length = 0;
                                    var bytes = new byte[300000];
                                    _Sockets.Add(client);
                                    int index = -1;
                                    _MW.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        var UC = NewUC(_MW.Team_List.Children.Count,client);
                                       // MessageBox.Show(_MW.Team_List.Children.Count.ToString());
                                        UC.Select_Index = _MW.Team_List.Children.Count;
                                        _MW.Team_List.Children.Add(UC);
                                        index = _MW.Team_List.Children.IndexOf(UC);
                                        //if (_MW.Team_List.Children[index] is Client_Uc uc)
                                        //{
                                        //    uc.Select_Index = index;
                                        //}
                                    }));
                                    do
                                    {
                                        try
                                        {
                                          //  MessageBox.Show(IND.ToString());
                                           // Thread.Sleep(1000);
                                            length = client.Receive(bytes);
                                            int IND = _Sockets.IndexOf(client);
                                           // Task.Delay(100);
                                            _MW.Dispatcher.BeginInvoke(new Action(() => { 
                                                if (_MW.Team_List.Children[IND] is Client_Uc user && user.Isokay)
                                                {
                                                    try
                                                    {
                                                        var bitmapImage = new BitmapImage();
                                                        using (var memoryStream = new MemoryStream(bytes, 0, length))
                                                        {
                                                            bitmapImage.BeginInit();
                                                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                                            bitmapImage.StreamSource = memoryStream;
                                                            bitmapImage.EndInit();
                                                            memoryStream.Close();
                                                        }
                                                        user.Screen_Img.Background = new ImageBrush(bitmapImage);

                                                    }
                                                    catch
                                                    {
                                                 
                                                    }
                                                    
                                                }                                  
                                            }));
                                        }
                                        catch (Exception)
                                        {
                                            int ind = _Sockets.IndexOf(client);
                                         //   MessageBox.Show(ind.ToString());
                                             client.Shutdown(SocketShutdown.Both);
                                            _Sockets[ind].Shutdown(SocketShutdown.Both);
                                            _Sockets.RemoveAt(ind);
                                            _MW.Dispatcher.Invoke(new Action(() => { _MW.Team_List.Children.RemoveAt(ind); }));
                                            break;
                                        }
                                    }while (true);

                                });

                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("a");
                    }
             

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
