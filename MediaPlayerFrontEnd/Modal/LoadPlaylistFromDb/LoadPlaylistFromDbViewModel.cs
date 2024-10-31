using MediaDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayerPL
{
    public class LoadPlaylistFromDbViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<Playlist> _playlistFromDatabase;
        private Playlist _selectedPlaylist;
        private bool _dialogResult;

        public ObservableCollection<Playlist> PlaylistFromDatabase { get { return _playlistFromDatabase; } set { if (_playlistFromDatabase != value) { _playlistFromDatabase = value; OnPropertyChanged(nameof(PlaylistFromDatabase)); } } }
        public Playlist SelectedPlaylist { get { return _selectedPlaylist; } set { if (_selectedPlaylist != value) { _selectedPlaylist = value; OnPropertyChanged(nameof(SelectedPlaylist)); } } }
        public bool DialogResult { get { return _dialogResult; } set { if (_dialogResult != value) { _dialogResult = value; OnPropertyChanged(nameof(DialogResult)); } } }

        public Action Close { get; set; }
        public Command LoadPlaylist { get; private set; }
        public Command CancelLoadPlaylist { get; }

        public LoadPlaylistFromDbViewModel(ICollection<Playlist> playlistsFromDb)
        {
            PlaylistFromDatabase = new ObservableCollection<Playlist>();
            LoadPlaylist = new Command(LoadPlaylistFromDb, CanLoadPlaylist);
            CancelLoadPlaylist = new Command(CancelPlaylist, CanCancelPlaylist);

            foreach (var playlist in playlistsFromDb)
                PlaylistFromDatabase.Add(playlist);
        }

        private bool CanLoadPlaylist()
        {
            if (_playlistFromDatabase != null && SelectedPlaylist != null)
                return true;
            return false;
        }
        private bool CanCancelPlaylist() => true;

        private void LoadPlaylistFromDb()
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
