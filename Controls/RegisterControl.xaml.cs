using GMT_2025.Models;
using GMT_2025.ViewModels;
using System;
using System.Collections;
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

namespace GMT_2025.Controls
{
    /// <summary>
    /// RegisterControl.xaml 的互動邏輯
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        public RegisterControl(RegisterViewModel registerViewModel )
        {
            DataContext = registerViewModel;
            InitializeComponent();
        }


        public IEnumerable ComboBoxItemsSource
        {
            get => (IEnumerable)GetValue(ComboBoxItemsSourceProperty);
            set => SetValue(ComboBoxItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ComboBoxItemsSourceProperty =
            DependencyProperty.Register(nameof(ComboBoxItemsSource), typeof(IEnumerable), typeof(RegisterControl), new PropertyMetadata(null));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(RegisterControl), new PropertyMetadata(null));
    }
}
