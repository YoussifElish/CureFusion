namespace CureFusion.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor ) : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        var cascadeFKs = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(x=> x.DeleteBehavior == DeleteBehavior.Cascade && !x.IsOwnership);
        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        base.OnModelCreating(builder);
    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<HealthArticle> HealthArticles { get; set; } 
    public DbSet<Appointment> Appointments { get; set; } 
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        foreach (var entityEntry in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Value;

            if(entityEntry.State == EntityState.Added)
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
