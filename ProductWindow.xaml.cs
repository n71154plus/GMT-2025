using GMT_2025.Controls;
using GMT_2025.Models;
using GMT_2025.ViewModels;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace GMT_2025
{
    /// <summary>
    /// ProductWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ProductWindow : Window
    {
        public Dictionary<string, RegisterControl> RegisterControls { get; set; } = new Dictionary<string, RegisterControl>();
        public ProductWindow(Product product)
        {
            DataContext = new ProductViewModel(product);
            InitializeComponent();
            //InitialRegisterControls(product.RegisterTable.Registers);
            AssignLeftMenuCommandButton(product.RegisterTable.Commands);
            //this.DataContextChanged += ProductWindow_DataContextChanged;
            //foreach(var s in DataContext)
        }
        
        private void AssignLeftMenuCommandButton(Dictionary<string, ControlCommand> controlCommands)
        {
            foreach (var s in controlCommands)
            {
                var button = new Button()
                {
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent,
                    Content = new TextBlock
                    {
                        Background = Brushes.Transparent,
                        FontWeight = FontWeights.Bold,
                        Text = s.Value.Name,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        ToolTip = s.Value.Description,
                        Foreground = new SolidColorBrush(Colors.Blue),
                        Padding = new Thickness(1),
                        Margin = new Thickness(1),
                    }
                };
                var borderButton = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Gray,
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(1),
                    Margin = new Thickness(1),
                    Child = button
                };
                UniformGridLeftButton.Children.Add(borderButton);
            }
        }
        private void ProductWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ProductViewModel productViewModel)
            {
                if (productViewModel.Product.RegisterTable.Commands is Dictionary<string, ControlCommand> controlCommands)
                {

                    //UniformGridLeftButton
                    
                }
                if (productViewModel.Product.RegisterTable.Registers is Dictionary<string, Register> registers)
                {

                }

            }
        }

    }
}
