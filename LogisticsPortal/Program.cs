using LogisticsPortal.Components;
using LogisticsPortal.Data;
using LogisticsPortal.Services;
using LogisticsPortal.State;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add Entity Framework Core with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=logisticsportal.db";

builder.Services.AddDbContext<LogisticsContext>(options =>
    options.UseSqlite(connectionString));

// Add application services
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<DriverService>();

// Add state container
builder.Services.AddScoped<ShipmentState>();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LogisticsContext>();
    await dbContext.Database.MigrateAsync();
    await DbInitializer.InitializeAsync(dbContext);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
