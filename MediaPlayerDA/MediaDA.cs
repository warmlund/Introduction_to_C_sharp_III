using MediaStorage;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MediaPlayerDA
{
    internal class MediaDA : IMediaDA
    {

        public List<Media> LoadMedia(string[] filenames)
        {
            var loadedMedia = new List<Media>();

            for (int i = 0; i < filenames.Length; i++)
            {
                loadedMedia.Add(new Media
                {
                    FileName = Path.GetFileName(filenames[i]),
                    FilePath = filenames[i],
                    Format = Path.GetExtension(filenames[i])
                });
            }

            return loadedMedia;
        }

        public Playlist LoadPlaylist(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string jsonPlaylist = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<Playlist>(jsonPlaylist);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool SavePlaylist(Playlist playlist, string path)
        {
            try
            {
                string jsonPlaylist = JsonConvert.SerializeObject(playlist, Formatting.Indented);
                File.WriteAllText(path, jsonPlaylist);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
