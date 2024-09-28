using MediaDTO;
using MediaPlayerBL;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MediaPlayerPL
{
    public class MediaPLViewModel : NotifyPropertyChanged
    {
        #region instance variables
        private int _interval;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private bool _isPlaying = false;
        private bool _isVideo = false;
        private bool _isImage = false;
        private BitmapImage _image;
        private Uri _video;
        public AsyncCommand Play { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command LoadMedia { get; private set; }
        #endregion

        #region Properties
        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); } } }
        public bool IsImage { get { return _isImage; } set { if (_isImage != value) { _isImage = value; OnPropertyChanged(nameof(IsImage)); MessageBox.Show($"image is {value}"); } } }
        public bool IsVideo { get { return _isVideo; } set { if (_isVideo != value) { _isVideo = value; OnPropertyChanged(nameof(IsVideo)); MessageBox.Show($"video is {value}"); } } }
        public Media CurrentPlayingMedia { get => _mediaBl.CurrentPlayingMedia; set { if (_mediaBl.CurrentPlayingMedia != value) { _mediaBl.CurrentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); } } }
        public string CurrentFormat { get => _mediaBl.CurrentFormat; set { if (_mediaBl.CurrentFormat != value) { _mediaBl.CurrentFormat = value; OnPropertyChanged(nameof(CurrentFormat)); MessageBox.Show("Property CurrentFormat changed"); } } }
        public BitmapImage CurrentImage { get => _image; set { if (_image != value) { _image = value; OnPropertyChanged(nameof(_image)); } } }
        public Uri CurrentVideo { get => _video; set { if (_video != value) { _video = value; OnPropertyChanged(nameof(_video)); } } }
        #endregion

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            Play = new AsyncCommand(TogglePlayPause, CanPlayMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadExistingPlaylist);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadNewMedia);
            Interval = mediaBL.PlaySpeed;
            _currentLoadedMedia = new ObservableCollection<Media>();

            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
            mediaBL.MediaChanged += (media) => CurrentPlayingMedia = media;
            mediaBL.FormatChanged += (format) => CurrentFormat = format;

        }
        private bool CanPlayMedia() => CurrentLoadedMedia.Count > 0;

        private async Task TogglePlayPause()
        {
            IsPlaying = !IsPlaying;
            _mediaBl.IsPlaying = IsPlaying;

            if (IsPlaying)
            {
                _mediaBl.ResetIndex();
                await _mediaBl.PlayMediaAsync(CurrentLoadedMedia.ToList());
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
                SetUpFirstLoadedMedia();
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
                SetUpFirstLoadedMedia();
            }

            else
            {
                openManager.AlertUser();
            }
        }

        private void SetUpFirstLoadedMedia()
        {
            CurrentPlayingMedia = CurrentLoadedMedia[0];
            CurrentFormat = CurrentPlayingMedia.Format;
            CheckFormat();
        }

        private void CheckFormat()
        {
            if (CurrentFormat != null)
            {
                if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.FilePath))
                {
                    IsVideo = true;
                    IsImage = false;
                    _video = _mediaBl.CreateVideo(CurrentPlayingMedia.FilePath);

                }

                else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.FilePath))
                {
                    IsVideo = false;
                    IsImage = true;
                    _image = _mediaBl.CreateImage(CurrentPlayingMedia.FilePath);
                }

                else
                {
                    return;
                }
            }

            else
            {
                return;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLoadedMedia));
            Play.RaiseCanExecuteChanged();
        }
    }
}
