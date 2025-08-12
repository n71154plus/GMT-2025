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

        [ObservableProperty]
        private ObservableCollection<MenuItemViewModel> menuItems = new();

        private readonly IProductService _productService;
        private readonly Func<Product, ProductWindow> _productWindowFactory;

        public ProductsViewModel(IProductService productService, Func<Product, ProductWindow> productWindowFactory)
        {
            _productService = productService;
            _productWindowFactory = productWindowFactory;
            _ = LoadProductsAsync().ContinueWith(t =>
            {
                // 錯誤處理
            }, TaskContinuationOptions.OnlyOnFaulted);
            InitializeMenuItems();
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



        private void InitializeMenuItems()
        {
            MenuItems.Clear();
            MenuItems.Add(new MenuItemViewModel
            {
                Header = "_File",
                Children =
            {
                new MenuItemViewModel { Header = "_New" },
                new MenuItemViewModel { Header = "_Open" },
                new MenuItemViewModel { Header = "_Exit", Command = new RelayCommand(Exit) }
            }
            });
            MenuItems.Add(new MenuItemViewModel
            {
                Header = "_Edit",
                Children =
            {
                new MenuItemViewModel { Header = "_Copy" },
                new MenuItemViewModel { Header = "_Paste" }
            }
            });
        }


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
    }
}