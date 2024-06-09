using GHE.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHE.InfraData.Data;

public class GheContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public GheContext()
    {
        Database.Migrate();
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var data = "GHE.db";
        var databasePath = Path.Combine(folderPath, data);

        optionsBuilder.UseSqlite($"Filename={databasePath}");
        Console.WriteLine(databasePath);
    }
}
