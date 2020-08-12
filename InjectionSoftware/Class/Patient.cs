using InjectionSoftware.Util.Scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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

        public string PatientFullSeperatedName
        {
            get
            {
                return PatientFullname.Replace(' ', '\n');
            }
        }

        public string DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsInpatient { get; set; }
        public string Referral { get; set; }
        public string UniqueExamIdentifier { get; set; }
        public string ExamCode { get; set; }
        public string ExamName { get; set; }

        public Patient()
        {

        }

        public Patient(Hl7file hl7File) {
            try
            {
                PatientID = hl7File.getSegment("PID").getString(3).Split('^')[0];
                PatientSurname = hl7File.getSegment("PID").getString(5).Split('^')[0];
                if (hl7File.getSegment("PID").getString(5).Split('^').Length > 1)
                {
                    PatientLastname = hl7File.getSegment("PID").getString(5).Split('^')[1];
                }
                else
                {
                    PatientLastname = "";
                }
                DateOfBirth = hl7File.getSegment("PID").getString(7);
                IsMale = hl7File.getSegment("PID").getString(8) == "M" ? true : false;
                PhoneNumber = hl7File.getSegment("PID").getString(13);
                IsInpatient = hl7File.getSegment("PV1").getString(2) == "I" ? true : false;
                Referral = hl7File.getSegment("PV1").getString(7).Replace('^', ' ');
                UniqueExamIdentifier = hl7File.getSegment("OBR").getString(2);
                ExamCode = hl7File.getSegment("OBR").getString(3).Split('-')[0];
                ExamName = hl7File.getSegment("OBR").getString(4);
            }
            catch (System.Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public Patient(XElement xElement)
        {

        }

        public XElement toXml()
        {
            XElement patient = new XElement("patient");

            XElement patientID = new XElement("patientID",PatientID);
            XElement patientLastname = new XElement("patientLastname", PatientLastname);
            XElement patientSurname = new XElement("patientSurname", PatientSurname);
            XElement dateOfBirth = new XElement("dateOfBirth", DateOfBirth);
            XElement isMale = new XElement("isMale", IsMale.ToString());
            XElement phoneNumber = new XElement("phoneNumber", IsMale.ToString());
            XElement isInpatient = new XElement("isInpatient", IsInpatient.ToString());
            XElement referral = new XElement("referral", Referral.ToString());
            XElement uniqueExamIdentifier = new XElement("uniqueExamIdentifier", UniqueExamIdentifier.ToString());
            XElement examCode = new XElement("examCode", ExamCode.ToString());
            XElement examName = new XElement("examName", ExamName.ToString());

            patient.Add(patientID);
            patient.Add(patientLastname);
            patient.Add(patientSurname);
            patient.Add(dateOfBirth);
            patient.Add(isMale);
            patient.Add(phoneNumber);
            patient.Add(isInpatient);
            patient.Add(referral);
            patient.Add(uniqueExamIdentifier);
            patient.Add(examCode);
            patient.Add(examName);

            return patient;
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
