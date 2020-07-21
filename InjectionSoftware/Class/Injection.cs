using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public Injection(Patient patient, int CaseNumber)
        {
            _Patient = patient;
            _CaseNumber = CaseNumber;
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
