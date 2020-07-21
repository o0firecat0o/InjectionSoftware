using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InjectionSoftware.ViewModels
{
    internal class InjectionViewModel
    {
        
        public ObservableCollection<Injection> Injections {
            get
            {
                return InjectionsManager.injections;
            }
        }

        public Command<Injection> Command1 { get; set; }

        public InjectionViewModel()
        {
            Command1 = new Command<Injection>(ExecuteCommand1);

            InjectionsManager.addInjection("A123456", "Hello", "World");

        }

        private void ExecuteCommand1(Injection injection)
        {
            MessageBox.Show("FUCK YOU");
            Console.Out.WriteLine("fuck you" + injection.AccessionNumber);
        }
    }
}
