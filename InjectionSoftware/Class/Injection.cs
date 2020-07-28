using InjectionSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace InjectionSoftware.Class
{
    public class Injection : INotifyPropertyChanged
    {
        public Patient Patient { get; }

        private int _CaseNumber = 1;

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

        private int _CaseNumberOfDoctor;

        public int CaseNumberOfDoctor
        {
            get
            {
                return _CaseNumberOfDoctor;
            }
            set
            {
                _CaseNumberOfDoctor = value;
                OnPropertyChanged("CaseNumberOfDoctor");
            }
        }

        public string AccessionNumber
        {
            get; set;
        }

        /// <summary>
        /// List of RP injected, can be more than two, but only the first two will be displayed
        /// </summary>
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
        private ObservableCollection<RP> _RPs;

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
            get
            {
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
        private Doctor _Doctor;

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
        public DateTime InjectionTime
        {
            get
            {
                return _InjectionTime;
            }
            set
            {
                _InjectionTime = value;
                OnPropertyChanged("InjectionTime");
                OnPropertyChanged("InjectionTimeString");
                OnPropertyChanged("ImageTimeString");
                OnPropertyChanged("InjectionTimeSlider");
            }
        }
        public DateTime _InjectionTime;

        public float UptakeTime
        {
            get
            {
                return _UptakeTime;
            }
            set
            {
                _UptakeTime = value;
                OnPropertyChanged("UptakeTime");
                OnPropertyChanged("InjectionTime");
                OnPropertyChanged("InjectionTimeString");
                OnPropertyChanged("ImageTimeString");
                OnPropertyChanged("InjectionTimeSlider");
            }
        }
        private float _UptakeTime;

        /// <summary>
        /// The string of the injection time in HH:mm form
        /// </summary>
        public string InjectionTimeString
        {
            get
            {
                return InjectionTime.ToString("HH:mm");
            }
        }

        /// <summary>
        /// The string of the image time in HH:mm form
        /// </summary>
        public string ImageTimeString
        {
            get
            {
                return InjectionTime.AddMinutes(UptakeTime).ToString("HH:mm");
            }
        }

        public float InjectionTimeSlider
        {
            get
            {
                return (float)(DateTime.Now - InjectionTime).TotalMinutes / UptakeTime * 100f;
            }
        }

        private Room _SelectedRoom;
        public Room SelectedRoom
        {
            get
            {
                return _SelectedRoom;
            }
            set
            {
                _SelectedRoom = value;
                OnPropertyChanged("SelectedRoom");
            }
        }
                
        public bool isContrast
        {
            get
            {
                return _isContrast;
            }
            set
            {
                _isContrast = value;
                OnPropertyChanged("isContrast");
                OnPropertyChanged("ContrastDelayString");
            }
        }
        private bool _isContrast = false;

        public bool isDelay
        {
            get
            {
                return _isDelay;
            }
            set
            {
                _isDelay = value;
                OnPropertyChanged("isDelay");
                OnPropertyChanged("ContrastDelayString");
            }
        }
        private bool _isDelay = false;

        public bool isDischarge
        {
            get
            {
                return _isDischarge;
            }
            set
            {
                _isDischarge = value;
                OnPropertyChanged("isDischarge");
                OnPropertyChanged("BackgroundBrush");
            }
        }
        private bool _isDischarge = false;

        public string ContrastDelayString
        {
            get
            {
                string returnstring = "";
                if (isContrast)
                {
                    returnstring += "+C";
                }
                if (isDelay)
                {
                    returnstring += "+D";
                }
                return returnstring;
            }
        }

        /// <summary>
        /// the background brush of the injection panel for each injection, affected by whether the patient is discharged or not
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (isDischarge)
                {
                    return Brushes.DarkGray;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        public Injection(Patient patient, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom)
        {
            Patient = patient;
            this.RPs = RPs;
            this.Doctor = Doctor;
            this.UptakeTime = UptakeTime;
            this.InjectionTime = InjectionTime;
            this.SelectedRoom = SelectedRoom;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            dispatcherTimer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                OnPropertyChanged("InjectionTimeSlider");
            });
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
