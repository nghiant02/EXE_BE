using EXE201.BLL.DTOs;
using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using EXE201.DAL.Repository;
using MCC.DAL.Repository.Implements;
using MCC.DAL.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();


// Add services
builder.Services.AddScoped<IAddressServices, AddressServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();


// Add services to the container.
builder.Services.AddDbContext<EXE201Context>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
