using ETICARET.Business.Abstract;
using ETICARET.Business.Concrete;
using ETICARET.DataAccess.Abstract;
using ETICARET.DataAccess.Concrete.EfCore;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Middlewares;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

// Seed Identity
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();


builder.Services.Configure<IdentityOptions>(
    options =>
    {
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 0;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    }
);

// Cookie Configuration

builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.AccessDeniedPath = "/Account/accesdenied";
        options.LoginPath = "/Account/login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie = new CookieBuilder()
        {
            HttpOnly = true,
            Name = "ETICARET.Security.Cookie",
            SameSite = SameSiteMode.Strict
        };
    }
);

// Dataaccess and Business

builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICommentDal, EfCoreCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IOrderDal, EfCoreOrderDal>();
builder.Services.AddScoped<IOrderService, OrderManager>();


// Projeyi MVC mimarisi olacak þekilde tanýmlama

builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Dataacces Seed Database (Product,Category,ProductCategory)
SeedDatabase.Seed();

app.UseStaticFiles();
app.CustomStaticFiles(); // custom static files (middleware)  node_modules 
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();


// endpoints

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
            name: "products",
            pattern: "products/{category?}",
            defaults: new { controller = "Shop", action = "List" }
    );
    endpoints.MapControllerRoute(
            name: "adminProducts",
            pattern: "admin/products",
            defaults: new { controller = "Admin", action = "ProductList" }
    );
    endpoints.MapControllerRoute(
            name: "adminProducts",
            pattern: "admin/products/{id?}",
            defaults: new { controller = "Admin", action = "EditProduct" }
    );
    endpoints.MapControllerRoute(
            name: "adminProducts",
            pattern: "admin/categories",
            defaults: new { controller = "Admin", action = "CategoryList" }
    );
    endpoints.MapControllerRoute(
            name: "adminProducts",
            pattern: "admin/categories/{id?}",
            defaults: new { controller = "Admin", action = "EditCategory" }
    );
    endpoints.MapControllerRoute(
            name: "cart",
            pattern: "cart",
            defaults: new { controller = "Cart", action = "Index" }
    );
    endpoints.MapControllerRoute(
            name: "checkout",
            pattern: "checkout",
            defaults: new { controller = "Cart", action = "Checkout" }
    );
    endpoints.MapControllerRoute(
            name: "orders",
            pattern: "orders",
            defaults: new { controller = "Cart", action = "GetOrders" }
    );

}
);

// Seed Identity

SeedIdentity.Seed(userManager, roleManager, app.Configuration).Wait();

app.Run();
