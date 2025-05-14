using System;
using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrar clases de Rol
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<RolBusiness>();

// Registrar clases de ChangeLog
builder.Services.AddScoped<ChangeLogData>();
builder.Services.AddScoped<ChangeLogBusiness>();

// Registrar clases de Form
builder.Services.AddScoped<FormData>();
builder.Services.AddScoped<FormBusiness>();

// Registrar clases de FormModule
builder.Services.AddScoped<FormModuleData>();
builder.Services.AddScoped<FormModuleBusiness>();


// Registrar clases de Module
builder.Services.AddScoped<ModuleData>();
builder.Services.AddScoped<ModuleBusiness>();

// Registrar clases de Person
builder.Services.AddScoped<PersonData>();
builder.Services.AddScoped<PersonBusiness>();


// Registrar clases de RolForm
builder.Services.AddScoped<RolFormPermissionData>();
builder.Services.AddScoped<RolFormPermissionBusiness>();

// Registrar clases de Permission
builder.Services.AddScoped<PermissionData>();
builder.Services.AddScoped<PermissionBusiness>();

// Registrar clases de User
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<UserBusiness>();

// Registrar clases de UserRol
builder.Services.AddScoped<UserRolData>();
builder.Services.AddScoped<UserRolBusiness>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Agregar CORS 
var OrigenesPermitidos = builder.Configuration.GetValue<string>
    ("OrigenesPermitidos")!.Split(',');

builder.Services.AddCors(Opciones =>
{
    Opciones.AddPolicy("AllowSpecificOrigins", politica =>
    {
        politica.WithOrigins(OrigenesPermitidos)
        .WithOrigins("http://127.0.0.1:5500")
        .WithOrigins("http://127.0.0.1:61371")

        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//Agregar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
