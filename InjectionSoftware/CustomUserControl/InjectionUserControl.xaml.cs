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

        public Injection Injection
        {
            get { return (Injection)GetValue(InjectionProperty); }
            set { SetValue(InjectionProperty, value); }
        }

        public static readonly DependencyProperty InjectionProperty =
          DependencyProperty.Register("Injection", typeof(Injection), typeof(InjectionUserControl), new FrameworkPropertyMetadata()
          {
              PropertyChangedCallback = OnInjectionChanged,
              BindsTwoWayByDefault = true
          }); 

        private static void OnInjectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                // code to be executed on value update
            }
        }

        
        /// <summary>
        /// Horizontal String boolean is used for determining whether the display name is in horizontal or vertical direction to save space
        /// </summary>
        public bool IsHorizontalString
        {
            get { return (bool)GetValue(IsHorizontalStringProperty); }
            set { SetValue(IsHorizontalStringProperty, value); }
        }

        public static readonly DependencyProperty IsHorizontalStringProperty =
          DependencyProperty.Register("StringOrientation", typeof(bool), typeof(InjectionUserControl), new FrameworkPropertyMetadata()
          {
              PropertyChangedCallback = OnIsHorizontalStringChanged,
              BindsTwoWayByDefault = true
          });

        private static void OnIsHorizontalStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                // code to be executed on value update
            }
        }
    }
}
