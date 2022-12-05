using BookStore.Helpers;
using BookStore.Models;
using BookStore.Services;
using FirstConsoleApp.Data;
using FirstConsoleApp.Models;
using FirstConsoleApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // where to put dependency injection (before builder.build)

        builder.Services.AddDbContext<BookStoreContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });


        builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreContext>().AddDefaultTokenProviders();

        // configure password validations
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 5;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 4;
        });

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(5);
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = builder.Configuration["App:LoginPath"];
        });

        builder.Services.AddControllersWithViews();

// this will obly be enabled in debug mode (preprocessor directive)
#if DEBUG
        // make updates at runtime
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

        #region DisableClientSideValidation
        // disable client side validation
        //builder.Services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(options =>
        //{
        //    options.HtmlHelperOptions.ClientValidationEnabled = false;
        //}); 
        #endregion
#endif

        // Dependency injection service for BookRepository
        // first parameter is the interface and the second is the impelementation of the interface
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        // register custom claims
        builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

        builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));

        var app = builder.Build();

        #region Environment
        ////maps a particular url to a particular resource
        //app.UseRouting();

        ////requires UseRouting()
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.Map("/", async context =>
        //    {
        //        if (app.Environment.IsDevelopment())
        //        {
        //            await context.Response.WriteAsync(app.Environment.EnvironmentName);
        //        }
        //        else
        //        {
        //            await context.Response.WriteAsync("Hello");
        //        }
        //    });
        //}); 
        #endregion


        //use middleware pipelines(after builder.build)
        // order of middleware matters a lot

        // tells app to use static files (wwwroot)
        app.UseStaticFiles();

        #region OtherStaticFolder
        // tells app to use static files (any other folder)
        //app.UseStaticFiles(new StaticFileOptions()
        //{
        //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
        //    RequestPath = "/MyStaticFiles"
        //}); 
        #endregion


        app.UseAuthentication();
        app.UseAuthorization();


        app.MapDefaultControllerRoute();
        app.MapControllerRoute(
            name: "MyArea",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

        //app.MapControllerRoute(
        //    name: "Default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}"
        //);

        // not specifying any route just adds endpoints for controllers
        //app.MapControllers();

        #region set about us route
        //set a url for about us page
        // url is domain/about-us
        //app.MapControllerRoute(
        //    name: "AboutUs",
        //    pattern: "about-us",
        //    defaults: new {controller = "Home", Action = "AboutUs" }
        //); 
        #endregion

        #region app.Use
        //app.Use(async (context, next) =>
        //{
        //    await context.Response.WriteAsync("Hello from my first middleware");
        //}); 
        #endregion

        #region app.UseEndoints
        // maps a particular url to a particular resource
        //app.UseRouting();

        // requires UseRouting()
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.Map("/", async context =>
        //    {
        //        await context.Response.WriteAsync("Hello From Webgentle App");
        //    });
        //});

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.Map("/asd", async context =>
        //    {
        //        await context.Response.WriteAsync("Hello From Webgentle App");
        //    });
        //}); 
        #endregion

        #region app.Map
        // doesn't require UseRouting()
        //app.Map("/", async context =>
        //{
        //    await context.Response.WriteAsync("Hello Rooney");
        //});

        //app.Map("/asd", async context =>
        //{
        //    await context.Response.WriteAsync("Hello Rooneya");
        //}); 
        #endregion



        app.Run();
    }
}