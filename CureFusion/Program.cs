﻿using CureFusion.API.Helpers;
using CureFusion.Application.Services;
using CureFusion.Domain.Common;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddScoped<PaymobService>();
builder.Services.AddHttpClient<PaymobService>();


builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration)
);
// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<WhatsAppConfigurations>(
    builder.Configuration.GetSection("WhatsAppConfigurations"));
builder.Services.Configure<WhatsAppConfigurations>(
    builder.Configuration.GetSection("TwilioSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMultipleOrigins", builder =>
    {
        builder
            .WithOrigins("http://localhost:4200", "https://midical2222.vercel.app", "https://midical2222333.vercel.app")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable Swagger in development
app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "CureFusion Dashboard",
    Authorization = new[]
    {
        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            SslRedirect = false,
            RequireSsl = false,
            LoginCaseSensitive = false,
            Users = new[]
            {
                new BasicAuthAuthorizationUser
                {
                    Login = builder.Configuration["HangFireSettings:Username"],
                    PasswordClear = builder.Configuration["HangFireSettings:Password"]
                }
            }
        })
    }
});
app.UseCors("AllowMultipleOrigins");
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
