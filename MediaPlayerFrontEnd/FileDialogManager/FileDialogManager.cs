namespace MediaPlayerPL
{

    public abstract class FileDialogManager : IFileDialogManager
    {
        public string Title { get; protected set; }
        public string FilePath { get; protected set; }
        public string Extension { get; protected set; }

        public string[] SelectedFiles { get; protected set; }

        public string PlaylistFilter { get; protected set; } = "JSON Files (*.json)|*.json";
        public string MediaFilter { get; protected set; } = "Media Files (*.mp4, *.wmv, *.avi, *.mpeg, *.mpg, *.asf, *.jpg, *.jpeg, *.png, *.bmp, *.gif, *.tiff, *.ico)|*.mp4;*.wmv;*.avi;*.mpeg;*.mpg;*.asf;*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.ico|"
                                  + "Video Files (*.mp4, *.wmv, *.avi, *.mpeg, *.mpg, *.asf)|*.mp4;*.wmv;*.avi;*.mpeg;*.mpg;*.asf|"
                                  + "Image Files (*.jpg, *.jpeg, *.png, *.bmp, *.gif, *.tiff, *.ico)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.ico";
        public abstract bool ShowDialog();

        public abstract void AlertUser();

    }
}
