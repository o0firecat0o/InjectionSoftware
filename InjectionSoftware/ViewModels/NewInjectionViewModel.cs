using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private readonly Window window;

        public NewInjectionViewModel(Window window)
        {
            this.window = window;
            Cancel = new Command(closeWindow);
            Confirm = new Command(confirm);
            DateTime = DateTime.Now;
            Console.Out.WriteLine(DateTime);
        }

        private void closeWindow()
        {
            window.Close();
        }

        private void confirm()
        {
            if(patientID!=null && patientID != "")
            {
                InjectionsManager.addInjection(patientID, patientSurname, patientLastname);
            }
            else
            {
                //TODO: handle exception
                throw new System.Exception("Patient ID is null");
            }
            Console.Out.WriteLine("adding injection with patient ID: " + patientID);
            window.Close();
        }
    }
}
