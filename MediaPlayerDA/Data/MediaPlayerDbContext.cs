using MediaDTO;
using Microsoft.EntityFrameworkCore;

namespace MediaPlayerDA.Data;

public partial class MediaPlayerDbContext : DbContext
{
    public MediaPlayerDbContext()
    {
    }

    public MediaPlayerDbContext(DbContextOptions<MediaPlayerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Media> Media { get; set; }

    public virtual DbSet<Playlist> Playlist { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=media_player_db;Integrated Security=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the relationship between Media and Playlist
        modelBuilder.Entity<Media>()
            .HasOne(m => m.Playlist)
            .WithMany(p => p.MediaFiles)
            .HasForeignKey(m => m.PlaylistName)  // Media references PlaylistName in Playlist
            .HasPrincipalKey(p => p.PlaylistName); // PlaylistName is the principal key

        // Set Cascade Delete when a Playlist is deleted
        modelBuilder.Entity<Playlist>()
            .HasMany(p => p.MediaFiles)
            .WithOne(m => m.Playlist)
            .OnDelete(DeleteBehavior.SetNull); // Set foreign key to null on deletion

        base.OnModelCreating(modelBuilder);
    }

    public void InitializeDatabase()
    {
        Database.Migrate();
    }

}
