using MediaDTO;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MediaPlayerBL
{
    /// <summary>
    /// Interface representing the business logic
    /// </summary>
    public interface IMediaBL
    {
        //Loads media
        List<Media> LoadMedia(string[] filenames);

        //Loads playlist
        List<Media> LoadPlaylist(string filepath);

        //Creates image
        BitmapImage CreateImage(string filepath);

        //Creates video
        Uri CreateVideo(string filepath);

        //Saves playlist
        void SavePlaylist(string filepath, List<Media> loadedMedia);

        //Gets playlist title
        string GetPlaylistTitle();

        //Checks image format
        bool IsImageFormat(string filepath);

        //Checks video format
        bool IsVideoFormat(string filepath);
    }
}
