using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDTO;

namespace MediaPlayerDA
{
    public interface IMediaDA
    {
        List<Media> LoadMedia(string[] filenames);
        Playlist LoadPlaylist(string path);
        bool SavePlaylist(string path, string title, List<Media> currentMedia);
    }
}
