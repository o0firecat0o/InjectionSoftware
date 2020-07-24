using InjectionSoftware.Class;
using InjectionSoftware.Enums;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InjectionSoftware.ViewModels
{
    public class NewInjectionViewModel
    {
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

        public Doctor SelectedDoctor { get; set; }

        public int UptakeTimeIndex { get; set; }


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

        private readonly MetroWindow window;

        public NewInjectionViewModel(MetroWindow window)
        {
            this.window = window;
            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            DateTime = DateTime.Now;
            UptakeTimeIndex = 0;
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
            
            
            if(patientID!=null && patientID != "")
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
                InjectionsManager.addInjection(patientID, patientSurname, patientLastname, RPs, SelectedDoctor, UptakeTime, DateTime);
                Console.Out.WriteLine("adding injection with patient ID: " + patientID);
                window.Close();
            }
            else
            {
                await window.ShowMessageAsync("Error", "Please enter Patient ID");
            }
        }
    }
}
