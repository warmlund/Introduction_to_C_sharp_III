using MediaDTO;
using MediaPlayerDA;
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
        ICollection<Media> LoadMedia(string[] filenames, bool loadFromDb);

        //Loads playlist
        ICollection<Media> LoadPlaylist(string filepath, bool loadFromDb);

        //Creates image
        BitmapImage CreateImage(string filepath);

        //Creates video
        Uri CreateVideo(string filepath);

        //Saves playlist
        void SavePlaylist(string filepath, ICollection<Media> loadedMedia, bool saveToDb);

        //Gets playlist title
        string GetPlaylistTitle();

        void ChangePlaylistTitle(string newName, Playlist playlist, bool fromDb);

        //Checks image format
        bool IsImageFormat(string filepath);

        //Checks video format
        bool IsVideoFormat(string filepath);
    }
}
