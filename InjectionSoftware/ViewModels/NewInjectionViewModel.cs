using InjectionSoftware.Class;
using InjectionSoftware.Enums;
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

        public DateTime DateTime {get; set;}

        public ListView RPListView { get; set; }

        public Doctor SelectedDoctor { get; set; }

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

        private readonly Window window;

        public NewInjectionViewModel(Window window)
        {
            this.window = window;
            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            DateTime = DateTime.Now;
        }

        private void closeWindow()
        {
            window.Close();
        }

        //TODO: add rps

        private void confirm()
        {
            ObservableCollection<RP> RPs = new ObservableCollection<RP>();
            foreach (RP rP in RPListView.SelectedItems)
            {
                RPs.Add(rP);
            }
            
            
            if(patientID!=null && patientID != "")
            {
                InjectionsManager.addInjection(patientID, patientSurname, patientLastname, RPs, SelectedDoctor);
                Console.Out.WriteLine("adding injection with patient ID: " + patientID);
            }
            else
            {
                //TODO: handle exception
                throw new System.Exception("Patient ID is null");
            }
            window.Close();
        }
    }
}
