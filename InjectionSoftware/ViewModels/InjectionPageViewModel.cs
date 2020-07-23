﻿using InjectionSoftware.Class;
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

namespace InjectionSoftware.ViewModels
{
    internal class InjectionPageViewModel
    {
        
        public CompositeCollection CompositeCollection
        {
            get
            {
                return _CompositeCollection;
            }
        }

        private readonly CompositeCollection _CompositeCollection = new CompositeCollection();

        public Command<Injection> Command1 { get; set; }

        public Command Command2 { get; set; }

        public InjectionPageViewModel()
        {
            Command1 = new Command<Injection>(ExecuteCommand1);
            Command2 = new Command(ExecuteCommand2);



            CollectionContainer injectionsCollection = new CollectionContainer() { Collection = InjectionsManager.injections };            
            _CompositeCollection.Add(injectionsCollection);
            _CompositeCollection.Add(new AddNewButton());
            _CompositeCollection.Add(new Legend());
        }

        private void ExecuteCommand1(Injection injection)
        {
            MessageBox.Show("FUCK YOU");
            Console.Out.WriteLine("fuck you" + injection.CaseNumber);
        }

        private void ExecuteCommand2()
        {
            Window newInjectionWindow = new NewInjection();
            newInjectionWindow.ShowDialog();
        }
    }
}