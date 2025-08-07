using EnergyLegacyApp.Business;
using EnergyLegacyApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:80");

// Register services for DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DatabaseHelper>();
builder.Services.AddScoped<PowerPlantRepository>();
builder.Services.AddScoped<EnergyConsumptionRepository>();
builder.Services.AddScoped<EnergyAnalyticsService>();

var app = builder.Build();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
