using BlazorApp21.Api.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoConnectionString = builder.Configuration["MongoDbSettings__ConnectionString"] 
    ?? throw new Exception("MongoDB connection string is missing!");

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<MongoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("AllowBlazor");
app.UseAuthorization();
app.MapControllers();

app.Run();
