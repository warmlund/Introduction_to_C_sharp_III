using MediaDTO;
using MediaPlayerBL;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace MediaPlayerPL
{
    public class MediaPLViewModel : NotifyPropertyChanged
    {
        private int _interval;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private string _playlistPath;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;

        public Command Play { get; private set; }
        public Command Pause { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command LoadMedia { get; private set; }

        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string PlaylistPath { get { return _playlistPath; } set { if (_playlistPath != value) { _playlistPath = value; OnPropertyChanged(nameof(PlaylistPath)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;

            Play = new Command(PlayMedia, CanPlayMedia);
            Pause = new Command(PauseMedia, CanPauseMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadExistingPlaylist);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadNewMedia);

            Interval = mediaBL.GetInterval();

        }
        private bool CanPlayMedia()
        {
            if (CurrentLoadedMedia != null)
                return true;
            return false;
        }

        private bool CanPauseMedia() => true;

        private void PauseMedia() => _mediaBl.PauseMedia();

        private void PlayMedia() => _mediaBl.PlayMedia(CurrentLoadedMedia.ToList());

        private bool CanLoadExistingPlaylist() => true;

        private void LoadExistingPlaylist()
        {
            CurrentLoadedMedia = new ObservableCollection<Media>(_mediaBl.LoadPlaylist(PlaylistPath));
            PlaylistTitle = _mediaBl.GetPlaylistTitle();
        }

        private bool CanSaveNewPlaylist() => CurrentLoadedMedia != null;

        private void SaveNewPlaylist()
        {
            _mediaBl.SavePlaylist(PlaylistPath, CurrentLoadedMedia.ToList());
        }

        private bool CanLoadNewMedia() => true;

        private void LoadNewMedia()
        {
            _mediaBl.LoadMedia(_selectedFiles);
        }
    }
}
