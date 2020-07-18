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
        
        public ObservableCollection<Injection> Injections { get; set; }

        public Command<Injection> Command1 { get; set; }


        public InjectionViewModel()
        {
            Command1 = new Command<Injection>(ExecuteCommand1);

            Injections = new ObservableCollection<Injection>();

            Injections.Add(new Injection()
            {
                AccessionNumber = "hello",
                CaseNumber = 1
            }); ;

            Injections.Add(new Injection()
            {
                AccessionNumber = "hello2",
                CaseNumber = 2
            });

            Injections.Add(new Injection()
            {
                AccessionNumber = "hello3",
                CaseNumber = 3
            });

            Injections.Add(new Injection()
            {
                AccessionNumber = "hello4",
                CaseNumber = 4
            });
        }

        private void ExecuteCommand1(Injection injection)
        {
            MessageBox.Show("FUCK YOU");
            Console.Out.WriteLine("fuck you" + injection.AccessionNumber);
        }
    }
}
