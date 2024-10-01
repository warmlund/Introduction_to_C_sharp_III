using Microsoft.Win32;
using System.Windows;

namespace MediaPlayerPL
{
    /// <summary>
    /// A class implementing the abstract class FileDialogManager
    /// Used for saving files or playlists
    /// </summary>
    internal class SaveManager : FileDialogManager
    {
        private SaveFileDialog _saveFile;

        /// <summary>
        /// Constructor for SaveManager
        /// </summary>
        public SaveManager()
        {
            _saveFile = new SaveFileDialog();
            Title = "Save Playlist";
        }

        /// <summary>
        /// Displays the save dialog for saving a playlist
        /// </summary>
        public override bool ShowDialog()
        {
            _saveFile.Title = Title; //sets the title
            _saveFile.Filter = PlaylistFilter; //sets the filter to the playlist filter

            bool result = _saveFile.ShowDialog() == true; //shows the dialog
            if (result) //if successfull
            {
                FilePath = _saveFile.FileName; //stores the filepath
            }
            return result;
        }

        public override void AlertUser()
        {
            MessageBox.Show("Failed to save playlist", "Home Mediaplayer", MessageBoxButton.OK); //alerts user if something goes wrong
        }
    }
}
