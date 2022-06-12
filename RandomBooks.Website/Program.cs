global using Microsoft.AspNetCore.Components.Authorization;
global using RandomBooks.Website.Logic.Services.AuthService;
global using RandomBooks.Website.Logic.Services.UserService;
global using RandomBooks.Website.Logic.Services.CustomerService;
global using RandomBooks.Website.Logic.Services.BookService;
global using RandomBooks.Website.Logic.Services.CategoryService;
global using RandomBooks.Website.Logic.Services.LanguageService;
global using RandomBooks.Website.Logic.Services.PublisherService;
global using RandomBooks.Website.Logic.Services.AuthorService;
global using RandomBooks.Website.Logic.Services.CartService;
global using RandomBooks.Website.Logic.Services.OrderService;
global using RandomBooks.Website.Logic.Services.ChartService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RandomBooks.Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IChartService, ChartService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

await builder.Build().RunAsync();
