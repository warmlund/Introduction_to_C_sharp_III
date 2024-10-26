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

        void SaveMedia(ICollection<Media> media, string PlaylistTitle);

        void RemoveMedia(ICollection<Media> media);

        //Loads playlist
        ICollection<Media> LoadPlaylist(string filepath, bool loadFromDb);

        ICollection<Playlist> GetPlaylistFromDb();

        //Creates image
        BitmapImage CreateImage(string filepath);

        //Creates video
        Uri CreateVideo(string filepath);

        //Saves playlist
        void SavePlaylist(string title,string filepath, ICollection<Media> loadedMedia);

        void SavePlaylistToDatabase(string title, ICollection<Media> loadedMedia);

        //Gets playlist title
        string GetPlaylistTitle();

        void ChangePlaylistTitle(string newName, Playlist playlist);

        void CreateNewPlaylist(string name);

        void RemovePlaylist(string title);

        //Checks image format
        bool IsImageFormat(string filepath);

        //Checks video format
        bool IsVideoFormat(string filepath);

        bool IsPlaylistInDatabase(string name);

        Playlist GetCurrentPlaylist();
    }
}
