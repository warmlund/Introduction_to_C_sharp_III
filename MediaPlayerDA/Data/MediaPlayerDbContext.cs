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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=media_player_db;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
