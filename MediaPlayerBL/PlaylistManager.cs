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
        public Playlist CurrentPlaylist { get; set; } //Property storingthe current playlist


        /// <summary>
        /// Constructor, initializing a new playlist
        /// </summary>
        public PlaylistManager()
        {
            CurrentPlaylist = new Playlist();
        }

        /// <summary>
        /// Method for saving a playlist
        /// </summary>
        public void SavePlaylist(IMediaDA mediaDA,string filepath, ICollection<Media> currentMedia)
        {
            CurrentPlaylist.PlaylistName = Path.GetFileNameWithoutExtension(filepath); //Stores the playlist title
            mediaDA.SavePlaylist(filepath, CurrentPlaylist.PlaylistName, currentMedia); //Saves the playlist
        }

        /// <summary>
        /// A method for loading an existing playlist
        /// </summary>
        public void LoadPlaylist(IMediaDA mediaDA, string filepath)
        {
            CurrentPlaylist = mediaDA.LoadPlaylist(filepath); //Sets the playlist with the loaded one
            CurrentPlaylist.PlaylistName= Path.GetFileNameWithoutExtension(filepath); //Sets the playlist title
        }
    }
}
