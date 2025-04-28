using Hangfire.Dashboard.BasicAuthorization;

using Hangfire;
using Serilog;
using CureFusion.Helpers;
using CureFusion.Settings;


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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
         builder =>
         {
             builder.AllowAnyOrigin()
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
app.UseStaticFiles();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.UseMiddleware<SessionValidationMiddleware>();
app.MapControllers();

app.Run();
