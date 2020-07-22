using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.ViewModels
{
    public class MainWindowViewModel
    {
        private Timer _Timer = new Timer();

        public Timer Timer
        {
            get
            {
                return _Timer;
            }
        }
    }
}
