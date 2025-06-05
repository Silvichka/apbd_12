using apbd_12.Data;
using apbd_12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITripService, TripService>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();

app.Run();

