using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApp.Middlewares;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseMySQL("server=localhost;database=my_database;user=root;password=tungduong98"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Description = "Backend API for the Todo application",
        Title = "TodoApi", Version = "v1", Contact = new OpenApiContact
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
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// builder.Services.AddProblemDetails();
// builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);
    // .AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddDbContext<AuthContext>(opt =>
    opt.UseMySQL("server=localhost;database=my_database;user=root;password=tungduong98"));
builder.Services.AddIdentityCore<MyUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddApiEndpoints();

builder.Services.AddScoped<TodoContext>();
builder.Services.AddScoped<AuthContext>();
builder.Services.AddScoped<WorkflowService>();
builder.Services.AddKeyedScoped<ITodoItemService, TodoService>("TodoService");
builder.Services.AddKeyedScoped<ITodoItemService, ItemService>("ItemService");
builder.Services.AddScoped<ItemService>();

builder.Services.AddScoped<StateService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<TransitionService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddKeyedScoped<IRepository<Item, long>, ItemRepository>("ItemRepository");
builder.Services.AddKeyedScoped<IRepository<Board, long>, BoardRepository>("BoardRepository");
builder.Services.AddKeyedScoped<IRepository<State, long>, StateRepository>("StateRepository");
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
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}