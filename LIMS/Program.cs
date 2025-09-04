using DataAccessLayer;
using LIMS.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Access content root path
var env = builder.Environment;
string publicKeyPath = Path.Combine(env.ContentRootPath, "Keys", "public_key.pem");
string privateKeyPath = Path.Combine(env.ContentRootPath, "Keys", "private_key.pem");
Cryptohelper.Initialize(publicKeyPath, privateKeyPath);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://192.168.1.19:8082")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Only if you're using cookies or Authorization headers
    });

});

builder.Services.AddControllers();
//builder.Services.AddScoped<DataClass>();
builder.Services.AddScoped<LoginDAL>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();