using InjectionSoftware.Util;
using InjectionSoftware.Util.Scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
                OnPropertyChanged("PatientFullSeperatedName");
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
                OnPropertyChanged("PatientFullSeperatedName");
            }
        }
        private string _PatientLastname;

        public string PatientID
        {
            get
            {
                return _PatientID;
            }
            set
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

        public string PatientCensoredName
        {
            get
            {
                // this function change CHAN TAI MAN
                // into CHAN Txx Mxx

                bool afterfirst = false;
                string returnstring = PatientSurname + " ";
                for (int i = 0; i < PatientLastname.Length; i++)
                {
                    if (afterfirst == false)
                    {
                        returnstring += PatientLastname[i];
                        afterfirst = true;
                    }
                    else if (PatientLastname[i] == ' ')
                    {
                        returnstring += ' ';
                        afterfirst = false;
                    }
                    else
                    {
                        returnstring += "x";
                    }
                }

                return returnstring;
            }
        }

        public string DateOfBirth { get; set; }
        public bool IsMale { get; set; } = true;
        public string PhoneNumber { get; set; } = "";
        public bool IsInpatient { get; set; } = false;
        public string IsInpatientString
        {
            get
            {
                if (IsInpatient)
                {
                    return "IP";
                }
                else
                {
                    return "OP";
                }
            }
        }
        public string Referral { get; set; } = "";
        public string UniqueExamIdentifier { get; set; } = "";

        private string _ExamCode = "";

        public string ExamCode
        {
            get
            {
                return _ExamCode;
            }
            set
            {
                _ExamCode = value;
                //backgroundbrush is used in room
                OnPropertyChanged("BackgroundBrush");
            }
        }

        public string ExamName { get; set; } = "";
        public string WardNumber { get; set; } = "";

        public int Age
        {
            get
            {
                if (DateOfBirth != null && DOBconverter.BarcodeDOBtohl7DOB(DateOfBirth).Length == 8)
                {
                    try
                    {
                        return (int)((DateTime.Now - DateTime.ParseExact(DOBconverter.BarcodeDOBtohl7DOB(DateOfBirth), "yyyyMMdd",
                CultureInfo.InvariantCulture)).TotalDays / 365);
                    }
                    catch (System.Exception e)
                    {
                        Console.Error.WriteLine(e);
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
        }

        public string Gender
        {
            get
            {
                if (IsMale)
                {
                    return "M";
                }
                else
                {
                    return "F";
                }
            }
        }

        //used to distinguish between NM PI PO case color
        public Brush BackgroundBrush
        {
            get
            {
                if (ExamCode.Contains("PO"))
                {
                    return Brushes.LightGoldenrodYellow;
                }
                else if (ExamCode.Contains("NM"))
                {
                    return Brushes.Aquamarine;
                }
                else if (ExamCode.Contains("PI"))
                {
                    return Brushes.LightGreen;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        public Patient()
        {

        }

        public Patient(Hl7file hl7File)
        {
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
                string rawDateOfBirth = hl7File.getSegment("PID").getString(7);
                DateOfBirth = DOBconverter.hl7DOBtoBarcodeDOB(rawDateOfBirth);
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
            XNamespace df = xElement.Name.Namespace;

            PatientID = xElement.Element(df + "patientID").Value;

            PatientSurname = xElement.Element(df + "patientSurname").Value;
            PatientLastname = xElement.Element(df + "patientLastname").Value;

            DateOfBirth = xElement.Element(df + "dateOfBirth").Value;
            IsMale = bool.Parse(xElement.Element(df + "isMale").Value);
            PhoneNumber = xElement.Element(df + "phoneNumber").Value;
            IsInpatient = bool.Parse(xElement.Element(df + "isInpatient").Value);
            Referral = xElement.Element(df + "referral").Value;
            UniqueExamIdentifier = xElement.Element(df + "uniqueExamIdentifier").Value;
            ExamCode = xElement.Element(df + "examCode").Value;
            ExamName = xElement.Element(df + "examName").Value;
            WardNumber = xElement.Element(df + "wardNumber").Value;
        }

        public XElement toXML()
        {
            XElement patient = new XElement("patient");

            XElement patientID = new XElement("patientID", PatientID);
            XElement patientLastname = new XElement("patientLastname", PatientLastname);
            XElement patientSurname = new XElement("patientSurname", PatientSurname);
            XElement dateOfBirth = new XElement("dateOfBirth", DateOfBirth);
            XElement isMale = new XElement("isMale", IsMale.ToString());
            XElement phoneNumber = new XElement("phoneNumber", IsMale.ToString());
            XElement isInpatient = new XElement("isInpatient", IsInpatient.ToString());
            XElement referral = new XElement("referral", Referral);
            XElement uniqueExamIdentifier = new XElement("uniqueExamIdentifier", UniqueExamIdentifier);
            XElement examCode = new XElement("examCode", ExamCode);
            XElement examName = new XElement("examName", ExamName);
            XElement wardNumber = new XElement("wardNumber", WardNumber);

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
            patient.Add(wardNumber);

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
