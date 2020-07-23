using InjectionSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Class
{
    public class Injection : INotifyPropertyChanged
    {
        private readonly Patient _Patient;

        public Patient Patient
        {
            get
            {
                return _Patient;
            }
        }

        private int _CaseNumber;

        public int CaseNumber 
        {
            get
            {
                return _CaseNumber;
            }
            set
            {
                _CaseNumber = value;
                OnPropertyChanged("CaseNumber");
                OnPropertyChanged("Row");
                OnPropertyChanged("Column");
            }
        }
        public int Row
        {
            get
            {
                return (CaseNumber - 1) % 10 ;
            }
        }

        public int Column
        {
            get
            {
                return (CaseNumber - 1) / 10;
            }
        }

        public string AccessionNumber
        {
            get; set;
        }

        private ObservableCollection<RP> _RPs;

        public ObservableCollection<RP> RPs
        {
            get
            {
                return _RPs;
            }
            set
            {
                _RPs = value;
                OnPropertyChanged("RP1");
                OnPropertyChanged("RP2");
            }
        }

        public Brush RP1
        {
            get
            {
                if (RPs.Count>0)
                {
                    return RPs[0].Color;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        public Brush RP2
        {
            get
            {
                if (RPs.Count > 0)
                {
                    if (RPs.Count > 1)
                    {
                        return RPs[1].Color;
                    }
                    else
                    {
                        return RPs[0].Color;
                    }
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        public Injection(Patient patient, int CaseNumber, ObservableCollection<RP> RPs)
        {
            _Patient = patient;
            this.CaseNumber = CaseNumber;
            this.RPs = RPs;
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
