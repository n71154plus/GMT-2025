using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// DictionaryDataGrid.xaml 的互動邏輯
    /// </summary>
    public partial class DictionaryDataGrid : UserControl
    {
        public DictionaryDataGrid()
        {
            InitializeComponent();
        }
        // DependencyProperty 讓外部能綁定 Dictionary<int, byte>
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(Dictionary<int, byte>), typeof(DictionaryDataGrid),
                new PropertyMetadata(null, OnItemsChanged));

        public Dictionary<int, byte> Items
        {
            get => (Dictionary<int, byte>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DictionaryDataGrid)d;
            control.BuildGrid();
        }

        private void BuildGrid()
        {
            dataGrid.Columns.Clear();

            if (Items == null || Items.Count == 0)
            {
                dataGrid.ItemsSource = null;
                return;
            }
            var centerStyle = new Style(typeof(TextBlock));
            centerStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            centerStyle.Setters.Add(new Setter(VerticalAlignmentProperty, VerticalAlignment.Center));
            var headerStyle = new Style(typeof(DataGridColumnHeader));
            headerStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            headerStyle.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            dataGrid.ColumnHeaderStyle = headerStyle;
            dataGrid.FontSize = 12;
            // 建立欄位，Header 是 key
            int index = 0;
            foreach (var key in Items.Keys)
            {
                var colIndex = index++; // 用於 Binding 索引閉包

                var column = new DataGridTextColumn
                {
                    Header = key.ToString("X2"),
                    Binding = new Binding($"[{colIndex}]"), // 綁定 List<byte> 的第 colIndex 個元素
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),  // 平均分配欄寬
                    ElementStyle = centerStyle
                };
                dataGrid.Columns.Add(column);
            }

            // DataGrid ItemsSource 只要一行，存放所有的 value，型態是 List<byte>
            dataGrid.ItemsSource = new List<List<string>>
            {
                new List<string>(Items.Values.Select(b => $"{b:X2}"))
            };
        }

    }
}
