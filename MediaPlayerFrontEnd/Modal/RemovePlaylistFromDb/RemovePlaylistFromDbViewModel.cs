using MediaDTO;
using System.Collections.ObjectModel;

namespace MediaPlayerPL
{
    public class RemovePlaylistFromDbViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<Playlist> _playlistFromDatabase;
        private Playlist _selectedPlaylist;
        private bool _dialogResult;

        public ObservableCollection<Playlist> PlaylistFromDatabase { get { return _playlistFromDatabase; } set { if (_playlistFromDatabase != value) { _playlistFromDatabase = value; OnPropertyChanged(nameof(PlaylistFromDatabase)); } } }
        public Playlist SelectedPlaylist { get { return _selectedPlaylist; } set { if (_selectedPlaylist != value) { _selectedPlaylist = value; OnPropertyChanged(nameof(SelectedPlaylist)); } } }
        public bool DialogResult { get { return _dialogResult; } set { if (_dialogResult != value) { _dialogResult = value; OnPropertyChanged(nameof(DialogResult)); } } }

        public Action Close { get; set; }
        public Command DeletePlaylist { get; private set; }
        public Command CancelLoadPlaylist { get; }

        public RemovePlaylistFromDbViewModel(ICollection<Playlist> playlistsFromDb)
        {
            PlaylistFromDatabase = new ObservableCollection<Playlist>();
            DeletePlaylist = new Command(DeletePlaylistFromDb, CanDeletePlaylist);
            CancelLoadPlaylist = new Command(CancelPlaylist, CanCancelPlaylist);

            foreach (var playlist in playlistsFromDb)
                PlaylistFromDatabase.Add(playlist);
        }

        private bool CanDeletePlaylist()
        {
            if (SelectedPlaylist != null)
                return true;
            return false;
        }
        private bool CanCancelPlaylist() => true;

        private void DeletePlaylistFromDb()
        {
            DialogResult = true;
            Close?.Invoke();
        }

        private void CancelPlaylist()
        {
            DialogResult = false;
            Close?.Invoke();
        }
    }
}
