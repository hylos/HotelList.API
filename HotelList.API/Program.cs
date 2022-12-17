using HotelList.API.Static;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
