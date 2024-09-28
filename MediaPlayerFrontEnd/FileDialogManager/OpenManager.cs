using Microsoft.Win32;
using System.Windows;
using System.IO;

namespace MediaPlayerPL
{
    internal class OpenManager : FileDialogManager
    {
        private OpenFileDialog _openFile;
        public OpenManager(string title)
        {
            _openFile = new OpenFileDialog();
            Title = title;
        }

      public override bool ShowDialog()
        {
            _openFile.Title = Title;
           
            if(Title =="Load Playlist")
            {
                _openFile.Multiselect = false;
                _openFile.Filter = PlaylistFilter;
            }
                
            else
            {
                _openFile.Multiselect = true;
                _openFile.Filter = MediaFilter;
            }
               
            bool result = _openFile.ShowDialog() == true;

            if (result)
            {
                if(Title =="Load Playlist")
                    FilePath = _openFile.FileName;
                else
                {
                    SelectedFiles = _openFile.FileNames;
                }
            }
            return result;
        }

        public override void AlertUser()
        {
            MessageBox.Show("Failed to load","Home Media Player", MessageBoxButton.OK);
        }
    }
}