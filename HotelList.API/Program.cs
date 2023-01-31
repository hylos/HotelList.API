using HotelList.API.Configurations;
using HotelList.API.Data;
using HotelList.API.IRepository;
using HotelList.API.IRepository.Repository;
using HotelList.API.Middleware;
using HotelList.API.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Connection String
var conn = builder.Configuration.GetConnectionString("HotelListDbConnectionString");
builder.Services.AddDbContext<HotelListDbContext>(options =>
{
    options.UseSqlServer(conn);
});

//Securing API - Add Identity
builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>("HotelListingAPI") //Added this line for refresh token
    .AddEntityFrameworkStores<HotelListDbContext>()
    .AddDefaultTokenProviders(); //added this line for refresh token

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
builder.Services.AddScoped<IAuthManager, AuthManager>();

//Setup JWT Bearer Authentication.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters 
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime= true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors(SD.CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
