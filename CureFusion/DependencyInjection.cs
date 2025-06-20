﻿using CureFusion.API.Authentication.Filters;
using CureFusion.API.Mapping;
using CureFusion.Application.Authentication;
using CureFusion.Application.Interfaces;
using CureFusion.Application.Services;
using CureFusion.Domain.Common;
using CureFusion.Services;
using FluentValidation.AspNetCore;
using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CureFusion.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options => options.AddDefaultPolicy(
                         builder => builder
                         .WithOrigins("https://localhost:7086")
                         .AllowAnyHeader()
                         .AllowAnyMethod()));

        services.AddDatabaseConnection(config);
        services.AddAuthConfig(config);
        services.AddHttpContextAccessor();
        services.AddMapsterConfig();
        services.AddIdentityConfig();
        services.AddFluentValidationConfig();
        services.AddBackgroundJobsConfig(config);
        services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();  // تسجيل BackgroundJobClient

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.Configure<TwilioSettings>(
    config.GetSection("TwilioSettings"));
        services.AddTransient<ITwilioVoiceService, TwilioVoiceService>();

        services.AddScoped<IDrugReminderService, DrugReminderService>();
        services.AddScoped<IDrugService, DrugService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<IGeocodingService, GeocodingService>();
        services.AddScoped<IGeoapifyService, GeoapifyService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddOptions<TwilioSettings>()
            .BindConfiguration("TwilioSettings")
            .ValidateDataAnnotations()
           .ValidateOnStart();

        services.Configure<MailSettings>(config.GetSection(nameof(MailSettings)));
        services.Configure<GeocodingOptions>(config.GetSection("Geocoding")); 
        services.Configure<GeoapifyOptions>(config.GetSection("Geoapify"));
       
        return services;
    }


    // FluentValidation Configuration
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();

        return services;
    }
    private static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Not Found");
        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));

        return services;
    }


    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetAssembly(typeof(MappingConfigurations)));
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
           .ValidateOnStart();

        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
                    ValidIssuer = settings?.Issuer,
                    ValidAudience = settings?.Audience

                };
            });

        return services;
    }

    private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;

        });

        return services;
    }
}
