using FluentValidation;
using server.DTOs;
using server.Extensions;
using server.Interfaces;
using server.Middlewares;
using server.Repositories;
using server.Services;
using server.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabaseService(builder.Configuration);

// Register repository services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUtilities, Utilities>();

// Register application service layers
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Register FluentValidation services
builder.Services.AddTransient<IValidator<ProductsDTO>, ProductRequestValidator>();
builder.Services.AddTransient<IValidator<UpdateProductDTO>, UpdateProductRequestValidator>();

// Register AutoMapper service with configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Enable CORS

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
