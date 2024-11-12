using Autofac.Extensions.DependencyInjection;
using Autofac;
using DevSkill.Inventory.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Reflection;
using DevSkill.Inventory.Web;
using Inventory.Infrastructure.InventoryDb;
using Inventory.Infrastructure.Identity;
using Inventory.Infrastructure.Extensions;
using Inventory.Domain;
using Autofac.Core;



#region Bootstrap logger
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .CreateBootstrapLogger();

#endregion


try
{
    #region file logger and Mssql logger
    Log.Information("application is starting");
    var builder = WebApplication.CreateBuilder(args);
    var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;
    var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: "Logs/web-log-.log",
        rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        restrictedToMinimumLevel: LogEventLevel.Information,
        columnOptions: new ColumnOptions
        {
        })
    .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    #endregion

    #region autofac

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(ContainerBuilder =>
    {
        ContainerBuilder.RegisterModule(new WebModule(connectionstring, migrationAssembly));
    });

    #endregion

    #region AutoMapper
    builder.Services.AddAutoMapper(typeof(WebProfile));
    #endregion

    builder.WebHost.UseUrls("http://*:80");

    #region General logger

    builder.Host.UseSerilog((ctx, lc) => lc
       .MinimumLevel.Debug()
       .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
       .Enrich.FromLogContext()
       .ReadFrom.Configuration(builder.Configuration)
       );

    #endregion




    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDbContext<InventoryDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));

    Log.Information(connectionString);
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //    .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddIdentity();


    builder.Services.AddControllersWithViews();

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    //app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "failed to start the Program");
}

finally
{
    Log.CloseAndFlush();
}
