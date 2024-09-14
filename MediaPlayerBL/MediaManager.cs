using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPlayerDA;
using MediaStorage;

namespace MediaPlayerBL
{
    internal class MediaManager
    {
        public List<Media> LoadedMedia {  get; set; }
        public int PlaySpeed {  get; set; }
        public Media CurrentMedia { get; set; }
        public string CurrentFormat { get; set; }   
        public bool IsPlaying { get; set; }

        private int currentIndex = 0;

        public MediaManager()
        {
            LoadedMedia = new List<Media>();   
        }

        public void LoadMedia(IMediaDA mediaDA, string[] filenames)
        {
            LoadedMedia = mediaDA.LoadMedia(filenames);
        }

        public void ArrangeMedia(List<Media> newSorting)
        {
            PauseMedia();
            LoadedMedia = newSorting;
        }

        public void PlayMedia()
        {
            while (IsPlaying)
            {
                for (int i = currentIndex; i < LoadedMedia.Count; i++)
                {
                    CurrentMedia = LoadedMedia[i];
                    CurrentFormat = CurrentMedia.Format;
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
