
using Binance.Net;
using RiskMgmt.Web.Hubs;
using RiskMgmt.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services
//builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Configure Binance.Net
builder.Services.AddBinance();

// Register custom services
builder.Services.AddSingleton<ICryptoPriceService, CryptoPriceService>();
builder.Services.AddScoped<IRiskCalculatorService, RiskCalculatorService>();
builder.Services.AddHostedService<CryptoPriceService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<PriceHub>("/priceHub");


app.Run();
