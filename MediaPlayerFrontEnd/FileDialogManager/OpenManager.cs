using Microsoft.Win32;
using System.Windows;

namespace MediaPlayerPL
{
    internal class OpenManager : FileDialogManager
    {
        private OpenFileDialog _openFile;
        public OpenManager()
        {
            _openFile = new OpenFileDialog();
            Title = "Open Playlist";
        }

      public override bool ShowDialog()
        {
            _openFile.Title = Title;
            _openFile.Filter = Filter;
            bool result = _openFile.ShowDialog() == true;
            if (result)
            {
                FilePath = _openFile.FileName;
            }
            return result;
        }

        public override void AlertUser()
        {
            MessageBox.Show("Home Mediaplayer", "Failed to load", MessageBoxButton.OK);
        }
    }
}