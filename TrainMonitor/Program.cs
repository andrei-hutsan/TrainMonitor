using Microsoft.EntityFrameworkCore;
using TrainMonitor.Data;
using TrainMonitor.Interfaces;
using TrainMonitor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddScoped<ITrainRepository, TrainRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();

builder.Services.AddHostedService<TrainBackgroundUpdater>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Train}/{action=Index}/{id?}"
);

app.Run();
