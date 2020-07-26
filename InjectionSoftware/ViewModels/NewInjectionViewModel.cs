﻿using InjectionSoftware.Class;
using InjectionSoftware.Enums;
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

        public string patientID { get; set; }

        public string patientSurname { get; set; }

        public string patientLastname { get; set; }
        
        public Command Cancel { get; set; }

        public Command Confirm { get; set; }

        /// <summary>
        /// The injection time of the RP, adjustable by Mahapp time picker
        /// </summary>
        public DateTime DateTime {get; set;}

        /// <summary>
        /// The control for ListView containing all the RPs
        /// </summary>
        public ListView RPListView { get; set; }

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

        public int UptakeTimeIndex { get; set; }

        public Room SelectedRoom { get; set; }



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

        private readonly MetroWindow window;

        

        public NewInjectionViewModel(MetroWindow window, Injection Injection = null)
        {
            this.window = window;
            this.Injection = Injection;

            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            DateTime = DateTime.Now;

            if (Injection != null)
            {
                Console.Out.WriteLine("loading previous injection with patientID: " + Injection.Patient.PatientID);
                patientID = Injection.Patient.PatientID;
                patientSurname = Injection.Patient.PatientSurname;
                patientLastname = Injection.Patient.PatientLastname;
                DateTime = Injection.InjectionTime;
                switch (Injection.UptakeTime)
                {
                    case 60f:
                        UptakeTimeIndex = 0;
                        break;
                    case 90f:
                        UptakeTimeIndex = 1;
                        break;
                    default:
                        UptakeTimeIndex = 0;
                        break;
                }
                SelectedRoom = Injection.SelectedRoom;
            }
            else
            {
                UptakeTimeIndex = 0;
            }

            
        }

        public void reselectRPs()
        {
            if(Injection != null)
            {
                RPListView.SelectedItems.Clear();
                foreach (RP rP in Injection.RPs)
                {
                    RPListView.SelectedItems.Add(rP);
                }
            }            
        }

        public void reselectRadiologist()
        {
            if(Injection != null)
            {
                SelectedDoctor = Injection.Doctor;
            }
        }

        private void closeWindow()
        {
            window.Close();
        }

        //TODO: add rps

        private async void confirm()
        {
            ObservableCollection<RP> RPs = new ObservableCollection<RP>();
            foreach (RP rP in RPListView.SelectedItems)
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
                if(Injection == null)
                {
                    InjectionsManager.addInjection(patientID, patientSurname, patientLastname, RPs, SelectedDoctor, UptakeTime, DateTime, SelectedRoom);
                    Console.Out.WriteLine("adding injection with patient ID: " + patientID);
                }
                //modify existing injection
                else
                {
                    Injection.Patient.PatientID = patientID;
                    Injection.Patient.PatientSurname = patientSurname;
                    Injection.Patient.PatientLastname = patientLastname;
                    Injection.RPs = RPs;
                    Injection.Doctor = SelectedDoctor;
                    Injection.UptakeTime = UptakeTime;
                    Injection.InjectionTime = DateTime;
                    Injection.SelectedRoom = SelectedRoom;
                    Console.Out.WriteLine("modifying injection with patient ID:" + patientID);
                }

                
                
                window.Close();
            }
            else
            {
                await window.ShowMessageAsync("Error", "Please enter Patient ID");
            }
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
