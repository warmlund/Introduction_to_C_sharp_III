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
        // Define relationships, keys, etc.
        modelBuilder.Entity<Media>()
            .HasOne(m => m.Playlist) 
            .WithMany(p => p.MediaFiles)
            .HasForeignKey(m => m.PlaylistName) 
            .HasPrincipalKey(p => p.PlaylistName);
            
        base.OnModelCreating(modelBuilder);
    }

    public void InitializeDatabase()
    {
        Database.Migrate();
    }

}
