using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data;

public class SamuraiContext : DbContext
{
    public DbSet<Samurai> Samurais => Set<Samurai>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<Battle> Battles => Set<Battle>();
    public DbSet<SamuraiBattleStat> SamuraiBattleStats => Set<SamuraiBattleStat>();

    public SamuraiContext()
    {
        // // change the default tracking behavior at the context instance level.
        // // No tracking queries are useful when the results are used in a read-only scenario.
        // // If you don't need to update the entities retrieved from the database, then a no-tracking query should be used.
        // base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
    {
        // // change the default tracking behavior at the context instance level.
        // // No tracking queries are useful when the results are used in a read-only scenario.
        // // If you don't need to update the entities retrieved from the database, then a no-tracking query should be used.
        // base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SamuraiAppData;Trusted_Connection=True;")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name/*, DbLoggerCategory.Database.Transaction.Name*/ }, LogLevel.Information)
            .EnableSensitiveDataLogging(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Samurai>()
         .HasMany(s => s.Battles)
         .WithMany(b => b.Samurais)
         .UsingEntity<BattleSamurai>
          (bs => bs.HasOne<Battle>().WithMany(),
           bs => bs.HasOne<Samurai>().WithMany())
         .Property(bs => bs.DateJoined)
         .HasDefaultValueSql("getdate()");

        modelBuilder.Entity<Horse>().ToTable("Horses");
        modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
    }
}
