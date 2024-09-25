namespace MediaPlayerPL
{
    public interface IFileDialogManager
    {
        bool ShowDialog();
        void AlertUser();
        string FilePath { get; }
        string Extension { get; }
        string PlaylistFilter { get; }
        string MediaFilter { get; }
        string[] SelectedFiles { get; }
    }
}
