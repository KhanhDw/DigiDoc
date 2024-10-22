using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// cấu hình sử dụng database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnect");
    options.UseSqlServer(connectionString);
});

// Đăng ký PointsService để sử dụng Dependency Injection


// Thêm dịch vụ JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Tùy chỉnh cấu hình JSON tại đây
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Không thay đổi tên thuộc tính
    options.JsonSerializerOptions.WriteIndented = true; // Kích hoạt format JSON (đẹp)
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull; // Bỏ qua thuộc tính null
});

// cấu hình sử dụng session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm dịch vụ Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/NotFound";
        options.LogoutPath = "/Account/Logout";
    });


// upload file config -------v
// Configure IIS Server Options
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

// Configure Kestrel server options
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = int.MaxValue;
});

// Configure form options
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});
// -- end upload file config -----^




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// use wwwroot
app.UseStaticFiles();
app.UseRouting();
// Middleware pipeline
app.UseAuthentication();  // Thêm Middleware Authentication
app.UseAuthorization();   // Thêm Middleware Authorization
// use session
app.UseSession();

app.MapControllerRoute(
    name: "root",
    pattern: "/",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}");

// Route tùy chỉnh khác
app.MapControllerRoute(
    name: "Coins",
    pattern: "{controller}/{action=Index}/{id?}",
    defaults: new { controller = "Coins" });

app.MapControllerRoute(
    name: "DocMarket",
    pattern: "DocMarket/{action=Index}",
    defaults: new { controller = "DocMarket" });

app.MapControllerRoute(
    name: "Community",
    pattern: "Community/{action=Index}/{id?}",
    defaults: new { controller = "Community" });

app.MapControllerRoute(
    name: "Cart",
    pattern: "Cart/{action=Index}/{SearchName?}",
    defaults: new { controller = "Cart" });

app.MapControllerRoute(
    name: "Register",
    pattern: "{controller}/{action}",
    defaults: new { controller = "Register", action = "Index" });

app.MapControllerRoute(
    name: "Login",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Login", action = "Index" });

app.MapControllerRoute(
    name: "Account",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Account", action = "PersonalInformation" });

app.MapControllerRoute(
    name: "DetailDoc",
    pattern: "{controller}/{action}/{iddoc?}",
    defaults: new { controller = "DetailDoc", action = "Index" });

app.MapControllerRoute(
    name: "NotFound",
    pattern: "{controller}/{action}",
    defaults: new { controller = "NotFound", action = "index" });

app.MapControllerRoute(
    name: "CategoryBook",
    pattern: "{controller}/{action}/{page?}",
    defaults: new { controller = "CategoryBook", action = "Index" });

app.MapControllerRoute(
    name: "Search",
    pattern: "{controller}/{action}/{SearchName?}",
    defaults: new { controller = "Search", action = "Index" });

app.MapControllerRoute(
    name: "ManageUsers",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "ManageUsers", action = "Index" });

app.MapControllerRoute(
    name: "ManageDocuments",
    pattern: "{controller}/{action}",
    defaults: new { controller = "DocumentsManage", action = "Index" });

app.MapControllerRoute(
    name: "BillManage",
    pattern: "{controller}/{action}",
    defaults: new { controller = "BillManage", action = "Index" });

app.MapControllerRoute(
    name: "UploadDocument",
    pattern: "{controller}/{action}",
    defaults: new { controller = "UploadDocument", action = "Index" });

app.MapControllerRoute(
    name: "UpdateDocument",
    pattern: "{controller}/{action}",
    defaults: new { controller = "UpdateDocument", action = "Index" });

app.MapControllerRoute(
    name: "UpdateUserInfo",
    pattern: "{controller}/{action}",
    defaults: new { controller = "UpdateUserInfo", action = "Index" });

app.MapControllerRoute(
    name: "ProfileUser",
    pattern: "{controller}/{action}",
    defaults: new { controller = "ProfileUser", action = "Index" });

app.Run();