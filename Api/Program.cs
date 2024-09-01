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

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<KafkaProducer>(provider =>
{
    var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVER");
    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
    return new KafkaProducer(bootstrapServers, topic);
});

builder.Services.AddHostedService<KafkaHeartbeatService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MovieService>();
builder.Services.AddScoped<MovieSearchService>();
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
