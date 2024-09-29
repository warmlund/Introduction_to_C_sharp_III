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
        private string _currentFormat;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private CancellationTokenSource _tokenSource;
        private Media _currentPlayingMedia;
        private bool _isPlaying = false;
        private bool _isVideo = false;
        private bool _isImage = false;
        private bool _isIndexChanged = false;
        private BitmapImage _image;
        private Uri _video;
        #endregion

        #region commands
        public AsyncCommand Play { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command LoadMedia { get; private set; }
        public CommandWithParameter<Media> MoveMediaUp { get; private set; }
        public CommandWithParameter<Media> MoveMediaDown { get; private set; }
        #endregion

        #region Properties
        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public int CurrentIndex { get { return _currentIndex; } set { if (_currentIndex != value) { _currentIndex = value; OnPropertyChanged(nameof(CurrentIndex));} } }
        public double ProgressValue { get { return _progressValue; } set { if (_progressValue != value) { _progressValue = value; OnPropertyChanged(nameof(ProgressValue)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); } } }
        public bool IsImage { get { return _isImage; } set { if (_isImage != value) { _isImage = value; OnPropertyChanged(nameof(IsImage));} } }
        public bool IsVideo { get { return _isVideo; } set { if (_isVideo != value) { _isVideo = value; OnPropertyChanged(nameof(IsVideo)); } } }
        public bool IsIndexChanged { get { return _isIndexChanged; } set { if (_isIndexChanged != value) { _isIndexChanged = value; OnPropertyChanged(nameof(IsIndexChanged));} } }
        public Media CurrentPlayingMedia { get => _currentPlayingMedia; set { if (_currentPlayingMedia != value) { _currentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); CheckFormat(); } } }
        public string CurrentFormat { get => _currentFormat; set { if (_currentFormat != value) { _currentFormat = value; OnPropertyChanged(nameof(CurrentFormat)); } } }
        public BitmapImage CurrentImage { get => _image; set { if (_image != value) { _image = value; OnPropertyChanged(nameof(CurrentImage));  } } }
        public Uri CurrentVideo { get => _video; set { if (_video != value) { _video = value; OnPropertyChanged(nameof(CurrentVideo)); } } }
        #endregion

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            _interval = 5;
            _currentLoadedMedia = new ObservableCollection<Media>();
            _currentIndex = 0;
            _tokenSource = new CancellationTokenSource();
            Play = new AsyncCommand(TogglePlayPause, CanPlayMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadExistingPlaylist);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadNewMedia);
            MoveMediaUp = new CommandWithParameter<Media>(MoveUp, CanMoveUp);
            MoveMediaDown = new CommandWithParameter<Media>(MoveDown, CanMoveDown);
            
            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
        }

        #region booleans
        private bool CanMoveDown(Media media)
        {
            return CurrentLoadedMedia.IndexOf(media) != CurrentLoadedMedia.IndexOf(CurrentLoadedMedia.Last());
        }
        private bool CanMoveUp(Media media)
        {
            return CurrentLoadedMedia.IndexOf(media)>0;
        }
        
        private bool CanLoadExistingPlaylist() => true;
        private bool CanSaveNewPlaylist() => CurrentLoadedMedia != null;
        private bool CanLoadNewMedia() => true;
        #endregion

        private async Task TogglePlayPause()
        {
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
                        if (i == loadedMedia.Count)
                            _currentIndex = 0;
                        else if (i == 0)
                            _currentIndex = 0;
                        _currentIndex = i - 1;
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
        private void MoveUp(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index > 0)
                SwapMedia(index, index - 1);
        }
        private void MoveDown(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index < CurrentLoadedMedia.Count - 1)
                SwapMedia(index, index + 1);
        }
        private void SwapMedia(int oldIndex, int newIndex)
        {
            var tempMedia = CurrentLoadedMedia[oldIndex];
            CurrentLoadedMedia[oldIndex] = CurrentLoadedMedia[newIndex];
            CurrentLoadedMedia[newIndex] = tempMedia;
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
