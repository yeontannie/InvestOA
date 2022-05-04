using FluentValidation.AspNetCore;
using InvestOA.Core.Data;
using InvestOA.Core;
using InvestOA.Core.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InvestOA.Repositories;
using InvestOA.DataManager;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    //Db
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

    //For Identity
    builder.Services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddControllersWithViews();

    //FluentValidation
    builder.Services.AddMvc()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>());

    builder.Services.AddScoped<EmailService>();
    builder.Services.AddScoped<PortfolioRepository>();
    builder.Services.AddScoped<HistoryRepository>();
    builder.Services.AddScoped<MainActions>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
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
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    throw exception;
}