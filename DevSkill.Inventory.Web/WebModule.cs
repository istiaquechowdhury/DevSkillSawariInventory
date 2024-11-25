using Autofac;
using DevSkill.Inventory.Web.Data;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Domain.UnitOfWorkInterfaces;
using Inventory.Infrastructure;
using Inventory.Infrastructure.InventoryDb;
using Inventory.Infrastructure.RepositoryPatternClasses;
using Inventory.Infrastructure.UnitOfWorkClasses;


namespace DevSkill.Inventory.Web
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
                {
                    builder.RegisterType<InventoryDbContext>().AsSelf()
                        .WithParameter("connectionString", connectionString)
                        .WithParameter("migrationAssembly", migrationAssembly)
                        .InstancePerLifetimeScope();

                    builder.RegisterType<ApplicationDbContext>().AsSelf()
                        .WithParameter("connectionString", connectionString)
                        .WithParameter("migrationAssembly", migrationAssembly)
                        .InstancePerLifetimeScope();

                    builder.RegisterType<ProductRepository>()
                        .As<IProductRepository>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<InventoryUnitOfWork>()
                        .As<IInventoryUnitOfWork>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<ProductManagementService>()
                        .As<IProductManagementService>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<CategoryManagementService>()
                        .As<ICategoryManagementService>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<CategoryRepository>()
                       .As<ICategoryRepository>()
                       .InstancePerLifetimeScope();

                    builder.RegisterType<TaxManagementService>()
                       .As<ITaxManagementService>()
                       .InstancePerLifetimeScope();

                    builder.RegisterType<TaxRepository>()
                      .As<ITaxRepository>()
                      .InstancePerLifetimeScope();

                    builder.RegisterType<WareHouseRepository>()
                    .As<IWareHouseRepository>()
                    .InstancePerLifetimeScope();

                    builder.RegisterType<WareHouseManagementService>()
                      .As<IWareHouseManagementService>()
                      .InstancePerLifetimeScope();

                    builder.RegisterType<StockListRepository>()
                   .As<IStockListRepository>()
                   .InstancePerLifetimeScope();

                    builder.RegisterType<StockListManagementService>()
                    .As<IStockListManagementService>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferRepository>()
                  .As<IStockTransferRepository>()
                  .InstancePerLifetimeScope();

                   builder.RegisterType<StockTransferManagementService>()
                  .As<IStockTransferManagementService>()
                  .InstancePerLifetimeScope();

                   builder.RegisterType<StockAdjustmentRepository>()
                  .As<IStockAdjustmentRepository>()
                  .InstancePerLifetimeScope();

                 builder.RegisterType<StockAdjustmentManagementService>()
                .As<IStockAdjustmentManagementService>()
                .InstancePerLifetimeScope();

                builder.RegisterType<ReasonManagementService>()
                .As<IReasonManagementService>()
                .InstancePerLifetimeScope();

                    builder.RegisterType<ReasonRepository>()
                  .As<IReasonRepository>()
                  .InstancePerLifetimeScope();

                    builder.RegisterType<MeasurementManagementService>()
                     .As<IMeasurementManagementService>()
                     .InstancePerLifetimeScope();

                        builder.RegisterType<MeasurementRepository>()
                    .As<IMeasurementRepository>()
                    .InstancePerLifetimeScope();


            builder.RegisterType<EmailUtility>()
             .As<IEmailUtility>()
             .InstancePerLifetimeScope();





        }
    }

}
