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
        public Patient Patient { get; }

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
                return (CaseNumber - 1) % 10;
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

        /// <summary>
        /// List of RP injected, can be more than two, but only the first two will be displayed
        /// </summary>
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
                OnPropertyChanged("RPsingleName");
                OnPropertyChanged("RPmultipleName1");
                OnPropertyChanged("RPmultipleName2");
            }
        }

        /// <summary>
        /// The color of the first injected RP
        /// </summary>
        public Brush RP1
        {
            get
            {
                if (RPs.Count > 0)
                {
                    return RPs[0].Color;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        /// <summary>
        /// The color of the second injected RP
        /// </summary>
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

        /// <summary>
        /// The textblock of RP if there is only 1 RP injection
        /// </summary>
        public string RPsingleName
        {
            get {
                if (RPs.Count == 1)
                    return RPs[0].SimplifiedName;
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// The textblock of the 1st RP if there is 2 or more RP injection
        /// </summary>
        public string RPmultipleName1
        {
            get
            {
                if (RPs.Count > 1)
                    return RPs[0].SimplifiedName;
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// The textblock of the 2nd RP if there is 2 or more RP injection
        /// </summary>
        public string RPmultipleName2
        {
            get
            {
                if (RPs.Count > 1)
                    return RPs[1].SimplifiedName;
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// The assigned doctor for this injection
        /// </summary>
        private Doctor _Doctor;

        public Doctor Doctor
        {
            get
            {
                return _Doctor;
            }
            set
            {
                _Doctor = value;
                OnPropertyChanged("Doctor");
                OnPropertyChanged("DoctorColor");
            }
        }

        /// <summary>
        /// The color representing the assigned doctor for this injection
        /// </summary>
        public Brush DoctorColor
        {
            get
            {
                return Doctor.Color;
            }
        }

        /// <summary>
        /// The injection time for this injection
        /// </summary>
        public DateTime InjectionTime { get; set; }

        public float UptakeTime { get; set; }

        public Injection(Patient patient, int CaseNumber, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime)
        {
            Patient = patient;
            this.CaseNumber = CaseNumber;
            this.RPs = RPs;
            this.Doctor = Doctor;
            this.UptakeTime = UptakeTime;
            this.InjectionTime = InjectionTime;
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
