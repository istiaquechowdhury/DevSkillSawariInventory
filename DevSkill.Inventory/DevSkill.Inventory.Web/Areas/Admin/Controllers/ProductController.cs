using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Services;
using AutoMapper;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure.InventoryDb;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]

    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly IMapper _mapper;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly ILogger<ProductController> _logger;
        private readonly ITaxManagementService _taxManagementService;
        private readonly IMeasurementManagementService _MeasureManagementService;
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly IWebHostEnvironment _hostingEnvironment; 





        public ProductController(IProductManagementService productManagementServices,
            IMapper mapper, ICategoryManagementService categoryManagementservice,ILogger<ProductController> logger,
            ITaxManagementService taxManagementService, IMeasurementManagementService measureManagementService,
            InventoryDbContext inventoryDbContext, IWebHostEnvironment hostingEnvironment)

        {
            _categoryManagementService = categoryManagementservice;
            _productManagementService = productManagementServices;
            _mapper = mapper;
            _logger = logger;
            _taxManagementService = taxManagementService;
            _MeasureManagementService = measureManagementService;
            _inventoryDbContext = inventoryDbContext; 
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {


            return View();
        }

        public IActionResult IndexSP()
        {
            var model = new ProductListModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            model.SetTaxValues(_taxManagementService.GetTaxes());
          





            return View(model);
        }


        public JsonResult GetProductJsonData([FromBody] ProductListModel model)
        {

            var result = _productManagementService.GetProducts(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("ImagePath", "ItemName", "Barcode", "Category", "AverageCost", "Price", "Taxes", "InStock", "Status"));
            var ProductJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {


                         "",
                         HttpUtility.HtmlEncode(record.ImagePath),
                         HttpUtility.HtmlEncode(record.ItemName),
                         HttpUtility.HtmlEncode(record.Barcode),
                         HttpUtility.HtmlEncode(record.Category?.Name),
                         HttpUtility.HtmlEncode(record.AverageCost),
                         HttpUtility.HtmlEncode(record.Price),
                         HttpUtility.HtmlEncode(record.Taxes?.Percentage),
                         HttpUtility.HtmlEncode(record.TotalWarehouseStock),
                       
                         HttpUtility.HtmlEncode(record.Status ? "Active" : "Inactive"),






                         record.Id.ToString(),
                         HttpUtility.HtmlEncode(record.InStock),

                         

                        }
                    ).ToArray()
            };

            return Json(ProductJsonData);

        }
        public async Task<JsonResult> GetProductJsonDataSP([FromBody] ProductListModel model)
        {

            var result = await _productManagementService.GetProductsSP(model.PageIndex, model.PageSize, model.SearchItem, model.FormatSortExpression("ImagePath", "ItemName", "Barcode", "Category", "AverageCost", "Price", "Taxes", "InStock", "Status"));
            var ProductJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {


                         
                         HttpUtility.HtmlEncode(record.ImagePath),
                         HttpUtility.HtmlEncode(record.ItemName),
                         HttpUtility.HtmlEncode(record.Barcode),
                         HttpUtility.HtmlEncode(record.CategoryName),

                         HttpUtility.HtmlEncode(record.TaxPercentage),
                         HttpUtility.HtmlEncode(record.TotalStock),

                         HttpUtility.HtmlEncode(record.Status ? "Active" : "Inactive"),
                         HttpUtility.HtmlEncode(record.InStock),

                         record.Id.ToString(),
                        



                        }
                    ).ToArray()
            };

            return Json(ProductJsonData);

        }






        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            var model = new ProductModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            model.SetTaxValues(_taxManagementService.GetTaxes());
            model.SetMeasureValues(_MeasureManagementService.GetMeasurements());
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel productModel, bool redirectToNew = false)
        {
            if (ModelState.IsValid)
            {
                // Handle image file if provided
                if (productModel.ImageFile != null && productModel.ImageFile.Length > 0)
                {
                    string uniqueFileName = _productManagementService.SetImagePath(productModel.ImageFile);
                    productModel.ImagePath = uniqueFileName;
                }

                // Map ProductModel to Product entity
                var product = _mapper.Map<Product>(productModel);

                // Set related entities
                product.Category = _categoryManagementService.GetCategory(productModel.CategoryId ?? Guid.Empty);
                product.Taxes = _taxManagementService.GetTax(productModel.TaxesId ?? Guid.Empty);
                product.MeasurementUnit = _MeasureManagementService.GetMeasurement(productModel.MeasurementUnitId ?? Guid.Empty);

                try
                {
                    // Save the product
                    _productManagementService.CreateProduct(product);

                    _productManagementService.AddStockListEntriesForProduct(product);

                    TempData["success"] = "item inserted successfully";

                    // Redirect based on whether 'Save and New' was clicked
                    if (redirectToNew)
                    {
                        // Redirect back to the create page to add another product
                        TempData["success"] = " item inserted successfully";
                        return RedirectToAction("Create");
                       

                    }
                    else
                    {
                        // Redirect to the product list page (Index)
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "item name should be unique";
                    _logger.LogError(ex, "Product creation failed");
                }

                // If we get here, something went wrong, redirect to Index
                return RedirectToAction("Index");
            }

            // If the model is invalid, re-populate dropdowns and return to the view
            productModel.SetTaxValues(_taxManagementService.GetTaxes());
            productModel.SetMeasureValues(_MeasureManagementService.GetMeasurements());
            productModel.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(productModel);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id)
        {
            Product product = _productManagementService.GetProduct(id);

            var model = _mapper.Map<ProductUpdateModel>(product);
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            model.SetTaxValues(_taxManagementService.GetTaxes());  
            model.SetMeasureValues(_MeasureManagementService.GetMeasurements());
           
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(ProductUpdateModel productUpdateModel, bool redirectToNew = false)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing product from the database
                var product = _productManagementService.GetProduct(productUpdateModel.Id);

                // Handle image removal if the checkbox is checked
                if (productUpdateModel.RemoveImage)
                {
                    // Delete the existing image if it exists
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", product.ImagePath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        productUpdateModel.ImagePath = null; // Set image path to null
                    }
                }
                else
                {
                    // Handle image file if a new one is uploaded
                    if (productUpdateModel.ImageFile != null && productUpdateModel.ImageFile.Length > 0)
                    {
                        string uniqueFileName = _productManagementService.SetImagePath(productUpdateModel.ImageFile);
                        productUpdateModel.ImagePath = uniqueFileName;
                    }
                    else
                    {
                        // Retain the existing image path if no new image is uploaded
                        productUpdateModel.ImagePath = product.ImagePath;
                    }
                }

                // Map the updated model values to the existing product entity
                product = _mapper.Map(productUpdateModel, product);

                // Set related entities (Category, Taxes, Measurement)
                product.Category = _categoryManagementService.GetCategory(productUpdateModel.CategoryId ?? Guid.Empty);
                product.Taxes = _taxManagementService.GetTax(productUpdateModel.TaxesId ?? Guid.Empty);
                product.MeasurementUnit = _MeasureManagementService.GetMeasurement(productUpdateModel.MeasurementUnitId ?? Guid.Empty);

                // Retain the image path
                product.ImagePath = productUpdateModel.ImagePath;

                try
                {
                    // Update the product in the database
                    _productManagementService.UpdateProduct(product);

                    TempData["success"] = "Item updated successfully";

                    // Redirect based on whether 'Save and New' was clicked
                    if (redirectToNew)
                    {
                        // Redirect back to the create page to add a new product after updating the current one
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        // Redirect to the product list page (Index)
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Item should be unique";
                    _logger.LogError(ex, "Product update failed");

                    // Return to the Index on error
                    return RedirectToAction("Index");
                }
            }

            // If the model is invalid, re-populate dropdowns and return to the view
            productUpdateModel.SetTaxValues(_taxManagementService.GetTaxes());
            productUpdateModel.SetMeasureValues(_MeasureManagementService.GetMeasurements());
            productUpdateModel.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(productUpdateModel);
        }



        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productManagementService.DeleteProduct(id);

                    TempData["success"] = "item Deleted Successfully";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "item delete failed";

                    _logger.LogError(ex, "Blog post creation failed");

                    return RedirectToAction("Index");



                }
               
            }

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkDelete([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No products selected for deletion.");
            }

            try
            {
                await _productManagementService.DeleteProductsAsync(ids); // Call service layer for deletion
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulk delete operation failed.");
                return StatusCode(500, "Internal server error");
            }
        }

       
    }
}
