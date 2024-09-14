using System.Collections.Generic;
using MediaStorage;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private List<Media> loadedMedia;
        private PlaylistManager playlistManager;
        private int interval = 5;
        public List<Media> LoadMedia()
        {
            return loadedMedia = new List<Media>();
        }

        public void LoadPlaylist(string pathname)
        {

        }

        public void SavePlaylist(string pathname)
        {

        }

        public int SetInterval(int interval)
        {
            return interval;
        }

        public void ArrangeMedia()
        {

        }

        public void PlayMedia()
        {

        }

        public void StopMedia()
        {

        }

    }
}
