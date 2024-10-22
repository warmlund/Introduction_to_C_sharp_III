using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDTO;
using MediaPlayerDA;


namespace MediaPlayerBL
{
    /// <summary>
    /// This class stores the logic and manages the current playlist in the player.
    /// </summary>
    public class PlaylistManager
    {
        public Playlist CurrentPlaylist { get; set; } //Property storing the current playlist

        public PlaylistManager()
        {
            CurrentPlaylist = null;
        }
        /// <summary>
        /// Method for saving a playlist
        /// </summary>
        public void SavePlaylist(IMediaDA mediaDA,string filepath, ICollection<Media> currentMedia, bool saveToDb)
        {
            if (saveToDb)
            {
                mediaDA.SavePlaylistToDatabase(filepath, currentMedia);
            }

            else
            {
                CurrentPlaylist.PlaylistName = Path.GetFileNameWithoutExtension(filepath); //Stores the playlist title
                mediaDA.SavePlaylist(filepath, CurrentPlaylist.PlaylistName, currentMedia); //Saves the playlist
            }
        }

        /// <summary>
        /// A method for loading an existing playlist
        /// </summary>
        public void LoadPlaylist(IMediaDA mediaDA, string filepath, bool loadToDb)
        {
            if (loadToDb)
            {
                CurrentPlaylist = mediaDA.LoadPlaylistFromDatabase(filepath);
            }

            else
            {
                CurrentPlaylist = mediaDA.LoadPlaylist(filepath); //Sets the playlist with the loaded one
                CurrentPlaylist.PlaylistName = Path.GetFileNameWithoutExtension(filepath); //Sets the playlist title
            }
        }

        public void ChangePlaylistTitle(IMediaDA mediaDA, string title, Playlist playlist)
        {

            if(mediaDA.IsPlaylistInDatabase(title))
                mediaDA.ChangePlaylistTitle(title, playlist);

            else
                playlist.PlaylistName = title;
        }

        public void CreateNewPlaylist(IMediaDA mediaDA,string title)
        {
            CurrentPlaylist =new Playlist { PlaylistName = title };
            mediaDA.CreateNewPlaylist(CurrentPlaylist);
        }

        public void RemoveCurrentPlaylist(IMediaDA mediaDA, string title)
        {
            mediaDA.RemovePlaylistFromDatabase(title);
        }

        internal bool IsPlaylistInDatabase(MediaDA mediaDA, string name)
        {
            return mediaDA.IsPlaylistInDatabase(name);
        }
    }
}
