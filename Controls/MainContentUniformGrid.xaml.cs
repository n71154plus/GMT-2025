using GMT_2025.Models;
using GMT_2025.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GMT_2025.Controls
{
    public partial class MainContentUniformGrid : UserControl
    {
        public static readonly DependencyProperty GroupByProperty =
            DependencyProperty.Register(nameof(GroupBy), typeof(string), typeof(MainContentUniformGrid),
                new PropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(nameof(Columns), typeof(int?), typeof(MainContentUniformGrid),
                new PropertyMetadata(null, OnLayoutChanged));

        private Dictionary<string, RegisterControl> RegisterControls { get; } = new();

        public MainContentUniformGrid()
        {
            InitializeComponent();
            DataContextChanged += MainContentUniformGrid_DataContextChanged;
        }

        private void MainContentUniformGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ProductViewModel productViewModel)
            {
                RegisterControls.Clear();

                if (productViewModel.Product?.RegisterTable?.Registers is { } registers &&
                    productViewModel.RegisterViewModelMap is { } registerViewModels)
                {
                    foreach (var item in registers.Values)
                    {
                        var control = new RegisterControl(registerViewModels[item.Name]);
                        RegisterControls.Add(item.Name, control);
                    }
                }
            }
        }

        public int? Columns
        {
            get => (int?)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public string GroupBy
        {
            get => (string)GetValue(GroupByProperty);
            set => SetValue(GroupByProperty, value);
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainContentUniformGrid grid)
                grid.BuildGrid();
        }

        private (Border border, UniformGrid panel) CreateGroupPanel(string groupName, Color bgColor)
        {
            var groupPanel = new UniformGrid
            {
                Margin = new Thickness(0),
                Rows = 9,
                Columns = 1
            };

            var groupText = new TextBlock
            {
                Text = groupName,
                FontWeight = FontWeights.UltraBold,
                Padding = new Thickness(1),
                Margin = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.Blue,
                TextWrapping = TextWrapping.Wrap
            };

            groupPanel.Children.Add(groupText);

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(1),
                Margin = new Thickness(1),
                Background = new SolidColorBrush(bgColor),
                Child = groupPanel
            };

            return (border, groupPanel);
        }

        private void BuildGrid()
        {
            if (RegisterControls == null || string.IsNullOrEmpty(GroupBy))
                return;

            var layout = new UniformGrid
            {
                Columns = Columns ?? 1
            };

            var rand = new Random();
            var usedColors = new HashSet<Color>();
            var groupedRegisters = RegisterGroupBy(RegisterControls, GroupBy);

            foreach (var group in groupedRegisters)
            {
                int maxRows = Math.Min(9, group.Value.Count + 1);
                int count = 0;

                Border currentBorder = null;
                UniformGrid currentPanel = null;

                void StartNewPanel()
                {
                    var color = GenerateDistinctColor(rand, usedColors);
                    (currentBorder, currentPanel) = CreateGroupPanel(group.Key, color);
                    currentPanel.Rows = maxRows;
                    count = 1;
                }

                StartNewPanel();

                foreach (var register in group.Value)
                {
                    if (!RegisterControls.TryGetValue(register.Name, out var control))
                        continue;

                    if (count >= maxRows)
                    {
                        layout.Children.Add(currentBorder);
                        StartNewPanel();
                    }

                    currentPanel.Children.Add(control);
                    count++;
                }

                if (currentBorder != null)
                    layout.Children.Add(currentBorder);
            }

            RootGrid.Children.Clear();
            RootGrid.Children.Add(layout);
        }

        private static Dictionary<string, List<RegisterViewModel>> RegisterGroupBy(IDictionary<string, RegisterControl> registers, string groupPropertyName)
        {
            return registers
                .Values
                .Where(r => r != null && r.DataContext is RegisterViewModel)
                .Select(r => r.DataContext as RegisterViewModel)
                .GroupBy(r =>
                {
                    var prop = typeof(RegisterViewModel).GetProperty(groupPropertyName);
                    return prop?.GetValue(r) as string ?? "";
                })
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private static Color GenerateDistinctColor(Random rand, HashSet<Color> usedColors)
        {
            Color color;
            do
            {
                color = Color.FromRgb(
                    (byte)rand.Next(192, 255),
                    (byte)rand.Next(192, 255),
                    (byte)rand.Next(192, 255));
            } while (!usedColors.Add(color));
            return color;
        }
    }
}
