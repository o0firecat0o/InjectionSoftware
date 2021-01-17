﻿using InjectionSoftware.Network;
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
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Start();
            float r = 0;
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                r++;
                if (DateTime.Today.CompareTo(today) != 0 || r>1)
                {
                    Console.Out.WriteLine("[WindowAutoRestart] date changed. Proceeeding to auto restart program");
                    WindowConfig.IsAutoRestart = 1;

                    WindowConfig.WindowHeight = MainWindow.window.Height;
                    WindowConfig.WindowWidth = MainWindow.window.Width;
                    WindowConfig.WindowTop = MainWindow.window.Top;
                    WindowConfig.WindowLeft = MainWindow.window.Left;
                    WindowConfig.WindowState = MainWindow.window.WindowState;
                    if (NetworkManager.isServer)
                    {
                        WindowConfig.IsServer = 1;
                    }
                    else
                    {
                        WindowConfig.IsServer = 0;
                    }

                    WindowConfig.Save();

                    System.Windows.Forms.Application.Restart();
                    System.Windows.Application.Current.Shutdown();

                    timer.Stop();
                }
            });
        }
    }
}
