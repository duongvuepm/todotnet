using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoApp.Middlewares;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseMySQL(builder.Configuration["Database:Connection"]));

builder.Services.AddDbContext<AuthContext>(opt =>
    opt.UseMySQL(builder.Configuration["Database:Connection"]));
builder.Services.AddIdentityCore<MyUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddApiEndpoints();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Description = "Backend API for the Todo application",
        Title = "TodoApi",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
});

builder.Services.AddScoped<TodoContext>();
builder.Services.AddScoped<AuthContext>();
builder.Services.AddScoped<WorkflowService>();
builder.Services.AddKeyedScoped<ITodoItemService, TodoService>("TodoService");
builder.Services.AddKeyedScoped<ITodoItemService, ItemService>("ItemService");
builder.Services.AddScoped<ItemService>();

builder.Services.AddScoped<StateService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<TransitionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddKeyedScoped<IRepository<Item, long>, ItemRepository>("ItemRepository");
builder.Services.AddKeyedScoped<IRepository<Board, long>, BoardRepository>("BoardRepository");
builder.Services.AddKeyedScoped<IRepository<State, long>, StateRepository>("StateRepository");
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddKeyedScoped<IRepository<Transition, long>, TransitionRepository>("TransitionRepository");

builder.Services.AddControllers();

var app = builder.Build();

app.MapIdentityApi<MyUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}