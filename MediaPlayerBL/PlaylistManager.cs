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
    /// 
    /// </summary>
    public class PlaylistManager
    {
        public Playlist CurrentPlaylist { get; set; }

        public PlaylistManager()
        {
            CurrentPlaylist = new Playlist();
        }
        public void SavePlaylist(IMediaDA mediaDA,string filepath, List<Media> currentMedia)
        {
            CurrentPlaylist.PlaylistName = Path.GetFileNameWithoutExtension(filepath);
            mediaDA.SavePlaylist(filepath, CurrentPlaylist.PlaylistName, currentMedia);
        }

        public void LoadPlaylist(IMediaDA mediaDA, string filepath)
        {
            CurrentPlaylist = mediaDA.LoadPlaylist(filepath);
            CurrentPlaylist.PlaylistName= Path.GetFileNameWithoutExtension(filepath);
        }
    }
}
