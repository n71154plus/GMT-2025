using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace GMT_2025.Controls
{
    public partial class RoundedComboBox : UserControl
    {
        public RoundedComboBox()
        {
            InitializeComponent();
        }

        // ItemsSource 依賴屬性
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(IEnumerable), typeof(RoundedComboBox), new PropertyMetadata(null));

        public IEnumerable Items
        {
            get => (IEnumerable)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        // SelectedItem 依賴屬性
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(RoundedComboBox), new PropertyMetadata(null));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}
