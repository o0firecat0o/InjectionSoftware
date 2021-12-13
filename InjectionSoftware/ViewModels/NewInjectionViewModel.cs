using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using InjectionSoftware.Enums;
using InjectionSoftware.Pages;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InjectionSoftware.ViewModels
{
    public class NewInjectionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// if Injection is defined, the case is being modified instead of a new one
        /// </summary>
        public Injection Injection { get; set; }

        private Patient _SelectedPatient;
        public Patient SelectedPatient
        {
            get
            {
                return _SelectedPatient;
            }
            set
            {
                _SelectedPatient = value;
                OnPropertyChanged("SelectedPatient");
            }
        }

        #region The following are the patient information which will be modified and updated        
        private string _patientID;
        public string patientID
        {
            get
            {
                return _patientID;
            }
            set
            {
                _patientID = value;
                OnPropertyChanged("patientID");
            }
        }

        private string _patientSurname;
        public string patientSurname { get { return _patientSurname; } set { _patientSurname = value; OnPropertyChanged("patientSurname"); } }

        private string _patientLastname;
        public string patientLastname { get { return _patientLastname; } set { _patientLastname = value; OnPropertyChanged("patientLastname"); } }

        private string _UniqueExamIdentifier;
        public string UniqueExamIdentifier { get { return _UniqueExamIdentifier; } set { _UniqueExamIdentifier = value; OnPropertyChanged("UniqueExamIdentifier"); } }

        private string _ExamCode;
        public string ExamCode { get { return _ExamCode; } set { _ExamCode = value; OnPropertyChanged("ExamCode"); } }

        private string _DateOfBirth;
        public string DateOfBirth { get { return _DateOfBirth; } set { _DateOfBirth = value; OnPropertyChanged("DateOfBirth"); } }

        private int _GenderIndex;
        public int GenderIndex { get { return _GenderIndex; } set { _GenderIndex = value; OnPropertyChanged("GenderIndex"); } }

        private int _InpatientIndex;
        public int InpatientIndex { get { return _InpatientIndex; } set { _InpatientIndex = value; OnPropertyChanged("InpatientIndex"); } }

        private string _WardNumber;
        public string WardNumber { get { return _WardNumber; } set { _WardNumber = value; OnPropertyChanged("WardNumber"); } }
        #endregion

        /// <summary>
        /// The injection time of the RP, adjustable by Mahapp time picker
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                OnPropertyChanged("DateTime");
            }
        }
        private DateTime _DateTime;

        private Modality _SelectedModality;
        public Modality SelectedModality
        {
            get
            {
                return _SelectedModality;
            }
            set
            {
                _SelectedModality = value;
                OnPropertyChanged("SelectedModality");
            }
        }

        /// <summary>
        /// The selected radiologist who will dictate the case
        /// </summary>
        private Doctor _SelectedDoctor;
        public Doctor SelectedDoctor
        {
            get
            {
                return _SelectedDoctor;
            }
            set
            {
                _SelectedDoctor = value;
                OnPropertyChanged("SelectedDoctor");
            }
        }

        private int _UptakeTimeIndex;
        public int UptakeTimeIndex { get { return _UptakeTimeIndex; } set { _UptakeTimeIndex = value; OnPropertyChanged("UptakeTimeIndex"); } }

        private Room _SelectedRoom;
        public Room SelectedRoom
        {
            get { return _SelectedRoom; }
            set
            {
                _SelectedRoom = value;
                OnPropertyChanged("SelectedRoom");
            }
        }

        private bool _isContrast;
        public bool isContrast { get { return _isContrast; } set { _isContrast = value; OnPropertyChanged("isContrast"); } }

        private bool _isDelay;
        public bool isDelay { get { return _isDelay; } set { _isDelay = value; OnPropertyChanged("isDelay"); } }

        private string _patientStatus;
        public string patientStatus { get { return _patientStatus; } set { _patientStatus = value; OnPropertyChanged("patientStatus"); } }

        public ObservableCollection<Patient> ALLPatient
        {
            get
            {
                return PatientManager.Patients;
            }
        }

        /// <summary>
        /// All the registered RP
        /// </summary>
        public ObservableCollection<RP> ALLRP
        {
            get
            {
                return RP.RPs;
            }
        }

        public ObservableCollection<Doctor> ALLDoctor
        {
            get
            {
                return Doctor.Doctors;
            }
        }

        public ObservableCollection<Room> AllRoom
        {
            get
            {
                return Room.Rooms;
            }
        }

        public ObservableCollection<Modality> ALLModality
        {
            get
            {
                return Modality.Modalities;
            }
        }

        /// <summary>
        /// for better user input, automatically change uptake hour if the user select RP
        /// </summary>
        private float hasUptaketimeChanged = 2;

        public Command ClearPatient { get; set; }
        public Command Cancel { get; set; }
        public Command Confirm { get; set; }
        public Command Delete { get; set; }
        public Command Discharge { get; set; }
        public Command Readmit { get; set; }

        SelectionDialog deleteConfirmDialog = new SelectionDialog();
        SelectionDialog dischargeConfirmDialog = new SelectionDialog();
        SelectionDialog readmitConfirmDialog = new SelectionDialog();
        SelectionDialog duplicatedRoomConfirmDialog = new SelectionDialog();

        public NewInjectionViewModel(Injection Injection = null)
        {
            this.Injection = Injection;

            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            Delete = new Command(delete);
            Discharge = new Command(discharge);
            Readmit = new Command(readmit);
            ClearPatient = new Command(clearPatient);
            DateTime = DateTime.Now;

            NewInjection.window.Closed += Window_Closed;
            ((NewInjection)NewInjection.window).RP_injection.SelectionChanged += reselectUptakeTime;
            ((NewInjection)NewInjection.window).PatientSelection.SelectionChanged += patientSelectionChanged;

            if (Injection != null)
            {
                Console.Out.WriteLine("loading previous injection with patientID: " + Injection.Patient.PatientID);
                // make it impossible to change the patient ID if loading from previous injection
                ((NewInjection)NewInjection.window).patientIDTextBox.IsReadOnly = true;
                ((NewInjection)NewInjection.window).patientIDTextBox.Background = Brushes.LightGray;

                // copy all the injection information to this VM.
                if (PatientManager.HasPatient(Injection.Patient.PatientID))
                {
                    SelectedPatient = PatientManager.GetPatient(Injection.Patient.PatientID);
                    ((NewInjection)NewInjection.window).PatientSelection.SelectedItem = SelectedPatient;
                }
                patientID = Injection.Patient.PatientID;
                patientSurname = Injection.Patient.PatientSurname;
                patientLastname = Injection.Patient.PatientLastname;
                UniqueExamIdentifier = Injection.Patient.UniqueExamIdentifier;
                ExamCode = Injection.Patient.ExamCode;
                DateOfBirth = Injection.Patient.DateOfBirth;
                GenderIndex = Injection.Patient.IsMale == true ? 0 : 1;
                InpatientIndex = Injection.Patient.IsInpatient == true ? 0 : 1;
                WardNumber = Injection.Patient.WardNumber;
                DateTime = Injection.InjectionTime;
                switch ((int)Injection.UptakeTime)
                {
                    case 60:
                        UptakeTimeIndex = 0;
                        break;
                    case 90:
                        UptakeTimeIndex = 1;
                        break;
                    default:
                        UptakeTimeIndex = 0;
                        break;
                }
                SelectedRoom = Injection.SelectedRoom;
                isContrast = Injection.isContrast;
                isDelay = Injection.isDelay;
                patientStatus = Injection.patientStatus;
            }
            else
            {
                UptakeTimeIndex = 0;
                GenderIndex = 0;
                InpatientIndex = 1;
                patientStatus = "Registered";
            }

            reselectModality();
            reselectRPs();
            reselectRadiologist();
            reselectRoom();
        }

        public void selectPatient(Patient patient)
        {
            SelectedPatient = patient;
            ((NewInjection)NewInjection.window).PatientSelection.SelectedItem = SelectedPatient;
        }

        private void reselectModality()
        {
            if (Injection != null)
            {
                SelectedModality = Injection.Modality;
            }
            else
            {
                SelectedModality = Modality.Modalities[0];
            }
        }

        private void reselectRPs()
        {
            if (Injection != null)
            {
                ((NewInjection)NewInjection.window).RP_injection.SelectedItems.Clear();
                foreach (RP rP in Injection.RPs)
                {
                    ((NewInjection)NewInjection.window).RP_injection.SelectedItems.Add(rP);
                }
            }
            else
            {
                ((NewInjection)NewInjection.window).RP_injection.SelectedIndex = 0;
            }
        }

        private void reselectRadiologist()
        {
            if (Injection != null)
            {
                SelectedDoctor = Injection.Doctor;
            }
            else
            {
                SelectedDoctor = Doctor.Doctors[0];
            }
        }

        private void reselectRoom()
        {
            if (Injection != null)
            {
                SelectedRoom = Injection.SelectedRoom;
            }
            else
            {
                SelectedRoom = Room.Rooms[0];
            }
        }

        public void reselectUptakeTime(object sender, SelectionChangedEventArgs args)
        {
            if (hasUptaketimeChanged > 0)
            {
                if (Injection == null && ((NewInjection)NewInjection.window).RP_injection.SelectedItems.Count >= 2)
                {
                    ((NewInjection)NewInjection.window).RP_injection.SelectedItems.RemoveAt(0);
                    float UptakeTime = ((RP)((NewInjection)NewInjection.window).RP_injection.SelectedItems[0]).UptakeTime;
                    if (UptakeTime == 60f)
                    {
                        UptakeTimeIndex = 0;
                    }
                    else
                    {
                        UptakeTimeIndex = 1;
                    }
                }
                hasUptaketimeChanged -= 1;
            }
        }

        public void patientSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            Patient patient = (Patient)(((NewInjection)NewInjection.window).PatientSelection.SelectedItem);
            if (patient == null)
            {
                return;
            }
            patientID = patient.PatientID;
            patientLastname = patient.PatientLastname;
            patientSurname = patient.PatientSurname;
            UniqueExamIdentifier = patient.UniqueExamIdentifier;
            ExamCode = patient.ExamCode;
            DateOfBirth = patient.DateOfBirth;
            GenderIndex = patient.IsMale == true ? 0 : 1;
            InpatientIndex = patient.IsInpatient == true ? 0 : 1;

            ((NewInjection)NewInjection.window).patientIDTextBox.IsReadOnly = true;
            ((NewInjection)NewInjection.window).patientIDTextBox.Background = Brushes.LightGray;
            //optional information
            SelectedPatient = patient;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NewInjection.window.Closed -= Window_Closed;
            dispose();
        }

        private void clearPatient()
        {
            SelectedPatient = null;
            patientID = null;
            patientLastname = null;
            patientSurname = null;
            UniqueExamIdentifier = "";
            ExamCode = "";
            DateOfBirth = "";
            GenderIndex = 0;
            InpatientIndex = 0;
            ((NewInjection)NewInjection.window).PatientSelection.SelectedIndex = -1;
            ((NewInjection)NewInjection.window).patientIDTextBox.IsReadOnly = false;
            ((NewInjection)NewInjection.window).patientIDTextBox.Background = Brushes.White;
        }

        private void dispose()
        {
            Console.Out.WriteLine("[NewInjectionViewModel] Closing New Injection Window now, begin to unregister events");
            //release all the resources            
            deleteConfirmDialog.Cancel.Click -= deleteDialog_OnCloseDown;
            deleteConfirmDialog.Confirm.Click -= deleteDialog_OnDeleteDown;
            dischargeConfirmDialog.Cancel.Click -= dischargeDialog_OnCloseDown;
            dischargeConfirmDialog.Confirm.Click -= dischargeDialog_OnConfirmDown;
            readmitConfirmDialog.Cancel.Click -= readmitDialog_OnCloseDown;
            readmitConfirmDialog.Confirm.Click -= readmitDialog_OnConfirmDown;
            duplicatedRoomConfirmDialog.Cancel.Click -= duplicatedRoomConfirmDialog_OnCloseDown;
            duplicatedRoomConfirmDialog.Confirm.Click -= duplicatedRoomConfirmDialog_OnConfirmDown;

            ((NewInjection)NewInjection.window).RP_injection.SelectionChanged -= reselectUptakeTime;
            ((NewInjection)NewInjection.window).PatientSelection.SelectionChanged -= patientSelectionChanged;

            deleteConfirmDialog = null;
            dischargeConfirmDialog = null;
            readmitConfirmDialog = null;
            duplicatedRoomConfirmDialog = null;

            Cancel = null;
            Confirm = null;
            Delete = null;
            Discharge = null; 
            Readmit = null;
            ClearPatient = null;


            Injection = null;
            SelectedPatient = null;
            NewInjection.window = null;
        }

        private void closeWindow()
        {
            NewInjection.window.Close();
        }

        private async void confirm()
        {
            //check whether the user has inputted the patientID
            if (patientID == null || patientID == "")
            {
                await NewInjection.window.ShowMessageAsync("Error", "Please enter Patient ID");
                return;
            }


            //check whether the room has existing patient
            if (SelectedRoom.getNumberOfPatient() >= 1 && SelectedRoom.MultiplePatientAllowed == false)
            {
                if (!SelectedRoom.hasPatient(patientID))
                {
                    duplicatedRoomConfirmDialog.MessageText.Content = "There is multiple patient in the selected room,\n Are you sure to proceed?";
                    duplicatedRoomConfirmDialog.Cancel.Content = "Return";
                    duplicatedRoomConfirmDialog.Confirm.Content = "Confirm & Add";
                    duplicatedRoomConfirmDialog.Confirm.FontSize = 8;
                    duplicatedRoomConfirmDialog.Cancel.Click += duplicatedRoomConfirmDialog_OnCloseDown;
                    duplicatedRoomConfirmDialog.Confirm.Click += duplicatedRoomConfirmDialog_OnConfirmDown;
                    await NewInjection.window.ShowMetroDialogAsync(duplicatedRoomConfirmDialog);
                    return;
                }
            }

            add();
        }

        public async void delete()
        {
            deleteConfirmDialog.Cancel.Click += deleteDialog_OnCloseDown;
            deleteConfirmDialog.Confirm.Click += deleteDialog_OnDeleteDown;
            await NewInjection.window.ShowMetroDialogAsync(deleteConfirmDialog);
        }

        private async void deleteDialog_OnCloseDown(object sender, RoutedEventArgs e)
        {
            await NewInjection.window.HideMetroDialogAsync(deleteConfirmDialog);
        }

        private async void deleteDialog_OnDeleteDown(object sender, RoutedEventArgs e)
        {
            InjectionsManager.removeInjectionFileSync(Injection.AccessionNumber);
            await NewInjection.window.HideMetroDialogAsync(deleteConfirmDialog);
            if (NewInjection.window != null)
            {
                NewInjection.window.Close();
            }
        }

        private async void discharge()
        {
            dischargeConfirmDialog.MessageText.Content = "Are you sure you want to discharge the case?";
            dischargeConfirmDialog.Confirm.Content = "discharge";
            dischargeConfirmDialog.Cancel.Click += dischargeDialog_OnCloseDown;
            dischargeConfirmDialog.Confirm.Click += dischargeDialog_OnConfirmDown;
            await NewInjection.window.ShowMetroDialogAsync(dischargeConfirmDialog);
        }

        private async void dischargeDialog_OnCloseDown(object sender, RoutedEventArgs e)
        {
            await NewInjection.window.HideMetroDialogAsync(dischargeConfirmDialog);
        }

        private async void dischargeDialog_OnConfirmDown(object sender, RoutedEventArgs e)
        {
            InjectionsManager.changePatientStatusFileSync(Injection.AccessionNumber, "Discharged");
            await NewInjection.window.HideMetroDialogAsync(dischargeConfirmDialog);
            NewInjection.window.Close();
        }

        private async void readmit() {
            readmitConfirmDialog.MessageText.Content = "Are you sure you want to re-admit the case?";
            readmitConfirmDialog.Confirm.Content = "Re-Admit";
            readmitConfirmDialog.Confirm.Background = Brushes.Yellow;
            readmitConfirmDialog.Confirm.Foreground = Brushes.Black;
            readmitConfirmDialog.Cancel.Click += readmitDialog_OnCloseDown;
            readmitConfirmDialog.Confirm.Click += readmitDialog_OnConfirmDown;
            await NewInjection.window.ShowMetroDialogAsync(readmitConfirmDialog);
        }

        private async void readmitDialog_OnCloseDown(object sender, RoutedEventArgs e)
        {
            await NewInjection.window.HideMetroDialogAsync(readmitConfirmDialog);
        }

        private async void readmitDialog_OnConfirmDown(object sender, RoutedEventArgs e)
        {
            InjectionsManager.changePatientStatusFileSync(Injection.AccessionNumber, "Registered");
            await NewInjection.window.HideMetroDialogAsync(readmitConfirmDialog);
            NewInjection.window.Close();
        }


        private async void duplicatedRoomConfirmDialog_OnCloseDown(object sender, RoutedEventArgs e)
        {
            await NewInjection.window.HideMetroDialogAsync(duplicatedRoomConfirmDialog);
        }

        private async void duplicatedRoomConfirmDialog_OnConfirmDown(object sender, RoutedEventArgs e)
        {
            await NewInjection.window.HideMetroDialogAsync(duplicatedRoomConfirmDialog);
            add();
        }

        public void add()
        {
            //Clone the observable collection into another collection
            ObservableCollection<RP> RPs = new ObservableCollection<RP>();
            foreach (RP rP in ((NewInjection)NewInjection.window).RP_injection.SelectedItems)
            {
                RPs.Add(rP);
            }

            //Convert the uptakeindex into float;
            float UptakeTime;
            switch (UptakeTimeIndex)
            {
                case 0:
                    UptakeTime = 60f;
                    break;
                case 1:
                    UptakeTime = 90f;
                    break;
                default:
                    UptakeTime = 60f;
                    break;
            }

            //add new injection
            if (Injection == null)
            {
                InjectionsManager.modInjectionFileSync ("", SelectedModality, patientID, patientSurname, patientLastname, UniqueExamIdentifier, ExamCode, DateOfBirth, GenderIndex == 0, InpatientIndex == 0, WardNumber, RPs, SelectedDoctor, UptakeTime, DateTime, SelectedRoom, isContrast, isDelay, patientStatus);
                Console.Out.WriteLine("adding injection with patient ID: " + patientID);
            }
            //modify existing injection
            else
            {
                InjectionsManager.modInjectionFileSync(Injection.AccessionNumber, SelectedModality, patientID, patientSurname, patientLastname, UniqueExamIdentifier, ExamCode, DateOfBirth, GenderIndex == 0, InpatientIndex == 0, WardNumber, RPs, SelectedDoctor, UptakeTime, DateTime, SelectedRoom, isContrast, isDelay, patientStatus);
                Console.Out.WriteLine("modifying injection with patient ID:" + patientID);
            }

            NewInjection.window.Close();
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
