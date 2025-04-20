using Hangfire.Dashboard.BasicAuthorization;

using Hangfire;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDependencies(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration)
);
// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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
}); app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
