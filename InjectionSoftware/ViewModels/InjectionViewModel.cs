using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InjectionSoftware.ViewModels
{
    internal class InjectionViewModel
    {
        private Injection injection;

        public Command command1 { get; set; }

        public Injection Injection
        {
            get
            {
                return injection;
            }
        }

        public InjectionViewModel()
        {
            injection = new Injection();
            injection.AccessionNumber = "aghfello";

            command1 = new Command(ExecuteCommand1);
        }

        private void ExecuteCommand1()
        {
            MessageBox.Show("FUCK YOU");
        }
    }
}
