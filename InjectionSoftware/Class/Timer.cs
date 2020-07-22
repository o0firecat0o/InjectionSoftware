using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InjectionSoftware.Class
{
    public class Timer : INotifyPropertyChanged
    {
        private float _h = 30;
        private float _m = 30;
        private float _s = 30;

        public Timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                DateTime dateTime = DateTime.Now;
                S = ((float)dateTime.Millisecond / 1000 + dateTime.Second)/60f*100f;
                M = (float)dateTime.Minute / 60f * 100f;
                H = (float)(dateTime.Hour % 12) / 12f * 100f;
            });
        }

        public float H
        {
            get
            {
                return _h;
            }
            set
            {
                _h = value;
                OnPropertyChanged("H");
            }
        }

        public float M
        {
            get
            {
                return _m;
            }
            set
            {
                _m = value;
                OnPropertyChanged("M");
            }
        }

        public float S
        {
            get
            {
                return _s;
            }
            set
            {
                _s = value;
                OnPropertyChanged("S");
            }
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
