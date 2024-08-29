using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieApi.Models;
using MovieApi.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGO_URI");
    options.DatabaseName = "api";
    options.CollectionName = "movies";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.Configure<MongoDBSettings>(
// builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<PeliculaService>();
builder.Services.AddScoped<PeliculaSearchService>();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
