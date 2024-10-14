using MediaDTO;
using MediaPlayerDA.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MediaPlayerDA
{
    public class DatabaseManager
    {
        MediaPlayerDbContext db;
        public DatabaseManager()
        {
            using (db = new MediaPlayerDbContext())
            {
                db.Database.Migrate();
                AddSampleDataToDb(db);
            }
        }

        private void AddSampleDataToDb(MediaPlayerDbContext db)
        {
            // Check if the playlist "Nature" already exists
            Playlist? nature = db.Playlist.FirstOrDefault(p => p.PlaylistName == "Nature");
            if (nature == null)
            {
                // If not, create the "Nature" playlist
                nature = new Playlist { PlaylistName = "Nature" };
                db.Playlist.Add(nature);
            }

            Playlist? animalNature = db.Playlist.FirstOrDefault(p => p.PlaylistName == "Animal and nature");
            if (animalNature == null)
            {

                animalNature = new Playlist { PlaylistName = "Animal and nature" };
                db.Playlist.Add(animalNature);
            }

            db.SaveChanges(); // Save playlists

            // Sample media files
            AddMediaIfNotExists(db, "Cherry.jpg", "/Sample_Data/Media/Images/Cherry.jpg", ".jpg", nature.PlaylistName);
            AddMediaIfNotExists(db, "Forest.jpg", "/Sample_Data/Media/Images/Forest.jpg", ".jpg", nature.PlaylistName);
            AddMediaIfNotExists(db, "Lake.jpeg", "/Sample_Data/Media/Images/Lake.jpeg", ".jpeg", nature.PlaylistName);
            AddMediaIfNotExists(db, "Railroad.jpeg", "/Sample_Data/Media/Images/Railroad.jpeg", ".jpeg", nature.PlaylistName);
            AddMediaIfNotExists(db, "River.jpg", "/Sample_Data/Media/Images/River.jpg", ".jpg", nature.PlaylistName);

            AddMediaIfNotExists(db, "Urban.jpg", "/Sample_Data/Media/Images/Urban.jpg", ".jpg", animalNature.PlaylistName);
            AddMediaIfNotExists(db, "Valley.jfif", "/Sample_Data/Media/Images/Valley.jfif", ".jfif", animalNature.PlaylistName);
            AddMediaIfNotExists(db, "Waterfall.jpg", "/Sample_Data/Media/Images/Waterfall.jpg", ".jpg", animalNature.PlaylistName);
            AddMediaIfNotExists(db, "Fox.MP4", "/Sample_Data/Media/Videos/Fox.MP4", ".MP4", nature.PlaylistName);
            AddMediaIfNotExists(db, "Owl.MP4", "/Sample_Data/Media/Videos/Owl.MP4", ".MP4", animalNature.PlaylistName);
            AddMediaIfNotExists(db, "Snow Leopard.MP4", "/Sample_Data/Media/Videos/Snow Leopard.MP4", ".MP4", animalNature.PlaylistName);

            db.SaveChanges(); // Save media files
        }

        private void AddMediaIfNotExists(MediaPlayerDbContext db, string fileName, string filePath, string format, string playlistName)
        {
            // Check if the media file already exists
            var existingMedia = db.Media.FirstOrDefault(m => m.FileName == fileName && m.PlaylistName == playlistName);

            if (existingMedia == null) // If not found, add it
            {
                Media newMedia = new Media
                {
                    FileName = fileName,
                    FilePath = filePath,
                    Format = format,
                    PlaylistName = playlistName
                };
                db.Media.Add(newMedia);
            }
        }

        public ICollection<Media> LoadMediaFromDb()
        {
            ICollection<Media> loadedMedia = db.Media.OrderBy(m => m.FileName).ToList();

            return loadedMedia;
        }

        public bool SaveMediaToDb(ICollection<Media> currentMedia)
        {
            try
            {
                foreach (var media in currentMedia)
                {
                    db.Media.Add(media);
                    db.SaveChanges();
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool SavePlaylistToDb(string title, ICollection<Media> currentMedia)
        {

            try
            {
                Playlist playlist = new Playlist
                {
                    PlaylistName = title
                };

                db.Playlist.Add(playlist);
                db.SaveChanges();

                foreach (var media in currentMedia)
                {
                    media.PlaylistName = title;
                    db.Media.Add(media);
                    db.SaveChanges();
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        internal Playlist LoadPlaylistFormDb(string name)
        {
            return db.Playlist.Where(p => p.PlaylistName.Equals(name)).FirstOrDefault();
        }
    }
}
