global using Microsoft.EntityFrameworkCore;
global using RandomBooks.Data.Context;
global using RandomBooks.Shared;
global using RandomBooks.Shared.Authentication;
global using RandomBooks.Shared.DatabaseModels;
global using RandomBooks.Shared.DTOs;
global using RandomBooks.API.Services.AuthService;
global using RandomBooks.API.Services.UserService;
global using RandomBooks.API.Services.BookService;
global using RandomBooks.API.Services.CategoryService;
global using RandomBooks.API.Services.LanguageService;
global using RandomBooks.API.Services.PublisherService;
global using RandomBooks.API.Services.AuthorService;
global using RandomBooks.API.Services.CartService;
global using RandomBooks.API.Services.OrderService;
global using RandomBooks.API.Services.ChartService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// RegisterServices
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IChartService, ChartService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RandomBooks.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Format:   Bearer {Token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:JWTToken").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddHttpContextAccessor();

//Enable CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
     .AllowAnyHeader());
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
