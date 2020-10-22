﻿using InjectionSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InjectionSoftware
{
    /// <summary>
    /// Interaction logic for RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        public RoomPage()
        {
            InitializeComponent();
            RoomPageViewModel roomPageViewModel= new RoomPageViewModel();            
            DataContext = roomPageViewModel;
            roomPageViewModel.Init();
        }
    }
}
