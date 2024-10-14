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

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Media>().HasData(
               new Media
               {
                   MediaId = 1,
                   FileName = "Cherry.jpg",
                   FilePath = "/Sample_Data/Media/Images/Cherry.jpg",
                   Format = ".jpg",
                   PlaylistId = 1
               },
              new Media
              {
                  MediaId = 2,
                  FileName = "Forest.jpg",
                  FilePath = "/Sample_Data/Media/Images/Forest.jpg",
                  Format = ".jpg",
                  PlaylistId = 1
              },
              new Media
              {
                  MediaId = 3,
                  FileName = "Lake.jpeg",
                  FilePath = "/Sample_Data/Media/Images/Lake.jpeg",
                  Format = ".jpeg",
                  PlaylistId = 1
              },
               new Media
               {
                   MediaId = 4,
                   FileName = "Railroad.jpeg",
                   FilePath = "/Sample_Data/Media/Images/Railroad.jpeg",
                   Format = ".jpeg",
                   PlaylistId = 1
               },
                new Media
                {
                    MediaId = 5,
                    FileName = "River.jpg",
                    FilePath = "/Sample_Data/Media/Images/River.jpg",
                    Format = ".jpg",
                    PlaylistId = 1
                },
                 new Media
                 {
                     MediaId = 6,
                     FileName = "Urban.jpg",
                     FilePath = "/Sample_Data/Media/Images/Urban.jpg",
                     Format = ".jpg",
                     PlaylistId = 2
                 },
                                  new Media
                                  {
                                      MediaId = 7,
                                      FileName = "Valley.jfif",
                                      FilePath = "/Sample_Data/Media/Images/Valley.jfif",
                                      Format = ".jfif",
                                      PlaylistId = 2
                                  },
                                                   new Media
                                                   {
                                                       MediaId = 8,
                                                       FileName = "Waterfall.jpg",
                                                       FilePath = "/Sample_Data/Media/Images/Waterfall.jpg",
                                                       Format = ".jpg",
                                                       PlaylistId = 2
                                                   },
              new Media
              {
                  MediaId = 9,
                  FileName = "Fox.MP4",
                  FilePath = "/Sample_Data/Media/Videos/Fox.MP4",
                  Format = ".MP4",
                  PlaylistId = 1
              },
               new Media
               {
                   MediaId = 10,
                   FileName = "Owl.MP4",
                   FilePath = "/Sample_Data/Media/Videos/Owl.MP4",
                   Format = ".MP4",
                   PlaylistId = 2
               },
                new Media
                {
                    MediaId = 11,
                    FileName = "Snow Leopard.MP4",
                    FilePath = "/Sample_Data/Media/Videos/Snow Leopard.MP4",
                    Format = ".MP4",
                    PlaylistId = 2
                }
           );

        modelBuilder.Entity<Playlist>().HasData(
            new Playlist
            {
                PlaylistId = 1,
                PlaylistName = "Nature"
            },
            new Playlist
            {
                PlaylistId = 2,
                PlaylistName = "Animal and nature"
            }
            );
    }
}
