using GMT_2025.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace GMT_2025.Controls
{
    public class LeftSideControl : ContentControl
    {
        public static readonly DependencyProperty ControlCommandsProperty =
            DependencyProperty.Register(nameof(ControlCommands), typeof(IDictionary<string, ControlCommand>), typeof(LeftSideControl),
                new PropertyMetadata(null, OnItemsSourceChanged));
        public IDictionary<string, ControlCommand> ControlCommands
        {
            get => (IDictionary<string, ControlCommand>)GetValue(ControlCommandsProperty);
            set => SetValue(ControlCommandsProperty, value);
        }
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LeftSideControl grid)
                grid.BuildGrid();
        }
        private void BuildGrid()
        {
            var uniformGrid = new UniformGrid
            {
                Name = "UniformGridLeftButton",
                Margin = new Thickness(0),
                //Rows = ViewModel.CommandsRowCount, // 綁定轉為設定值
                Columns = 1,
                Width = 80,
                Background = Brushes.Transparent
            };

            // 第一個 Border（Device Address 區塊）
            var deviceAddressBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(1),
                Margin = new Thickness(1)
            };

            var deviceAddressDock = new DockPanel { Margin = new Thickness(0) };

            // Title
            deviceAddressDock.Children.Add(new TextBlock
            {
                Text = "Device Address",
                Foreground = Brushes.Blue,
                FontWeight = FontWeights.ExtraBold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0)
            });
            //DockPanel.SetDock(deviceAddressDock.Children[^1], Dock.Top);

            // ListBox
            var listBox = new ListBox
            {
                BorderThickness = new Thickness(0),
                Width = double.NaN, // "Auto"
                Margin = new Thickness(0),
                SelectedIndex = 0,
                //ItemsSource = ViewModel.Product.RegisterTable.DeviceAddress
            };

            // DataTemplate for ListBox Items
            var dataTemplate = new DataTemplate();
            var textFactory = new FrameworkElementFactory(typeof(TextBlock));
            textFactory.SetBinding(TextBlock.TextProperty, new Binding
            {
                StringFormat = "0x{0:X2}"
            });
            dataTemplate.VisualTree = textFactory;
            listBox.ItemTemplate = dataTemplate;

            deviceAddressDock.Children.Add(listBox);
            deviceAddressBorder.Child = deviceAddressDock;
            uniformGrid.Children.Add(deviceAddressBorder);

            // 第二個 Border（CheckSum 區塊）
            var checksumBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(1),
                Margin = new Thickness(1)
            };

            var checksumDock = new DockPanel
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0)
            };

            checksumDock.Children.Add(new TextBlock
            {
                Text = "CheckSum",
                Foreground = Brushes.Blue,
                FontWeight = FontWeights.ExtraBold,
                Margin = new Thickness(0)
            });
            //DockPanel.SetDock(checksumDock.Children[^1], Dock.Top);

            var viewbox = new Viewbox { Stretch = Stretch.Uniform, Margin = new Thickness(0) };
            var checksumText = new TextBlock
            {
                TextAlignment = TextAlignment.Center
            };
            checksumText.SetBinding(TextBlock.TextProperty, new Binding("CheckSum")
            {
                StringFormat = "{0:X6}"
            });
            viewbox.Child = checksumText;

            checksumDock.Children.Add(viewbox);
            checksumBorder.Child = checksumDock;
            uniformGrid.Children.Add(checksumBorder);

            // 加入到容器中（假設有個容器例如MainPanel是你想加的地方）
            //MainPanel.Children.Add(uniformGrid); // 假設 MainPanel 是 StackPanel/DockPanel 等


        }
    }
}
