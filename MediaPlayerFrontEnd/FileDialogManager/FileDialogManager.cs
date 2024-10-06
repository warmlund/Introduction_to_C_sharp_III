namespace MediaPlayerPL
{
    /// <summary>
    /// An abstract class for handling filedialogs
    /// Implements the IFileDialogManager interface
    /// </summary>
    public abstract class FileDialogManager : IFileDialogManager
    {
        //Properties 
        public string Title { get; protected set; }
        public string FilePath { get; protected set; }
        public string Extension { get; protected set; }
        public string[] SelectedFiles { get; protected set; }

        //Filters for what files should be able to be opened
        public string PlaylistFilter { get; protected set; } = "JSON Files (*.json)|*.json";
        public string MediaFilter { get; protected set; } = "Media Files (*.mp4, *.wmv, *.avi, *.mpeg, *.mpg, *.asf, *.jpg, *.jpeg, *.png, *.bmp, *.gif, *.tiff, *.ico)|*.mp4;*.wmv;*.avi;*.mpeg;*.mpg;*.asf;*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.ico|"
                                  + "Video Files (*.mp4, *.wmv, *.avi, *.mpeg, *.mpg, *.asf)|*.mp4;*.wmv;*.avi;*.mpeg;*.mpg;*.asf|"
                                  + "Image Files (*.jpg, *.jpeg, *.png, *.bmp, *.gif, *.tiff, *.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.ico";
        
        /// <summary>
        /// Shows file dialog, returns true if successful
        /// </summary>
        /// <returns></returns>
        public abstract bool ShowDialog();

        /// <summary>
        /// Alerts user if something went wrong
        /// </summary>
        public abstract void AlertUser();

    }
}
