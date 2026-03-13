using Api.Services.UserManagement.Data.Impl;
using Api.Services.UserManagement.Data;
using Api.Services.UserManagement.Manager.Impl;
using Api.Services.UserManagement.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Api.Services.Infra.Cache;
using Api.Services.UserManagement.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx,cfg)=> {
    cfg.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext();
});

// Add services to the container.

builder.Services.AddApiVersioning();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1",
        Description = "API for managing applications users",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "subramanya.sw@gmail.com"
        }
    });
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddSingleton<Api.Services.Infra.Events.IEventProducer, Api.Services.Infra.Events.APIEventProducer>();

builder.Services.AddDbContext<Api.Services.DataAccess.Entities.UserManagement.UserManagementContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementDB") ?? 
            throw new InvalidOperationException("Connection string 'UserManagementDB' not found.")));
builder.Services.AddSingleton<ICacheProvider, RedisCacheProvider>();
builder.Services.AddTransient<IApplicationManager, ApplicationManager>();
builder.Services.AddTransient<IApplicationRepository, ApplicationRepository>();
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddHostedService<UserListener>();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API");
        c.RoutePrefix = string.Empty;
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
