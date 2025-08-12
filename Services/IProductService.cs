using System.Collections.Generic;
using System.Threading.Tasks;
using GMT_2025.Models;

namespace GMT_2025.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> LoadProductsAsync();
    }
}
