using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaPlayerDA;
using MediaStorage;

namespace MediaPlayerBL
{
    public interface IMediaBL
    {
        void LoadMedia(IMediaDA mediaDA, string[] filenames);
        void SetInterval(int interval);
        void ArrangeMedia(List<Media> NewSorting);
        void LoadPlaylist(IMediaDA mediaDA, string filepath);
        bool SavePlaylist(IMediaDA mediaDA, string filepath, Playlist playlist);
        void PlayMedia();
        void PauseMedia();
    }
}
