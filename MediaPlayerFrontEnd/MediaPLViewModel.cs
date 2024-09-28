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
        private int _currentIndex;
        private double _progressValue;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private CancellationTokenSource _tokenSource;
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
        public double ProgressValue { get { return _progressValue; } set { if (_progressValue != value) { _progressValue = value; OnPropertyChanged(nameof(ProgressValue)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); } } }
        public bool IsImage { get { return _isImage; } set { if (_isImage != value) { _isImage = value; OnPropertyChanged(nameof(IsImage));} } }
        public bool IsVideo { get { return _isVideo; } set { if (_isVideo != value) { _isVideo = value; OnPropertyChanged(nameof(IsVideo)); } } }
        public Media CurrentPlayingMedia { get => _mediaBl.CurrentPlayingMedia; set { if (_mediaBl.CurrentPlayingMedia != value) { _mediaBl.CurrentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); CheckFormat(); } } }
        public string CurrentFormat { get => _mediaBl.CurrentFormat; set { if (_mediaBl.CurrentFormat != value) { _mediaBl.CurrentFormat = value; OnPropertyChanged(nameof(CurrentFormat)); } } }
        public BitmapImage CurrentImage { get => _image; set { if (_image != value) { _image = value; OnPropertyChanged(nameof(CurrentImage));  } } }
        public Uri CurrentVideo { get => _video; set { if (_video != value) { _video = value; OnPropertyChanged(nameof(CurrentVideo)); } } }
        #endregion

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            _interval = 5;
            _currentLoadedMedia = new ObservableCollection<Media>();
            _currentIndex = 0;

            Play = new AsyncCommand(TogglePlayPause, CanPlayMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadExistingPlaylist);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadNewMedia);
            
            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
            mediaBL.MediaChanged += (media) => CurrentPlayingMedia = media;
            //mediaBL.FormatChanged += (format) => CurrentFormat = format;
        }
        private bool CanPlayMedia() => CurrentLoadedMedia.Count > 0;

        private async Task TogglePlayPause()
        {
            _tokenSource = new CancellationTokenSource();
            if (IsPlaying)
            {
               await PlayMediaAsync(CurrentLoadedMedia.ToList());
            }
            else
            {
                PauseMedia();
            }
        }

        public async Task PlayMediaAsync(List<Media> loadedMedia)
        {
            try
            {
                for (int i = _currentIndex; i < loadedMedia.Count; i++)
                {
                    if (_tokenSource.Token.IsCancellationRequested)
                    {
                        if(i == loadedMedia.Count)
                            _currentIndex = 0;
                        else if(i == 0)
                            _currentIndex = 0;
                        _currentIndex = i-1;
                        break; 
                    }

                    CurrentPlayingMedia = loadedMedia[i];
                    CurrentFormat = CurrentPlayingMedia.Format;
                    CheckFormat();
                    ProgressValue = 0;

                    if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format))
                    {
                        CurrentVideo = _mediaBl.CreateVideo(CurrentPlayingMedia.FilePath);
                    }
                    else
                    {
                        CurrentImage = _mediaBl.CreateImage(CurrentPlayingMedia.FilePath);
                    }

                    for (int j = 0; j < Interval; j++)
                    {
                        if (_tokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        ProgressValue = (j + 1) * (100.0 / Interval);
                        await Task.Delay(1000);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Handle task cancellation
                CurrentPlayingMedia = null;
                CurrentImage = null;
                CurrentVideo = null;
                IsPlaying = false;
            }
            finally
            {
                IsPlaying = false; 
            }
        }


        public void PauseMedia()
        {
            _tokenSource.Cancel();
            IsPlaying = false;
        }

        private bool CanLoadExistingPlaylist() => true;
        private bool CanSaveNewPlaylist() => CurrentLoadedMedia != null;
        private bool CanLoadNewMedia() => true;
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
            if (CurrentPlayingMedia != null)
            {
                if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format))
                {
                    IsVideo = true;
                    IsImage = false;
                    OnPropertyChanged(nameof(IsVideo));
                    OnPropertyChanged(nameof(IsImage));
                    CurrentVideo = _mediaBl.CreateVideo(CurrentPlayingMedia.FilePath); 
                }
                else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.Format))
                {
                    IsVideo = false;
                    IsImage = true;
                    OnPropertyChanged(nameof(IsVideo));
                    OnPropertyChanged(nameof(IsImage));
                    CurrentImage = _mediaBl.CreateImage(CurrentPlayingMedia.FilePath);
                }
                else
                {
                    IsVideo = false;
                    IsImage = false;
                    CurrentImage = null;
                    CurrentVideo = null;
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLoadedMedia));
            Play.RaiseCanExecuteChanged();
        }
    }
}
