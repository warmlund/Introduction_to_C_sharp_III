using Microsoft.Win32;
using System.Windows;

namespace MediaPlayerPL
{
    internal class SaveManager : FileDialogManager
    {
        private SaveFileDialog _saveFile;

        public SaveManager()
        {
            _saveFile = new SaveFileDialog();
            Title = "Save Playlist";
        }

        public override bool ShowDialog()
        {
            _saveFile.Title = Title;
            _saveFile.Filter = PlaylistFilter;
            bool result = _saveFile.ShowDialog() == true;
            if (result)
            {
                FilePath = _saveFile.FileName;
            }
            return result;
        }

        public override void AlertUser()
        {
            MessageBox.Show("Failed to save playlist", "Home Mediaplayer", MessageBoxButton.OK);
        }
    }
}
