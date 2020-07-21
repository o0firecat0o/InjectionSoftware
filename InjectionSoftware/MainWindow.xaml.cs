using InjectionSoftware.Class;
using InjectionSoftware.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace InjectionSoftware
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private InjectionPage injectionPage = new InjectionPage();
        private RoomPage roomPage = new RoomPage();

        private DispatcherTimer dT = new DispatcherTimer();
        DispatcherTimer timer;

        
        public MainWindow()
        {
            InitializeComponent();
            //set the default selection to 1
            leftControlBar.SelectedIndex = 0;

            //Sets the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.00);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                DateTime dt = DateTime.Now;
                time.Text = "" + DateTime.Now.Hour.ToString("D2") + ":"
                + DateTime.Now.Minute.ToString("D2") + ":"
                + DateTime.Now.Second.ToString("D2");
                Console.Out.WriteLine(time.Text);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hellowrold");
        }

        // switching pages
        private void leftControlBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            switch (lv.SelectedIndex)
            {
                case 0:
                    Main.Content = injectionPage;
                    break;
                case 1:
                    Main.Content = roomPage;
                    break;
                case 2:

                    break;
            }            
        }
    }
}
