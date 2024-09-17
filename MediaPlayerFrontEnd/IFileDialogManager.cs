namespace MediaPlayerPL
{
    public interface IFileDialogManager
    {
        bool ShowDialog();
        string FilePath { get; }
        string Extension { get; }

        string Filter { get; }
    }
}
