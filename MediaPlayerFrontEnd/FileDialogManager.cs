namespace MediaPlayerPL
{
    public abstract class FileDialogManager : IFileDialogManager
    {
        public string Title { get; protected set; }
        public string FilePath { get; protected set; }
        public string Extension { get; protected set; }

        public string Filter { get; protected set; } = "JSON Files (*.json)|*.json";
        public abstract bool ShowDialog();

        public abstract void AlertUser();

    }
}
