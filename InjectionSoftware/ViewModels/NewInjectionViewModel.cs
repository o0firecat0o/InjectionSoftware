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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InjectionSoftware.ViewModels
{
    public class NewInjectionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// if Injection is defined, the case is being modified instead of a new one
        /// </summary>
        public Injection Injection { get; set; }

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

        public Command Cancel { get; set; }

        public Command Confirm { get; set; }

        public Command Delete { get; set; }

        public Command Discharge { get; set; }

        /// <summary>
        /// The injection time of the RP, adjustable by Mahapp time picker
        /// </summary>
        public DateTime DateTime { get
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
        public Room SelectedRoom { get { return _SelectedRoom; } set { _SelectedRoom = value; OnPropertyChanged("SelectedRoom"); } }

        private bool _isContrast;
        public bool isContrast { get { return _isContrast; } set { _isContrast = value; OnPropertyChanged("isContrast"); } }

        private bool _isDelay;
        public bool isDelay { get { return _isDelay; } set { _isDelay = value; OnPropertyChanged("isDelay"); } }

        private bool _isDischarge;
        public bool isDischarge { get { return _isDischarge; } set { _isDischarge = value; OnPropertyChanged("isDischarge"); } }



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

        /// <summary>
        /// for better user input, automatically change uptake hour if the user select RP
        /// </summary>
        private float hasUptaketimeChanged = 2;

        public NewInjectionViewModel(Injection Injection = null)
        {
            this.Injection = Injection;

            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            Delete = new Command(delete);
            Discharge = new Command(discharge);
            DateTime = DateTime.Now;

            NewInjection.window.Closed += Window_Closed;
            ((NewInjection)NewInjection.window).RP_injection.SelectionChanged += reselectUptakeTime;

            if (Injection != null)
            {
                Console.Out.WriteLine("loading previous injection with patientID: " + Injection.Patient.PatientID);
                patientID = Injection.Patient.PatientID;
                patientSurname = Injection.Patient.PatientSurname;
                patientLastname = Injection.Patient.PatientLastname;
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
                isDischarge = Injection.isDischarge;
            }
            else
            {
                UptakeTimeIndex = 0;
            }

            reselectRPs();
            reselectRadiologist();
            reselectRoom();
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
            if (hasUptaketimeChanged>0)
            {
                if (Injection == null && ((NewInjection)NewInjection.window).RP_injection.SelectedItems.Count >= 2)
                {
                    ((NewInjection)NewInjection.window).RP_injection.SelectedItems.RemoveAt(0);
                    float UptakeTime = ((RP)((NewInjection)NewInjection.window).RP_injection.SelectedItems[0]).UptakeTime;
                    if(UptakeTime == 60f)
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

        private void Window_Closed(object sender, EventArgs e)
        {
            NewInjection.window.Closed -= Window_Closed;
            dispose();
        }

        private void dispose()
        {
            Console.Out.WriteLine("[NewInjectionViewModel] Closing New Injection Window now, begin to unregister events");
            //release all the resources            
            deleteConfirmDialog.Cancel.Click -= deleteDialog_OnCloseDown;
            deleteConfirmDialog.Confirm.Click -= deleteDialog_OnDeleteDown;
            dischargeConfirmDialog.Cancel.Click -= dischargeDialog_OnCloseDown;
            dischargeConfirmDialog.Confirm.Click -= dischargeDialog_OnConfirmDown;

            ((NewInjection)NewInjection.window).RP_injection.SelectionChanged -= reselectUptakeTime;

            Cancel = null;
            Confirm = null;
            Delete = null;
            Discharge = null;
            Injection = null;
            NewInjection.window = null;
        }

        private void closeWindow()
        {
            NewInjection.window.Close();
        }

        private async void confirm()
        {
            ObservableCollection<RP> RPs = new ObservableCollection<RP>();
            foreach (RP rP in ((NewInjection)NewInjection.window).RP_injection.SelectedItems)
            {
                RPs.Add(rP);
            }

            if (patientID != null && patientID != "")
            {
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
                    InjectionsManager.SendAndModInjection("", patientID, patientSurname, patientLastname, RPs, SelectedDoctor, UptakeTime, DateTime, SelectedRoom, isContrast, isDelay, isDischarge);
                    Console.Out.WriteLine("adding injection with patient ID: " + patientID);
                }
                //modify existing injection
                else
                {
                    InjectionsManager.SendAndModInjection(Injection.AccessionNumber, patientID, patientSurname, patientLastname, RPs, SelectedDoctor, UptakeTime, DateTime, SelectedRoom, isContrast, isDelay, isDischarge);
                    Console.Out.WriteLine("modifying injection with patient ID:" + patientID);
                }



                NewInjection.window.Close();
            }
            else
            {
                await NewInjection.window.ShowMessageAsync("Error", "Please enter Patient ID");
            }
        }

        SelectionDialog deleteConfirmDialog = new SelectionDialog();
        SelectionDialog dischargeConfirmDialog = new SelectionDialog();

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
            InjectionsManager.delInjection(Injection);
            await NewInjection.window.HideMetroDialogAsync(deleteConfirmDialog);
            NewInjection.window.Close();
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
            InjectionsManager.dischargeInjection(Injection, true);
            await NewInjection.window.HideMetroDialogAsync(dischargeConfirmDialog);
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
