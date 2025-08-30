using Demo.WebUI.CustomMiddleware;
using Demo.WebUI.Hubs;
using Demo.WebUI.MiddlewareExtensions;
using Demo.WebUI.SubscribeTableDependencies;
using DemoTask.Core;
using DemoTask.Domain.RepositoryContract;
using DemoTask.Infrastructure.Context;
using DemoTask.Infrastructure.Context.Repository;
using DemoTask.Service.Interface;
using DemoTask.Service.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DemoTaskContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TMConnection")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDapperService, DapperService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMasterMenuRepository, MasterMenuRepository>();
builder.Services.AddTransient<IMasterMenuService, MasterMenuService>();
builder.Services.AddTransient<IUserRoleMasterRepository, UserRoleMasterRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IMemberTaskRepository, MemberTaskRepository>();
builder.Services.AddTransient<IMemberTaskService, MemberTaskService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
//DI
builder.Services.AddSingleton<ProjectHub>();
builder.Services.AddSingleton<SubscribeProjectTableDependency>();
builder.Services.AddSingleton<MemberTaskHub>();
builder.Services.AddSingleton<SubscribeMemberTaskTableDependency>();
builder.Services.AddSingleton<SubscribeUserTableDependency>();
builder.Services.AddSingleton<SubscribeMemberTaskLastUpdateTableDependency>();
builder.Services.AddSingleton<OnlineUsersService>();
builder.Services.AddSingleton<EmailService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); 
builder.Services.AddMemoryCache();
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

var app = builder.Build();
//app.UseMiddleware<OnlineUsersMiddleware>();
var connectionString = app.Configuration.GetConnectionString("TMConnection");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();
app.MapHub<ProjectHub>("/projectHub");
app.MapHub<MemberTaskHub>("/memberTaskHub");
app.UseStaticFiles();
app.UseSession();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseSqlTableDependency<SubscribeProjectTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeMemberTaskTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeUserTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeMemberTaskLastUpdateTableDependency>(connectionString);
app.Run();
