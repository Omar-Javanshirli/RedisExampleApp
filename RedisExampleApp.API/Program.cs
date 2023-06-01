using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repositories;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("myDatabase");
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

//RedisService-in Constructo-ru parametir qebul edir.Paremetri asagida ki kod vasitesi ile gonderirik.
builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]!);
});


//Numune.
//builder.Services.AddSingleton<IDatabase>(sp =>
//{
//    //yuxarida yazilmisa RedisService -den RediServici elde edirem.
//    var redisService = sp.GetRequiredService<RedisService>();
//    return redisService.GetDb(0);  
//});

var app = builder.Build();


//InMemory Default olarag bize datalari gosdermir.Biz context vasitesi ile ve Database.EnsureCreated methodunu cagirmaliyiq.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //EnsureCreated => Program her defe run oldugu zaman Database sifirdan yani yeniden yaradir.
    dbContext.Database.EnsureCreated();
}

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