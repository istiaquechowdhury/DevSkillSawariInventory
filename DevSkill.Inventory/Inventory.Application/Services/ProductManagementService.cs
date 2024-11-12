using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using Microsoft.AspNetCore.Http;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Domain.Dtos;

namespace Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;


        public ProductManagementService(IInventoryUnitOfWork InventoryUnitOfWork)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
        }



        public void CreateProduct(Product Products)
        {
            if (!_InventoryUnitOfWork.Product.IsTitleDuplicate(Products.ItemName))
            {
                _InventoryUnitOfWork.Product.Add(Products);
                _InventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("ItemName should be unique.");



        }

        public void DeleteProduct(Guid id)
        {
            _InventoryUnitOfWork.Product.Remove(id);
            _InventoryUnitOfWork.Save();
        }

        public Product GetProduct(Guid id)
        {
            return _InventoryUnitOfWork.Product.GetProduct(id);
        }

        public (IList<Product> data, int total, int totaldisplay) GetProducts(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _InventoryUnitOfWork.Product.GetPagedProducts(pageIndex, pageSize, search, order);
        }

        public string SetImagePath(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Define the path where you want to save the image
                var fileName = Path.GetFileName(file.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Ensure the upload folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the relative path to the image
                return $"/images/{fileName}";
            }
            return null; // Or handle this c
        }

        public void UpdateProduct(Product product)
        {
            if (!_InventoryUnitOfWork.Product.IsTitleDuplicate(product.ItemName, product.Id))
            {
                _InventoryUnitOfWork.Product.Edit(product);
                _InventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("ItemName should be unique.");


        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            // Ensure that the list of ids is not null or empty
            if (ids == null || !ids.Any())
            {
                return; // Exit if there are no ids to delete
            }

            // Use the Remove method with a condition that matches the ids in the provided list
            _InventoryUnitOfWork.Product.Remove(p => ids.Contains(p.Id));

            // Save changes to the database
            await _InventoryUnitOfWork.SaveAsync();
        }



        public void AddStockListEntriesForProduct(Product product)
        {
            var stocklistEntries = new List<StockList>
            {
                    new StockList
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        WarehouseId = GetWarehouseId("Dhanmondi"), // Fetch WarehouseId for Dhanmondi
                        InHand = product.Dhanmondi,
                        WeightedAvgCost = product.CostPerUnit,
                        Product = product, // Attach the product
                        Warehouse = _InventoryUnitOfWork.WareHouse.GetById(GetWarehouseId("Dhanmondi")) // Attach the warehouse
                    },
                    new StockList
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        WarehouseId = GetWarehouseId("Mirpur"),
                        InHand = product.Mirpur,
                        WeightedAvgCost = product.CostPerUnit,
                        Product = product, // Attach the product
                        Warehouse = _InventoryUnitOfWork.WareHouse.GetById(GetWarehouseId("Mirpur")) // Attach the warehouse
                    },
                    new StockList
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        WarehouseId = GetWarehouseId("Uttara"),
                        InHand = product.Uttara,
                        WeightedAvgCost = product.CostPerUnit,
                        Product = product, // Attach the product
                        Warehouse = _InventoryUnitOfWork.WareHouse.GetById(GetWarehouseId("Uttara")) // Attach the warehouse
                    },
                    new StockList
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        WarehouseId = GetWarehouseId("WIP"),
                        InHand = product.WIP,
                        WeightedAvgCost = product.CostPerUnit,
                        Product = product, // Attach the product
                        Warehouse = _InventoryUnitOfWork.WareHouse.GetById(GetWarehouseId("WIP")) // Attach the warehouse
                    }
            };

            foreach (var stocklist in stocklistEntries)
            {
                if (stocklist.InHand > 0)
                {
                    _InventoryUnitOfWork.StockList.Add(stocklist);
                }

            }

            _InventoryUnitOfWork.Save();
        }


        private Guid GetWarehouseId(string warehouseName)
        {
            // Return hardcoded WarehouseId based on the warehouse name using if-else statements
            if (warehouseName.Equals("Dhanmondi", StringComparison.OrdinalIgnoreCase))
            {
                return new Guid("8373853D-AC7F-4640-1535-08DCE895BD99");
            }
            else if (warehouseName.Equals("Mirpur", StringComparison.OrdinalIgnoreCase))
            {
                return new Guid("29A40E14-D037-42E8-558E-08DCE12C4CA6");
            }
            else if (warehouseName.Equals("Uttara", StringComparison.OrdinalIgnoreCase))
            {
                return new Guid("FA3C62BD-56D2-4A9B-558F-08DCE12C4CA6");
            }
            else if (warehouseName.Equals("WIP", StringComparison.OrdinalIgnoreCase))
            {
                return new Guid("AED770B7-B634-48B4-1536-08DCE895BD99");
            }
            else
            {
                throw new ArgumentException("Unknown warehouse name: " + warehouseName);
            }
        }

        public async Task<(IList<ProductDto> data, int total, int totaldisplay)> GetProductsSP(int pageIndex, int pageSize, ProductSearchDto search, string? order)
        {
           return await _InventoryUnitOfWork.GetPagedProductsUsingSPAsync(pageIndex, pageSize, search, order); 
        }

        public async Task<int> GetProductCountAsync()
        {
            return await _InventoryUnitOfWork.Product.GetCountAsync();  
        }
    }
}
