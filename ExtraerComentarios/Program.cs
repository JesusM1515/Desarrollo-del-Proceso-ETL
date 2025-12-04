using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.API;
using Application.Services.API;
using Infraestructure.BD.Context;
using Infraestructure.Repositories.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Añadir la conexion con la base de datos cuando se este creando la aplicacion
builder.Services.AddSqlServer<Context>(builder.Configuration.GetConnectionString("AppConnection"));

//Servicios
builder.Services.AddScoped<IAPISocialCommentsRepository, APISocialCommentsRepository>();
builder.Services.AddScoped<ISocialComments, APISocialCommentsServices>();

builder.Services.AddSingleton<APISocialCommentsMap>();

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
