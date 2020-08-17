using InjectionSoftware.Class;
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

namespace InjectionSoftware.CustomUserControl
{
    /// <summary>
    /// Interaction logic for InjectionUserControl.xaml
    /// </summary>
    public partial class InjectionUserControl : UserControl
    {



       



        public InjectionUserControl()
        {
            InitializeComponent();

        }

        public static readonly DependencyProperty InjectionProperty =
          DependencyProperty.Register("Injection", typeof(Injection), typeof(InjectionUserControl), new FrameworkPropertyMetadata()
          {
              PropertyChangedCallback = OnInjectionChanged,
              BindsTwoWayByDefault = true
          });

        public Injection Injection
        {
            get { return (Injection)GetValue(InjectionProperty); }
            set { SetValue(InjectionProperty, value); }
        }

        private static void OnInjectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                // code to be executed on value update
            }
        }
    }
}
