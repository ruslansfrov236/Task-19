using book.business.Abstract;
using book.business.Concrete;
using book.data.Abstract;
using book.data.Concrete.EfCore;
using book.webui.EmailServices;
using book.webui.Identify;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IEmailSender>(builder.Configuration
                         
                        .GetSection("EmailSender")
                       );
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
// Get the current configuration file.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=shopDb"));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    //şifre {password}
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    //bloklama {lockout}
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.AllowedForNewUsers = true;
    // Istifadeci {user}
    // options.User.AllowedUserNameCharacters="";
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // options.SignIn.RequireConfirmedAccount=true;


});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = false;
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".Book.Security.Cookie",
        SameSite = SameSiteMode.Strict
    };
});
builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<IProductRepository, EfCoreProductRepository>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
//  builder.Services.AddScoped<IEmailSender , SmtpEmailSender>(i=> new EmailSender(
// _configuration ["EmailSender:Host"]
// ));
 builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>( i=> new EmailSender(
    configuration["EmailSender:Host"]
 ));


builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    SeedDatabase.Seed();
    app.UseDeveloperExceptionPage();




}
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "category",
    pattern: "admin/category",
    defaults: new { controller = "Admin", action = "CategoryList" }
);
app.MapControllerRoute(
    name: "categoryedit",
    pattern: "admin/edit/{id?}",
    defaults: new { controller = "Admin", action = "CategoryEdit" }
);

app.MapControllerRoute(
    name: "categorycreate",
    pattern: "admin/create",
    defaults: new { controller = "Admin", action = "CategoryCreate" }
);
app.MapControllerRoute(
    name: "adminproductlist",
    pattern: "admin/products",
    defaults: new { controller = "Admin", action = "ProductList" }
);
app.MapControllerRoute(
    name: "adminedit",
    pattern: "admin/predit/{id?}",
    defaults: new { controller = "Admin", action = "Edit" }
);
app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Book", action = "search" }
);

app.MapControllerRoute(
    name: "products",
    pattern: "products/{category?}",
    defaults: new { controller = "Book", action = "list" }
);
app.MapControllerRoute(
    name: "products",
    pattern: "admin/productcreate",
    defaults: new { controller = "Admin", action = "CreateProduct" }
);
app.MapControllerRoute(
    name: "productsdetails",
    pattern: "{url}",
    defaults: new { controller = "Book", action = "details" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();