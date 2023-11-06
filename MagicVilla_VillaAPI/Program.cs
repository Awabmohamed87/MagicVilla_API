
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add services to the container.

// use Serilog logger instead of default one
/*Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("logs/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();*/

builder.Services.AddControllers(options =>
{
    //options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<ILogging, Logging>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
