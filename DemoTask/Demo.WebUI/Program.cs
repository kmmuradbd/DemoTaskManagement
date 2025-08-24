using DemoTask.Core;
using DemoTask.Domain.RepositoryContract;
using DemoTask.Infrastructure.Context;
using DemoTask.Infrastructure.Context.Repository;
using DemoTask.Service.Interface;
using DemoTask.Service.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DemoTaskContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TMConnection")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDapperService, DapperService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMasterMenuRepository, MasterMenuRepository>();
builder.Services.AddTransient<IMasterMenuService, MasterMenuService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.UseSession();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
