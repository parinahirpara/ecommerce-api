using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Mappings;
using EcommerceAPI.Repositories;
using EcommerceAPI.Repositories.Products;
using EcommerceAPI.Services;
using EcommerceAPI.Services.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
    // You can add other profiles here as well: cfg.AddProfile<ProductProfile>();
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This stops the infinite loop by ignoring nested reference loops entirely
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();

builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularAdminPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4300") // Your Angular App URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Required if you plan to use HttpOnly cookies/sessions later
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AngularAdminPolicy");
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
