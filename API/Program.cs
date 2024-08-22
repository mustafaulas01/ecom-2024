using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<StoreContext>(opt=>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddCors();

//redis
builder.Services.AddSingleton<IConnectionMultiplexer>(config=>{

    var connString=builder.Configuration.GetConnectionString("Redis") ?? 
    throw new Exception("Cannot get redis connection string");

    
    var configuration=ConfigurationOptions.Parse(connString,true);

    return ConnectionMultiplexer.Connect(configuration);
});

//bunun singleton olması lazım çünkü constructor da IConnectionMultiplexer geçiyoruz oda program ayaga kalktığında bir instance alacak şekilde ayarlandı yukarıda

builder.Services.AddSingleton<ICartService,CartService>();

//redis son

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();

try 
{

using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
var context=services.GetRequiredService<StoreContext>();
await context.Database.MigrateAsync();
await StoreContextSeed.SeedAsync(context);


}

catch(System.Exception ex)
{
Console.WriteLine(ex);
}


app.Run();
