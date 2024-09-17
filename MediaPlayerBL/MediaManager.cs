using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPlayerDA;
using MediaDTO;

namespace MediaPlayerBL
{
    internal class MediaManager
    {
        public int PlaySpeed {  get; set; }
        public Media ShownMedia { get; set; }
        public string CurrentFormat { get; set; }   
        public bool IsPlaying { get; set; }

        private int currentIndex = 0;

        public MediaManager()
        {
            PlaySpeed = 5;
            IsPlaying = false;
            CurrentFormat = string.Empty;
            ShownMedia = null;
        }

        public List<Media> LoadMedia(IMediaDA mediaDA, string[] filenames)
        {
            return mediaDA.LoadMedia(filenames);
        }

        public void PlayMedia(List<Media> loadedMedia)
        {
            while (IsPlaying)
            {
                for (int i = currentIndex; i < loadedMedia.Count; i++)
                {
                    ShownMedia = loadedMedia[i];
                    CurrentFormat = ShownMedia.Format;
                    Thread.Sleep(PlaySpeed * 1000);
                }
            }
        }

        public void PauseMedia()
        {
            IsPlaying = false;
        }

        public void SetPlaySpeed(int speed)
        {
            PlaySpeed = speed;
        }
    }
}
