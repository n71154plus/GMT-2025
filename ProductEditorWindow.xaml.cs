using System.Windows;
using GMT_2025.Models;
using GMT_2025.ViewModels;

namespace GMT_2025
{
    public partial class ProductEditorWindow : Window
    {
        public ProductEditorWindow(Product product)
        {
            InitializeComponent();
            DataContext = new ProductEditorViewModel(product);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
