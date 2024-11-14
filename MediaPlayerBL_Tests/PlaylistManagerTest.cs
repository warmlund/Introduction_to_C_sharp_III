using MediaDTO;
using MediaPlayerBL;
using MediaPlayerDA;
using System.IO;

namespace MediaPlayerBL_Tests
{
    [TestClass]
    public class PlaylistManagerTest
    {
        private PlaylistManager playlistManager;
        private MediaDA mediaDA;

        [TestInitialize]
        public void Setup()
        {
            playlistManager = new PlaylistManager();
            mediaDA = new MediaDA();
        }

        [TestMethod]
        public void SavePlaylistTest()
        {
            //Arrange
            string title = "TestTitle";
            string filePath = @"Sample_Data/testsavelist.json";

            ICollection<Media> testMedia = new List<Media>
            {
                new Media{FileName= "Cherry.jpg", FilePath= @"Sample_Data/Media/Images/Cherry.jpg",Format= ".jpg", PlaylistName=title },
                new Media{FileName= "Forest.jpg", FilePath= @"Sample_Data/Media/Images/Forest.jpg",Format= ".jpg", PlaylistName=title }
            };

            //Act
            bool result = playlistManager.SavePlaylist(title, mediaDA, filePath, testMedia);

            //Assert
            Assert.IsTrue(result, "Playlist should save to desktop");
        }

        [TestMethod]
        public void LoadPlaylistTest()
        {
            //Arrange
            string filePath = @"Sample_Data/testloadlist.json";

            //Act
            playlistManager.LoadPlaylist(mediaDA, filePath, false);

            //Assert
            Assert.IsNotNull(playlistManager.CurrentPlaylist, "Current playlist should not be null");

        }

        [TestCleanup]
        public void Cleanup()
        {
            string saveFilePath = @"Sample_Data/testsavelist.json";

            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
        }
    }
}
