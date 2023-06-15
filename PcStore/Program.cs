using Microsoft.EntityFrameworkCore;
using PcStore.Data;
using Microsoft.AspNetCore.Identity;
using PcStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Somkin1Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Somkin1IdentityContext>();
builder.Services.AddIdentity<User, IdentityRole>().AddDefaultUI().AddEntityFrameworkStores<Somkin1IdentityContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<Somkin1IdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultIdentityConnection"));
});
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
    pattern: "{controller=Items}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
