using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayerBL
{
    public interface IMediaBL
    {
        List<Media> LoadMedia();
        int SetInterval(int interval);
        void ArrangeMedia();
        void LoadPlaylist(string pathname);
        void SavePlaylist(string pathname);
        void PlayMedia();
        void StopMedia();
    }
}
