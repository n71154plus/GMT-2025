using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMT_2025.Models;
using GMT_2025.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
namespace GMT_2025.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> productItems = new();

        private readonly IProductService _productService;
        private readonly Func<Product, ProductWindow> _productWindowFactory;
        private readonly Func<Product, ProductEditorWindow> _productEditorWindowFactory;

        public ProductsViewModel(IProductService productService,
            Func<Product, ProductWindow> productWindowFactory,
            Func<Product, ProductEditorWindow> productEditorWindowFactory)
        {
            _productService = productService;
            _productWindowFactory = productWindowFactory;
            _productEditorWindowFactory = productEditorWindowFactory;
            _ = LoadProductsAsync().ContinueWith(t =>
            {
                // 錯誤處理
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private async Task LoadProductsAsync()
        {
            var products = await _productService.LoadProductsAsync();
            ProductItems.Clear();
            foreach (var product in products)
            {
                ProductItems.Add(product);
            }
        }
        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void OpenProductWindow(Product product)
        {
            var productWindow = _productWindowFactory(product);
            productWindow.Show();
        }

        [RelayCommand]
        private void OpenProductEditor()
        {
            var editor = _productEditorWindowFactory(new Product());
            editor.Show();
        }
    }
}