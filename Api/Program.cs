using Api.Models;
using Api.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Env.Load();

builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGO_URI");
    options.DatabaseName = "api";
    options.CollectionName = "movies";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
