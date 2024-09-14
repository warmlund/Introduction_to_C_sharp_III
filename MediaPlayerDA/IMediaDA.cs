using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaStorage;

namespace MediaPlayerDA
{
    public interface IMediaDA
    {
        List<Media> LoadMedia(string[] filenames);
        Playlist LoadPlaylist(string path);
        bool SavePlaylist(Playlist playlist, string path);
    }
}
