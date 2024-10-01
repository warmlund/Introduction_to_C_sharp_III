namespace MediaPlayerPL
{

    /// <summary>
    /// Interface for filedialog management
    /// </summary>
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
