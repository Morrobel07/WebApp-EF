using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration[
        "ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);


});


builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition
    = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCors();


var app = builder.Build();

app.MapControllers();



app.UseMiddleware<WebApp.TestMiddleware>();

app.MapGet("/", () => "Hello World!");



var context = app.Services.CreateScope().ServiceProvider
.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();
