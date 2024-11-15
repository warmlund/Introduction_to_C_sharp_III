using MediaPlayerDA;
using MediaDTO;

namespace MediaPlayerDA_Tests
{
    public class DatabaseManagerTest
    {
        private DatabaseManager databaseManager;

        [SetUp]
        public void Setup()
        {
            databaseManager = new DatabaseManager();
        }

        [TearDown]
        public void TearDown()
        {
            databaseManager.DbContext.Database.EnsureDeleted();
            databaseManager.DbContext.Dispose();
        }

        [Test]
        public void SavePlaylistToDbTest()
        {
            //Arrange
            string testTitle = "playlistTitle";

            ICollection<Media> testMedia = new List<Media>
            {
                new Media{FileName= "Cherry.jpg", FilePath= @"Sample_Data/Media/Images/Cherry.jpg",Format= ".jpg", PlaylistName=testTitle },
                new Media{FileName= "Forest.jpg", FilePath= @"Sample_Data/Media/Images/Forest.jpg",Format= ".jpg", PlaylistName=testTitle }
            };

            //Act
            bool result = databaseManager.SavePlaylistToDb(testTitle, testMedia);

            //Assert
            Assert.That(result, Is.True, "SavePlaylistToDb should return true");
        }

        [Test]
        public void LoadPlaylistFromDbTest()
        {
            //Arrange
            string testTitle = "Nature";

            //Act
            Playlist testPlaylist = databaseManager.LoadPlaylistFromDb(testTitle);

            //Assert
            Assert.IsNotNull(testPlaylist, "Playlist should exist in the database");
        }

        [Test]
        public void SaveMediaToDbTest()
        {
            //Arrange: Create media with invalid paths
            string testTitle = "playlistTitle";

            ICollection<Media> testMedia = new List<Media>
            {
                new Media{FileName= "Cherry.jpg", FilePath= @"Invalid_Path/Cherry.jpg",Format= ".jpg", PlaylistName=testTitle },
                new Media{FileName= "Forest.jpg", FilePath= @"Invalid_Path/Forest.jpg",Format= ".jpg", PlaylistName=testTitle }
            };

            //Act 
            var result = databaseManager.SaveMediaToDb(testMedia, testTitle);

            //Assert
            Assert.That(result, Is.False, "SaveMediaToDb should return false when saving media from invalid path");
        }
    }
}