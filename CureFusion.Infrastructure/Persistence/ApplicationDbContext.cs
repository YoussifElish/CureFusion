﻿using System.Reflection;
using CureFusion.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CureFusion.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        var cascadeFKs = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(x => x.DeleteBehavior == DeleteBehavior.Cascade && !x.IsOwnership);
        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Questions)
            .WithOne(q => q.User)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);


    }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    public DbSet<HealthArticle> HealthArticles { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Drug> Drugs { get; set; }
    public DbSet<PatientAppointment> patientAppointments { get; set; }
    public DbSet<DrugReminder> DrugReminders { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<UploadedFile> UploadedFiles { get; set; }
    public DbSet<Subscription> subscriptions { get; set; }
    public DbSet<SubscriptionPlan> subscriptionPlans { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker.Entries<AuditableEntity>();
        foreach (var entityEntry in entries)
        {

            var currentUserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Value;
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedByID).CurrentValue = currentUserId;
            }

            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


}
