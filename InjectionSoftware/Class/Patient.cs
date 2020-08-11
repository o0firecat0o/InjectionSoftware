using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace InjectionSoftware.Class
{
    public class Patient : INotifyPropertyChanged
    {
        public string PatientSurname
        {
            get
            {
                return _PatientSurname;
            }
            set
            {
                _PatientSurname = value;
                OnPropertyChanged("PatientSurname");
                OnPropertyChanged("PatientFullname");
            }
        }
        private string _PatientSurname;

        public string PatientLastname
        {
            get
            {
                return _PatientLastname;
            }
            set
            {
                _PatientLastname = value;
                OnPropertyChanged("PatientLastname");
                OnPropertyChanged("PatientFullname");
            }
        }
        private string _PatientLastname;

        public string PatientID
        {
            get
            {
                return _PatientID;
            }set
            {
                _PatientID = value;
                OnPropertyChanged("PatientID");
            }
        }
        private string _PatientID;

        public string PatientFullname
        {
            get
            {
                return PatientSurname + " " + PatientLastname;
            }
        }

        public string DateOfBirth { get; set; }
        public bool isMale { get; set; }
        public string PhoneNumber { get; set; }
        public bool isInpatient { get; set; }
        public string ReferralName { get; set; }
        public float ReferralNo { get; set; }
        public string UniqueExamIdentifier { get; set; }
        public string ExamCode { get; set; }
        public string ExamName { get; set; }



        public Patient(string patientID, string patientSurname, string patientLastname)
        {
            this.PatientID = patientID;
            this.PatientLastname = patientLastname;
            this.PatientSurname = patientSurname;
            PatientManager.AddPatient(this);

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
