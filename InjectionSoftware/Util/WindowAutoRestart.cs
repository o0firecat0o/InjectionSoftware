using InjectionSoftware.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InjectionSoftware.Util
{
    public class WindowAutoRestart
    {

        public static DateTime today;

        public static void Init()
        {
            today = DateTime.Today;
            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                //only auto restart if it is server
                //if (!NetworkManager.isServer)
                //{
                //    return;
                //}
                Console.Out.WriteLine("[WindowAutoRestart] Pulse..");

                if (!isSameDate())
                {
                    timer.Stop();

                    NetworkManager.server.TCPBroadcastMessage("autoRestart", "");

                    System.Threading.Thread.Sleep(5000);

                    Console.Out.WriteLine("[WindowAutoRestart] date changed. Proceeeding to auto restart program");
                    AutoRestart();
                }
            });
        }

        public static bool isSameDate()
        {
            return DateTime.Today.CompareTo(today) == 0;
        }

        public static void AutoRestart()
        {
            Console.Out.WriteLine("[WindowAutoRestart] Restarting program...");
            WindowConfig.IsAutoRestart = 1;

            WindowConfig.WindowHeight = MainWindow.window.Height;
            WindowConfig.WindowWidth = MainWindow.window.Width;
            WindowConfig.WindowTop = MainWindow.window.Top;
            WindowConfig.WindowLeft = MainWindow.window.Left;
            WindowConfig.WindowState = MainWindow.window.WindowState;

            //Obsolete
            WindowConfig.IsServer = 0;

            WindowConfig.Save();

            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
