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
        public Command Cancel { get; set; }

        public DateTime DateTime {get; set;}

        private readonly Window window;

        public NewInjectionViewModel(Window window)
        {
            this.window = window;
            Cancel = new Command(closeWindow);
            DateTime = DateTime.Now;
            Console.Out.WriteLine(DateTime);
        }

        private void closeWindow()
        {
            Console.Out.WriteLine(DateTime);
        }
    }
}
