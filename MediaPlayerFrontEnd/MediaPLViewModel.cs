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
        // Private instance variables
        private int _interval;
        private double _videoProgress;
        private double _duration;
        private int _currentProgress;
        private double _progressValue;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private string _currentFormat;
        private string _selectedPlaylist;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private CancellationTokenSource _tokenSource;
        private TaskCompletionSource<bool> _mediaOpenedTcs;
        private Media _currentPlayingMedia;
        private ObservableCollection<Media> _selectedMedia;
        private EditPlaylistTitleModal _editPlaylistTitleModal;
        private AddMediaFromDbModal _addMediaFromDbView;
        private bool _isPlaying = false;
        private bool _isVideo = false;
        private bool _isImage = false;
        private bool _isIndexChanged = false;
        private bool _isEditing = false;
        private bool _isAdding = false;
        private BitmapImage _image;
        private Uri _video;
        public EventHandler PlayRequested;
        public EventHandler PauseRequested;
        public EventHandler StopRequested;
        #endregion

        #region commands
        //Commands for various commands called from the view
        public AsyncCommand Play { get; private set; }
        public Command CreatePlaylist { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command LoadPlaylistFromDb { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command SavePlaylistToDb { get; private set; }
        public Command RemovePlaylistFromDb { get; private set; }
        public Command ChangePlaylistTitle { get; private set; }
        public Command LoadMedia { get; private set; }
        public Command LoadMediaFromDb { get; private set; }
        public Command SaveMediaToDb { get; private set; }
        public Command RemoveMediaFromDb { get; private set; }
        public CommandWithParameter<Media> MoveMediaUp { get; private set; }
        public CommandWithParameter<Media> MoveMediaDown { get; private set; }
        #endregion

        #region Properties
        // Properties for data binding to the view
        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public double VideoDuration { get { return _duration; } set { if (_duration != value) { _duration = value; OnPropertyChanged(nameof(VideoDuration)); } } }
        public double VideoProgress { get { return _videoProgress; } set { if (_videoProgress != value) { _videoProgress = value; OnPropertyChanged(nameof(VideoProgress)); } } }
        public double ProgressValue { get { return _progressValue; } set { if (_progressValue != value) { _progressValue = value; OnPropertyChanged(nameof(ProgressValue)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); SavePlaylist.RaiseCanExecuteChanged(); LoadMedia.RaiseCanExecuteChanged(); LoadPlaylist.RaiseCanExecuteChanged(); LoadMediaFromDb.RaiseCanExecuteChanged(); RemovePlaylistFromDb.RaiseCanExecuteChanged(); ChangePlaylistTitle.RaiseCanExecuteChanged(); } } }
        public string SelectedPlaylist { get { return _selectedPlaylist; } set { if (_selectedPlaylist != value) { _selectedPlaylist = value; OnPropertyChanged(nameof(SelectedPlaylist)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); SavePlaylist.RaiseCanExecuteChanged(); LoadMedia.RaiseCanExecuteChanged(); LoadMediaFromDb.RaiseCanExecuteChanged(); LoadPlaylist.RaiseCanExecuteChanged(); ChangePlaylistTitle.RaiseCanExecuteChanged(); } } }
        public bool IsImage { get { return _isImage; } set { if (_isImage != value) { _isImage = value; OnPropertyChanged(nameof(IsImage)); } } }
        public bool IsVideo { get { return _isVideo; } set { if (_isVideo != value) { _isVideo = value; OnPropertyChanged(nameof(IsVideo)); } } }
        public bool IsIndexChanged { get { return _isIndexChanged; } set { if (_isIndexChanged != value) { _isIndexChanged = value; OnPropertyChanged(nameof(IsIndexChanged)); } } }
        public bool IsAdding { get { return _isAdding; } set { if (_isAdding != value) { _isAdding = value; OnPropertyChanged(nameof(IsAdding)); } } }
        public bool IsEditing { get { return _isEditing; } set { if (_isEditing != value) { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); } } }
        public Media CurrentPlayingMedia { get => _currentPlayingMedia; set { if (_currentPlayingMedia != value) { _currentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); CheckFormatAndSetCurrentMedia(); } } }
        public ObservableCollection<Media> SelectedMedia { get => _selectedMedia; set { if (_selectedMedia != value) { _selectedMedia = value; OnPropertyChanged(nameof(SelectedMedia)); } } }
        public BitmapImage CurrentImage { get => _image; set { if (_image != value) { _image = value; OnPropertyChanged(nameof(CurrentImage)); } } }
        public Uri CurrentVideo { get => _video; set { if (_video != value) { _video = value; OnPropertyChanged(nameof(CurrentVideo)); } } }
        public TaskCompletionSource<bool> TaskComplete { get => _mediaOpenedTcs; set { if (_mediaOpenedTcs != value) { _mediaOpenedTcs = value; OnPropertyChanged(nameof(TaskComplete)); } } }
        #endregion

        /// <summary>
        /// Constructor that initializes the view model with a media business logic instance
        /// </summary>
        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            _interval = 5;
            _currentProgress = 0;
            _currentLoadedMedia = new ObservableCollection<Media>();
            _selectedMedia = new ObservableCollection<Media>();
            _isAdding = false;
            _isEditing = false;
            _isPlaying = false;
            _playlistTitle = string.Empty;
            Play = new AsyncCommand(TogglePlayPause, CanPlayMedia);
            CreatePlaylist = new Command(CreateNewPlaylist, CanCreateNewPlaylist);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadPlaylist);
            LoadPlaylistFromDb = new Command(LoadPlaylistFromDatabase, CanLoadPlaylistFromDatabase);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            SavePlaylistToDb = new Command(SavePlaylistToDatabase, CanSavePlaylistToDatabase);
            RemovePlaylistFromDb = new Command(RemovePlaylistFromDatabase, CanRemovePlaylistFromDatabase);
            ChangePlaylistTitle = new Command(EditPlaylistTitle, CanEditPlaylistTitle);
            LoadMedia = new Command(LoadNewMedia, CanLoadOrSave);
            LoadMediaFromDb = new Command(LoadNewMediaFromDatabase, CanLoadNewMediaFromDatabase);
            SaveMediaToDb = new Command(SaveMediaToDatabase, CanSaveOrRemoveMediaToDatabase);
            RemoveMediaFromDb = new Command(RemoveMediaFromDatabase, CanSaveOrRemoveMediaToDatabase);
            MoveMediaUp = new CommandWithParameter<Media>(MoveUp, CanMoveUp);
            MoveMediaDown = new CommandWithParameter<Media>(MoveDown, CanMoveDown);
            ChangePlaylistTitle.RaiseCanExecuteChanged();
            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
        }

        #region booleans
        //Helper methods that checks if a command can be executed or not
        private bool CanMoveDown(Media media)
        {
            if (CurrentLoadedMedia.IndexOf(media) != CurrentLoadedMedia.IndexOf(CurrentLoadedMedia.Last()) && IsPlaying == false)
                return true;
            return false;
        }
        private bool CanMoveUp(Media media)
        {
            if (CurrentLoadedMedia.IndexOf(media) > 0 && IsPlaying == false)
                return true;
            return false;
        }
        private bool CanPlayMedia() => CurrentLoadedMedia.Count > 0;
        private bool CanSaveNewPlaylist()
        {
            if (_currentLoadedMedia.Count > 0 && IsPlaying == false)
                return true;

            return false;
        }
        private bool CanLoadOrSave()
        {
            if (IsPlaying == false && PlaylistTitle!=string.Empty)
                return true;
            return false;
        }
        private bool CanLoadPlaylist()
        {
            if (IsPlaying == false)
                return true;
            return false;
        }
        private bool CanLoadPlaylistFromDatabase()
        {
            if (IsPlaying == false)
                return true;
            return false;
        }
        private bool CanCreateNewPlaylist()
        {
            if (_mediaBl.GetCurrentPlaylist() == null)
                return true;
            return false;
        }
        private bool CanSavePlaylistToDatabase()
        {
            if (_currentLoadedMedia.Count > 0 && PlaylistTitle != null)
                return true;
            return false;
        }
        private bool CanRemovePlaylistFromDatabase()
        {
            if (_mediaBl.IsPlaylistInDatabase(PlaylistTitle))
                return true;
            return false;
        }
        private bool CanEditPlaylistTitle()
        {
            if (PlaylistTitle.Length > 0)
                return true;
            return false;
        }
        private bool CanLoadNewMediaFromDatabase()
        {
            if (PlaylistTitle != string.Empty)
                return true;
            return false;
        }
        private bool CanSaveOrRemoveMediaToDatabase()
        {
            if (SelectedMedia.Count > 0)
                return true;
            return false;
        }
        #endregion
        private void RemoveMediaFromDatabase()
        {
            _mediaBl.RemoveMedia(SelectedMedia);
        }
        private void SaveMediaToDatabase()
        {
            _mediaBl.SaveMedia(SelectedMedia, PlaylistTitle);
        }
        private void LoadNewMediaFromDatabase()
        {
            var mediaInDb = _mediaBl.LoadMedia(null, true);
            var addMediaFromDbViewModel = new AddMediaFromDbViewModel(mediaInDb);

            _addMediaFromDbView = new AddMediaFromDbModal
            {
                DataContext = addMediaFromDbViewModel
            };

            var result = _addMediaFromDbView.ShowDialog();

            if(result == true)
            {
                foreach (Media media in addMediaFromDbViewModel.SelectedMedia)
                    CurrentLoadedMedia.Add(media);

                SetUpFirstLoadedMedia();
            }
        }

        private void CreateNewPlaylist()
        {
            _isAdding = true;
            ShowModalEditPlaylist();
        }

        private void EditPlaylistTitle()
        {
            _isAdding = false;
            ShowModalEditPlaylist();
        }

        private void ShowModalEditPlaylist()
        {
            var editPlaylistTitleViewModel = new EditPlaylistTitleViewModel();
            _editPlaylistTitleModal = new EditPlaylistTitleModal
            {
                DataContext = editPlaylistTitleViewModel
            };

            var result = _editPlaylistTitleModal.ShowDialog();

            if (result == true)
            {
                if (IsAdding)
                {
                    PlaylistTitle = editPlaylistTitleViewModel.Title;
                    _mediaBl.CreateNewPlaylist(PlaylistTitle);
                }
                else
                {
                    PlaylistTitle = editPlaylistTitleViewModel.Title;
                    OnPropertyChanged(nameof(PlaylistTitle));
                    _mediaBl.ChangePlaylistTitle(PlaylistTitle, _mediaBl.GetCurrentPlaylist());
                }
            }
            else
            {
                PlaylistTitle = string.Empty;
            }
        }

        private void RemovePlaylistFromDatabase()
        {
            _mediaBl.RemovePlaylist(PlaylistTitle);
        }

        private void SavePlaylistToDatabase()
        {
            _mediaBl.SavePlaylist(PlaylistTitle, CurrentLoadedMedia, true);
        }

        private void LoadPlaylistFromDatabase()
        {
            _mediaBl.LoadPlaylist(SelectedPlaylist, true);
        }

        /// <summary>
        /// Method for toggling the play pause functionality
        /// </summary>
        /// <returns></returns>
        private async Task TogglePlayPause()
        {
            if (IsPlaying) //Checks if playing
            {
                _tokenSource = new CancellationTokenSource(); //Initialize cancellation token

                if (this.PlayRequested != null)
                {
                    this.PlayRequested(this, EventArgs.Empty); //Trigger playrequested event
                }
                await PlayMediaAsync(CurrentLoadedMedia.ToList()); //Calls the method for playing to start
            }

            else
            {
                if (this.PauseRequested != null)
                {
                    this.PauseRequested(this, EventArgs.Empty); //Trigger pauserequested
                }

                IsPlaying = false; //Sets Isplaying to false
                _tokenSource?.Cancel(); //cancel the ongoing play
            }
        }

        /// <summary>
        /// Asynchronous method to play the loaded media
        /// Making it async to not block the ui thread
        /// </summary>
        public async Task PlayMediaAsync(List<Media> loadedMedia)
        {
            try
            {
                for (int i = CurrentLoadedMedia.IndexOf(CurrentPlayingMedia); i < loadedMedia.Count; i++) //loop through the media of loaded media
                {
                    if (_tokenSource.Token.IsCancellationRequested) //checks if cancel is requested
                    {
                        ResetIndexWhenPaused(i); //resets index so when playing again it starts from the correct position
                        break;
                    }

                    CurrentPlayingMedia = loadedMedia[i]; //Sets currentlyplayingmedia

                    if (CurrentPlayingMedia.Format == "video" && VideoProgress == 0) // If the video was paused, use the existing VideoProgress value, else start from 0
                    {
                        ProgressValue = 0;
                    }


                    if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format)) //Checks if the current media is a video
                    {

                        if (!_mediaOpenedTcs?.Task.IsCompleted ?? true) // Ensure the video is opened only once, avoiding re-awaiting the TaskCompletionSource
                        {
                            _mediaOpenedTcs = new TaskCompletionSource<bool>();
                            await _mediaOpenedTcs.Task; // Wait for the video to be ready and sets the video duration
                        }

                        for (int j = (int)VideoProgress; j < VideoDuration; j++) //Loop through the duration of the video
                        {
                            if (_tokenSource.Token.IsCancellationRequested)
                            {
                                VideoProgress = j; // Save the current progress when paused
                                ResetIndexWhenPaused(i);
                                return; // Exit 
                            }

                            ProgressValue = (j + 1) * (100.0 / VideoDuration); // Update progress bar value based on the video's duration

                            await Task.Delay(1000, _tokenSource.Token); //Simulate one second
                        }

                        VideoProgress = 0; // After video finishes, reset VideoProgress
                    }

                    else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.Format)) //If it's an image
                    {
                        for (int j = _currentProgress; j < Interval; j++) //loop through the interval
                        {
                            if (_tokenSource.Token.IsCancellationRequested)
                            {
                                _currentProgress = j; // Save progress
                                ResetIndexWhenPaused(i);
                                return; // Exit
                            }

                            ProgressValue = (j + 1) * (100.0 / Interval); //Update progress
                            await Task.Delay(1000, _tokenSource.Token); //wait one second
                        }

                        _currentProgress = 0; // Reset progress
                    }

                    // If it's the last media, reset the player
                    if (i == CurrentLoadedMedia.Count - 1)
                    {
                        ProgressValue = 0;
                        CurrentPlayingMedia = CurrentLoadedMedia.First();
                        _currentProgress = 0;
                        IsPlaying = false;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Sets the index when paused, to prevent the play to start from the incorrect position
        /// </summary>
        private void ResetIndexWhenPaused(int i)
        {
            if (i == 0)
                CurrentPlayingMedia = CurrentLoadedMedia.First();
            else
                CurrentPlayingMedia = CurrentLoadedMedia[i - 1];
        }

        /// <summary>
        /// A method for loading an existing playlsit
        /// </summary>
        private void LoadExistingPlaylist()
        {
            var openManager = new OpenManager("Load Playlist");
            if (openManager.ShowDialog())
            {
                CurrentLoadedMedia.Clear(); //if successful clearing the collection of other media
                foreach (Media media in _mediaBl.LoadPlaylist(openManager.FilePath, false))
                {
                    CurrentLoadedMedia.Add(media); //adds media from the loaded playlist to the player
                }

                PlaylistTitle = _mediaBl.GetPlaylistTitle(); //Sets the title
                SetUpFirstLoadedMedia(); //sets up first loaded media
            }

            else
            {
                openManager.AlertUser();
            }
        }

        /// <summary>
        /// A method that saves a new playlist
        /// </summary>
        private void SaveNewPlaylist()
        {
            var saveManager = new SaveManager();
            if (saveManager.ShowDialog())
                _mediaBl.SavePlaylist(saveManager.FilePath, CurrentLoadedMedia.ToList(), false); // If successull calls the method in the bl layer to save the playlist

            else
            {
                saveManager.AlertUser();
            }
        }

        /// <summary>
        /// A mehod that loads media 
        /// </summary>
        private void LoadNewMedia()
        {
            var openManager = new OpenManager("Load Media");
            if (openManager.ShowDialog()) //if the load is successful
            {
                _selectedFiles = openManager.SelectedFiles; //sets selected files
                foreach (Media m in _mediaBl.LoadMedia(_selectedFiles, false))
                {
                    CurrentLoadedMedia.Add(m); //adds files to the collection
                }
                SetUpFirstLoadedMedia();
            }

            else
            {
                openManager.AlertUser();
            }
        }

        /// <summary>
        /// Method that moves a media up
        /// </summary>
        private void MoveUp(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index > 0)
                SwapMedia(index, index - 1);
        }

        /// <summary>
        /// method that moves a media down
        /// </summary>
        private void MoveDown(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index < CurrentLoadedMedia.Count - 1)
                SwapMedia(index, index + 1);
        }

        /// <summary>
        /// Reorders the media or the playlist
        /// </summary>
        private void SwapMedia(int oldIndex, int newIndex)
        {
            //Does an index swap
            var tempMedia = CurrentLoadedMedia[oldIndex];
            CurrentLoadedMedia[oldIndex] = CurrentLoadedMedia[newIndex];
            CurrentLoadedMedia[newIndex] = tempMedia;
            CurrentPlayingMedia = CurrentLoadedMedia[0];

            OnPropertyChanged(nameof(CurrentPlayingMedia)); //Updates the property of the currentPlayingMedia
        }

        /// <summary>
        /// Method that sets the curentloadedmedia and checks the format and sets
        /// curentvideo or image
        /// </summary>
        private void SetUpFirstLoadedMedia()
        {
            CurrentPlayingMedia = CurrentLoadedMedia[0];
            CheckFormatAndSetCurrentMedia();
        }

        /// <summary>
        /// This method checks what type of media the currentloadedmedia is.
        /// </summary>
        private void CheckFormatAndSetCurrentMedia()
        {
            if (CurrentPlayingMedia != null)
            {
                if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format)) //If it is a video
                {
                    IsVideo = true;
                    IsImage = false;
                    OnPropertyChanged(nameof(IsVideo)); //Updated the properties
                    OnPropertyChanged(nameof(IsImage));
                    CurrentVideo = _mediaBl.CreateVideo(CurrentPlayingMedia.FilePath); //Sets the currentvideo
                }

                else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.Format))
                {
                    IsVideo = false;
                    IsImage = true;
                    OnPropertyChanged(nameof(IsVideo));
                    OnPropertyChanged(nameof(IsImage));
                    CurrentImage = _mediaBl.CreateImage(CurrentPlayingMedia.FilePath); //Sets the currentimage
                }
                else
                {
                    IsVideo = false; //If fail sets to false
                    IsImage = false;
                }
            }
        }

        /// <summary>
        /// Method called if the collection of media has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLoadedMedia)); //Updates the currentLoadedMedia
            Play.RaiseCanExecuteChanged(); //Checks if the the Play command can be played
            SavePlaylist.RaiseCanExecuteChanged(); //Checks if a playlist can be saved
            SavePlaylistToDb.RaiseCanExecuteChanged();    
        }
    }
}
