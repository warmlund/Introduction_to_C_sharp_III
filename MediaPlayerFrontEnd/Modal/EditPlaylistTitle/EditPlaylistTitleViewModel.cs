namespace MediaPlayerPL
{
    public class EditPlaylistTitleViewModel : NotifyPropertyChanged
    {
        private string _playlistTitle;
        private bool _dialogResult;

        public Command AddPlaylist { get; private set; }
        public Command CancelAddPlaylist { get; }
        public string Title { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(Title)); AddPlaylist.RaiseCanExecuteChanged(); } } }
        public bool DialogResult { get { return _dialogResult; } set { if (_dialogResult != value) { _dialogResult = value; OnPropertyChanged(nameof(DialogResult)); } } }

        public Action Close { get; set; }

        public EditPlaylistTitleViewModel()
        {
            AddPlaylist = new Command(AddNewPlaylist, CanAddPlaylist);
            CancelAddPlaylist = new Command(CancelNewPlaylist, CanCancelAddPlaylist);
            Title = string.Empty;
        }

        private bool CanAddPlaylist()
        {
            if (Title.Length > 0)
                return true;
            return false;
        }

        private bool CanCancelAddPlaylist() => true;

        private void AddNewPlaylist()
        {
            DialogResult = true;
            Close?.Invoke();
        }

        private void CancelNewPlaylist()
        {
            DialogResult = false;
            Close?.Invoke();
        }
    }
}
