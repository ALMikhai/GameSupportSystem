using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Hubs;
using Server.Services.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddDbContext<Server.Models.AppContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options => {
	options.Configuration = $"{builder.Configuration.GetValue<string>("RedisCache:Host")}:{builder.Configuration.GetValue<int>("RedisCache:Port")}";
});

builder.Services.AddIdentity<Server.Models.Operator.Account, IdentityRole>()
	.AddEntityFrameworkStores<Server.Models.AppContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.ConfigureApplicationCookie(options => {
	options.LoginPath = "/Operator/Login";
	options.AccessDeniedPath = "/Home/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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

app.MapHub<ChatHub>("/ChatHub");

app.Run();
