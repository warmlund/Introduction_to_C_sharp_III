using MediaPlayerBL;
using System.Windows.Media.Imaging;

namespace MediaPlayerBL_Tests
{
    [TestClass]
    public class MediaBLTest
    {
        private MediaBL mediaBL;

        [TestInitialize]
        public void Setup()
        {
            mediaBL = new MediaBL();
        }

        [TestMethod]
        public void CreateBitmapTest()
        {
            //Arrange
            string filePathTest = "Sample_Data/Media/Images/Cherry.jpg";

            //Act
            BitmapImage bitmapImageTest = mediaBL.CreateImage(filePathTest);

            //Assert
            Assert.IsNotNull(bitmapImageTest, "BitmapImage should not be null");
        }

        [TestMethod]
        public void IsImageFormatEmptyTest()
        {
            //Arrange
            string testEmptyFormat = string.Empty;

            //Act
            bool emptyResult = mediaBL.IsImageFormat(testEmptyFormat);

            //Assert
            Assert.IsFalse(emptyResult, "Expected to return false for empty string");
        }

        public void IsImageFormatValidTest()
        {
            //Arrange
            string[] testFormats = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };

            //Act
            foreach (string format in testFormats)
            {
                bool validFormatResult = mediaBL.IsImageFormat(format);

                //Assert
                Assert.IsTrue(validFormatResult, $"Expected true for valid image format {format}");
            }
        }
    }
}