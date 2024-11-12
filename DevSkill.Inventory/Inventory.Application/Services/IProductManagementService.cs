using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Inventory.Domain.Dtos;

namespace Inventory.Application.Services
{
    public interface IProductManagementService
    {
        void AddStockListEntriesForProduct(Product product);

       
        void CreateProduct(Product Products);
        void DeleteProduct(Guid id);
        Task DeleteProductsAsync(List<Guid> ids);
        Product GetProduct(Guid id);
        (IList<Product> data, int total, int totaldisplay) GetProducts(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<(IList<ProductDto> data, int total, int totaldisplay)> GetProductsSP(int pageIndex, int pageSize, ProductSearchDto search, string? order);


        string SetImagePath(IFormFile fileName);
        void UpdateProduct(Product product);

        Task<int> GetProductCountAsync();
    }
}
