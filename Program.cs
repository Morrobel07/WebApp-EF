using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.OpenApi.Models;
//using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration[
        "ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);


});

//swagger
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
// });

// icluding xml format
// builder.Services.AddControllers()
//     .AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// builder.Services.AddControllers().AddNewtonsoftJson();

// builder.Services.Configure<MvcNewtonsoftJsonOptions>(options =>
// {
//     options.SerializerSettings.NullValueHandling
//     = Newtonsoft.Json.NullValueHandling.Ignore;
// });


// tell mvc to accept any format send by the header
// builder.Services.Configure<MvcOptions>(opts =>
// {
//     opts.RespectBrowserAcceptHeader = true;
//     opts.ReturnHttpNotAcceptable = true;
// });


//  builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// builder.Services.Configure<JsonOptions>(opts =>
// {
//     opts.JsonSerializerOptions.DefaultIgnoreCondition
//     = JsonIgnoreCondition.WhenWritingNull;
// });

// builder.Services.AddCors();



var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.MapControllerRoute("Default",
"{controller = Home}/{action=Index}/{id?}");


// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseSwagger();

// app.UseSwaggerUI(opts =>
// {
//     opts.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
// });




// app.UseMiddleware<WebApp.TestMiddleware>();

// app.MapGet("/", () => "Hello World!");



var context = app.Services.CreateScope().ServiceProvider
.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();
