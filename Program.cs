using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
//using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration[
        "ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);


});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<MvcNewtonsoftJsonOptions>(options =>
{
    options.SerializerSettings.NullValueHandling
    = Newtonsoft.Json.NullValueHandling.Ignore;
});


builder.Services.AddControllers();

// builder.Services.Configure<JsonOptions>(opts =>
// {
//     opts.JsonSerializerOptions.DefaultIgnoreCondition
//     = JsonIgnoreCondition.WhenWritingNull;
// });

builder.Services.AddCors();


var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseMiddleware<WebApp.TestMiddleware>();

app.MapGet("/", () => "Hello World!");



var context = app.Services.CreateScope().ServiceProvider
.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();
