using CommunityToolkit.Mvvm.ComponentModel;
using GMT_2025.Models;

namespace GMT_2025.ViewModels
{
    public partial class ProductEditorViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product product;

        public ProductEditorViewModel(Product product)
        {
            Product = product;
        }
    }
}
