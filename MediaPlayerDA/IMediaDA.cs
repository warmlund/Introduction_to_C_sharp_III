using MediaDTO;
using System.Collections.Generic;

namespace MediaPlayerDA
{

    /// <summary>
    /// Interface for the Data Access layer
    /// </summary>
    public interface IMediaDA
    {
        //Load media
        ICollection<Media> LoadMedia(string[] filenames);

        //Load playlist
        Playlist LoadPlaylist(string path);

        //Save playlist
        bool SavePlaylist(string path, string title, ICollection<Media> currentMedia);

        ICollection<Media> LoadMediaFromDatabase();

        Playlist LoadPlaylistFromDatabase(string name);

        void SaveMediaToDatabase(ICollection<Media> currentMedia, string PlaylistTitle);

        void SavePlaylistToDatabase(string title, ICollection<Media> currentMedia);

        void RemoveMediaFromDatabase(ICollection<Media> media);

        void RemovePlaylistFromDatabase(string title);

        void ChangePlaylistTitle(string newName, Playlist playlist);

        void CreateNewPlaylist(Playlist playlist);

        bool IsPlaylistInDatabase(string name);
    }
}
