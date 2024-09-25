using MediaDTO;
using MediaPlayerBL;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MediaPlayerPL
{
    public class MediaPLViewModel : NotifyPropertyChanged
    {
        private int _interval;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private bool _isPlaying = false;
        private Media _currentPlayingMedia;

        public Command Play { get; private set; }
        public Command Pause { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command LoadMedia { get; private set; }

        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); } } }
        public Media CurrentPlayingMedia { get { return _mediaBl.CurrentPlayingMedia; } set { if (_mediaBl.CurrentPlayingMedia != value) { _mediaBl.CurrentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); } } }

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            Play = new Command(TogglePlayPause, CanPlayMedia);
            Pause = new Command(PauseMedia, CanPauseMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadExistingPlaylist);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadNewMedia);
            Interval = mediaBL.GetInterval();
            _currentLoadedMedia = new ObservableCollection<Media>();
            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
        }
        private bool CanPlayMedia() => CurrentLoadedMedia.Count > 0;
        private bool CanPauseMedia()
        {
            if (IsPlaying)
                return true;
            return false;
        }

        private void PauseMedia()
        {
            IsPlaying = false;
            _mediaBl.SetMediaPlayPause(IsPlaying);
        }

        private void TogglePlayPause()
        {
            IsPlaying = !IsPlaying;
            _mediaBl.SetMediaPlayPause(IsPlaying);

            if (IsPlaying)
            {
                _mediaBl.PlayMedia(CurrentLoadedMedia.ToList());
            }
            else
            {
                _mediaBl.PauseMedia();
            }
        }

        private bool CanLoadExistingPlaylist() => true;

        private void LoadExistingPlaylist()
        {
            var openManager = new OpenManager("Load Playlist");
            if (openManager.ShowDialog())
            {
                CurrentLoadedMedia.Clear();
                foreach (Media media in _mediaBl.LoadPlaylist(openManager.FilePath))
                {
                    CurrentLoadedMedia.Add(media);
                }

                PlaylistTitle = _mediaBl.GetPlaylistTitle();
            }

            else
            {
                openManager.AlertUser();
            }
        }

        private bool CanSaveNewPlaylist() => CurrentLoadedMedia != null;

        private void SaveNewPlaylist()
        {
            var saveManager = new SaveManager();
            if (saveManager.ShowDialog())
                _mediaBl.SavePlaylist(saveManager.FilePath, CurrentLoadedMedia.ToList());
            else
            {
                saveManager.AlertUser();
            }
        }

        private bool CanLoadNewMedia() => true;

        private void LoadNewMedia()
        {
            var openManager = new OpenManager("Load Media");
            if (openManager.ShowDialog())
            {
                _selectedFiles = openManager.SelectedFiles;
                foreach (Media m in _mediaBl.LoadMedia(_selectedFiles))
                {
                    CurrentLoadedMedia.Add(m);
                }
            }

            else
            {
                openManager.AlertUser();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLoadedMedia));
            Play.RaiseCanExecuteChanged();
            Pause.RaiseCanExecuteChanged();
        }
    }
}
