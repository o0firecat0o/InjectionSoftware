using InjectionSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InjectionSoftware.Class
{
    public class Injection : INotifyPropertyChanged
    {
        private Patient _Patient;
        public Patient Patient
        {
            get
            {
                return _Patient;
            }
            set
            {
                _Patient = value;
                OnPropertyChanged("Patient");
            }
        }

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
                OnPropertyChanged("CaseNumberText");
                OnPropertyChanged("Row");
                OnPropertyChanged("Column");
            }
        }

        public string CaseNumberText
        {
            get
            {
                if (Modality == Modality.getModality("PETCT"))
                {
                    return CaseNumber.ToString();
                }
                else if (Modality == Modality.getModality("PETMR"))
                {
                    return "PETMR\n" + CaseNumber.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public int Row
        {
            get
            {
                if (Modality == Modality.getModality("PETCT"))
                {
                    return (CaseNumber - 1) % 10;
                }
                else if (Modality == Modality.getModality("PETMR"))
                {
                    return (5 + CaseNumber - 1) % 10;
                }
                else
                {
                    return 1000;
                }
            }
        }

        public int Column
        {
            get
            {

                if (Modality == Modality.getModality("PETCT"))
                {
                    return (CaseNumber - 1) / 10;
                }
                else if (Modality == Modality.getModality("PETMR"))
                {
                    return (CaseNumber - 1) / 10 + 2;
                }
                else
                {
                    return 1000;
                }

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
            get;
            set;
        }

        private Modality _Modality;
        public Modality Modality
        {
            get
            {
                return _Modality;
            }
            set
            {
                _Modality = value;
                OnPropertyChanged("Modality");
                OnPropertyChanged("CaseNumberText");
            }
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

        public PatientStatus patientStatus
        {
            get
            {
                return _patientStatus;
            }
            set
            {
                _patientStatus = value;
                OnPropertyChanged("patientStatus");
                OnPropertyChanged("BackgraoundBrush");
            }
        }
        private PatientStatus _patientStatus;

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
                if (patientStatus == PatientStatus.getPatientStatus("Discharged"))
                {
                    return Brushes.DarkGray;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        /// <summary>
        /// Used for searching, when the search is performed, the injection with the matching searchstring will be visible
        /// </summary>
        private bool _isVisible = true;
        public bool isVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("isVisible");
            }
        }

        //TODO: modify the search string to include other stuff, eg doctor name

        /// <summary>
        /// the string used for quick searching in injection page
        /// </summary>
        public string SearchString = "";
        public void updateSearchString()
        {
            SearchString = "";
            SearchString += Patient.PatientFullname + "_";
            SearchString += Patient.PatientID + "_";
            SearchString += AccessionNumber + "_";
            SearchString += Patient.WardNumber + "_";

            SearchString += Patient.PhoneNumber + "_";
            SearchString += Patient.Referral + "_";
            SearchString += Patient.UniqueExamIdentifier + "_";
            SearchString += SelectedRoom.Name + "_";

            foreach (RP rP in RPs)
            {
                SearchString += rP.Name + "_";
            }
        }

        public void Update()
        {
            OnPropertyChanged("InjectionTimeSlider");
        }

        public XElement toXML()
        {
            XElement injection = new XElement("injection");

            XElement accessionNumber = new XElement("accessionNumber", AccessionNumber);

            XElement modality = new XElement("modality", Modality.Name);

            XElement patientID = new XElement("patientID", Patient.PatientID);
            XElement patientSurname = new XElement("patientSurname", Patient.PatientSurname);
            XElement patientLastname = new XElement("patientLastname", Patient.PatientLastname);

            XElement uniqueExamIdentifier = new XElement("uniqueExamIdentifier", Patient.UniqueExamIdentifier);
            XElement examCode = new XElement("examCode", Patient.ExamCode);
            XElement dateOfBirth = new XElement("dateOfBirth", Patient.DateOfBirth);
            XElement gender = new XElement("gender", Patient.IsMale.ToString());
            XElement inpatient = new XElement("inpatient", Patient.IsInpatient.ToString());
            XElement wardNumber = new XElement("wardNumber", Patient.WardNumber);

            XElement rp1 = new XElement("rp1");
            XElement rp2 = new XElement("rp2");
            if (RPs.Count >= 1)
            {
                rp1.Value = RPs[0].Name;
                if (RPs.Count >= 2)
                {
                    rp2.Value = RPs[1].Name;
                }
                else
                {
                    rp2.Value = "";
                }
            }
            else
            {
                rp1.Value = "";
                rp2.Value = "";
            }

            XElement doctor = new XElement("doctor", Doctor.Name);
            XElement uptakeTime = new XElement("uptakeTime", UptakeTime.ToString());
            XElement injectionTime = new XElement("injectionTime", InjectionTime.ToString());
            XElement selectedRoom = new XElement("selectedRoom", SelectedRoom.Name);

            XElement isContrast = new XElement("isContrast", this.isContrast.ToString());
            XElement isDelay = new XElement("isDelay", this.isDelay.ToString());
            XElement patientStatus = new XElement("patientStatus", this.patientStatus.Name);

            injection.Add(accessionNumber);
            injection.Add(modality);
            injection.Add(patientID);
            injection.Add(patientSurname);
            injection.Add(patientLastname);

            injection.Add(uniqueExamIdentifier);
            injection.Add(examCode);
            injection.Add(dateOfBirth);
            injection.Add(gender);
            injection.Add(inpatient);

            injection.Add(rp1);
            injection.Add(rp2);
            injection.Add(doctor);
            injection.Add(uptakeTime);
            injection.Add(injectionTime);
            injection.Add(selectedRoom);
            injection.Add(isContrast);
            injection.Add(isDelay);
            injection.Add(patientStatus);
            injection.Add(wardNumber);

            return injection;
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
