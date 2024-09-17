using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayerPL
{
    internal class SaveManager:FileDialogManager
    {
        private SaveFileDialog _saveFile;

        public SaveManager()
        {
            _saveFile = new SaveFileDialog();
            Title = "Save Playlsit";
        }

        public override bool ShowDialog()
        {
            _saveFile.Title = Title;
            _saveFile.Filter = Filter;
            bool result = _saveFile.ShowDialog() == true;
            if (result)
            {
                FilePath = _saveFile.FileName;
            }
            return result;
        }

    }
}
