using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InjectionSoftware.ViewModels
{
    internal class ConsoleLogPageViewModel : INotifyPropertyChanged
    {

        private string _ConsoleLogString = "pending";
        public string ConsoleLogString
        {
            get { return _ConsoleLogString; }
            set
            {
                _ConsoleLogString = value;
                OnPropertyChanged("ConsoleLogString");
            }
        }

        public void Init()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                ConsoleLogString = sw.ToString();
            });
            dispatcherTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
