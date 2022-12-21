using HotelList.API.Configurations;
using HotelList.API.Data;
using HotelList.API.IRepository;
using HotelList.API.IRepository.Repository;
using HotelList.API.Static;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Connection String
var conn = builder.Configuration.GetConnectionString("HotelListDbConnectionString");
builder.Services.AddDbContext<HotelListDbContext>(options =>
{
    options.UseSqlServer(conn);
});

builder.Services.AddControllers().AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setup cors for API external access on the network.
builder.Services.AddCors(options =>
{
    options.AddPolicy(SD.CorsPolicyName, p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

//Set up Seri Log
builder.Host.UseSerilog((context, loggerCongiguration) => 
loggerCongiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

//Set up AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

//Set up Repository Pattern
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(SD.CorsPolicyName);
app.UseAuthorization();

app.MapControllers();

app.Run();
