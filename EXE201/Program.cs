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
using EXE201.DAL.DTOs.EmailDTOs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IDepositRepository, DepositRepository>();
builder.Services.AddScoped<IVerifyCodeRepository, VerifyCodeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRentalOrderRepository, RentalOrderRepository>();
builder.Services.AddScoped<IRentalOrderDetailRepository, RentalOrderDetailRepository>();
builder.Services.AddScoped<ICartRepository, CartRepostiory>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IFeedbacksRepository, FeedbackRepository>();


// Add services
builder.Services.AddScoped<IAddressServices, AddressServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IInventoryServices, InventoryServices>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IDepositServices, DepositServices>();
builder.Services.AddScoped<IForgotPawwordService, ForgotPasswordService>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IRentalOrderServices, RentalOrderServices>();
builder.Services.AddScoped<IRentalOrderDetailServices, RentalOrderDetailServices>();
builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<IMembershipServices, MembershipServices>();
builder.Services.AddScoped<IRatingServices, RatingServices>();
builder.Services.AddScoped<IFeedbackServices, FeedbackServices>();



//Add EmailSetting
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));

//Add services to the container.
builder.Services.AddDbContext<EXE201Context>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Name the Swagger 
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" }); });
var app = builder.Build();

//Get swagger.json follwing root directory 
app.UseSwagger(options => { options.RouteTemplate = "{documentName}/swagger.json"; });
//Load swagger.json follwing root directory 
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/v1/swagger.json", "Voguary API V1"); c.RoutePrefix = string.Empty; });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
