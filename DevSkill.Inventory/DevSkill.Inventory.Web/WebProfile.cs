using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile() 
        {
            CreateMap<ProductModel,Product>().ReverseMap();
            CreateMap<ProductUpdateModel,Product>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
            CreateMap<CategoryUpdateModel, Category>().ReverseMap();
            CreateMap<WareHouseModel, WareHouse>().ReverseMap();
            CreateMap<WareHouseUpdateModel, WareHouse>().ReverseMap();
            CreateMap<StockTransferModel, StockTransfer>().ReverseMap();
            CreateMap<StockAdjustmentModel, StockAdjustment>().ReverseMap();
            CreateMap<MeasureMentModel, MeasurementUnit>().ReverseMap();
            CreateMap<MeasurementUpdateModel, MeasurementUnit>().ReverseMap();
            CreateMap<ReasonCreateModel, Reason>().ReverseMap();

        }
    }
}
