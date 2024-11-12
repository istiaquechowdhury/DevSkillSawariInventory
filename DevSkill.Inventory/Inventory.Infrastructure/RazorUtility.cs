using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class RazorUtility
    {
        public static IList<SelectListItem> SetCategoryValues(IList<Category> categories)
        {
            var Categories = (from c in categories
                              select new SelectListItem(c.Name, c.Id.ToString()))
                         .ToList();

            Categories.Insert(0, new SelectListItem("Category", string.Empty));



            return Categories;
        }

        public static IList<SelectListItem> SetTaxValues(IList<Taxes> taxes)
        {
            var tax = (from c in taxes
                              select new SelectListItem(c.Name, c.Id.ToString()))
                         .ToList();


            tax.Insert(0, new SelectListItem("Tax", string.Empty));


            return tax;
        }
        public static IList<SelectListItem> SetWareHouseValues(IList<WareHouse> wareHouses)
        {
            var warehouse = (from c in wareHouses
                       select new SelectListItem(c.Name, c.Id.ToString()))
                         .ToList();



            warehouse.Insert(0, new SelectListItem("Warehouses", string.Empty));

            return warehouse;
        }

        public static IList<SelectListItem> SetReasonValues(IList<Reason> Reasons)
        {
            var reasons = (from c in Reasons
                             select new SelectListItem(c.Name, c.Id.ToString()))
                         .ToList();

            


            return reasons;
        }

        public static IList<SelectListItem> SetMeasurementvalues(IList<MeasurementUnit> Measurements)
        {
            var measure = (from c in Measurements
                           select new SelectListItem(c.Name, c.Id.ToString()))
                         .ToList();

          


            return measure;
        }

    }
}
