using InjectionSoftware.Class;
using InjectionSoftware.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace InjectionSoftware.ViewModels
{
    internal class InjectionPageViewModel
    {

        public CompositeCollection CompositeCollection
        {
            get;
        } = new CompositeCollection();


        public Command<Injection> Command1 { get; set; }

        public Command Command2 { get; set; }

        private string _SearchString;
        public string SearchString
        {
            get { return _SearchString; }
            set { _SearchString = value; Search(); }
        }

        public InjectionPageViewModel()
        {
            Command1 = new Command<Injection>(ExecuteCommand1);
            Command2 = new Command(ExecuteCommand2);


            CollectionContainer injectionsCollection = new CollectionContainer() { Collection = InjectionsManager.injections };
            CompositeCollection.Add(injectionsCollection);
            CompositeCollection.Add(new AddNewButton());
            CompositeCollection.Add(new Legend());
        }

        private void ExecuteCommand1(Injection injection)
        {
            Window newInjectionWindow = new NewInjection(injection);
            newInjectionWindow.ShowDialog();
        }

        private void ExecuteCommand2()
        {
            Window newInjectionWindow = new NewInjection();
            newInjectionWindow.ShowDialog();
        }

        //used for highlighting the injection result after searching
        private void Search()
        {
            //TODO: maybe use update trigger instead of enter to search?
            //caused more CPU power, but better user enjoyment
            foreach (Injection injection in InjectionsManager.injections)
            {
                injection.isVisible = false;
                if (injection.SearchString.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    injection.isVisible = true;
                }
            }
        }


    }
}
