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
        List<Media> LoadMedia(string[] filenames);

        //Load playlist
        Playlist LoadPlaylist(string path);

        //Save playlist
        bool SavePlaylist(string path, string title, List<Media> currentMedia);
    }
}
