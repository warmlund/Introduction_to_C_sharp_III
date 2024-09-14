using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaStorage;
using MediaPlayerDA;


namespace MediaPlayerBL
{
    /// <summary>
    /// This class stores the logic and manages the current playlist in the player.
    /// 
    /// </summary>
    public class PlaylistManager
    {
        public Playlist Playlist { get; set; }

        public PlaylistManager()
        {
            Playlist = new Playlist();
        }

        public bool SavePlaylist(IMediaDA mediaDA, string filepath, Playlist playlist)
        {
            return mediaDA.SavePlaylist(playlist, filepath);
        }

        public void LoadPlaylist(IMediaDA mediaDA, string filepath)
        {
            Playlist = mediaDA.LoadPlaylist(filepath);
        }
    }
}
