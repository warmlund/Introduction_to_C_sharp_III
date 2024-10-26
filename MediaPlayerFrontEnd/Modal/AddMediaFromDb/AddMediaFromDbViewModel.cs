using MediaDTO;
using System.Collections.ObjectModel;

namespace MediaPlayerPL
{
    public class AddMediaFromDbViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<Media> _mediaFromDatabase;
        private ObservableCollection<Media> _selectedMedia;
        private bool _dialogResult;

        public ObservableCollection<Media> MediaFromDatabase { get { return _mediaFromDatabase; } set { if (_mediaFromDatabase != value) { _mediaFromDatabase = value; OnPropertyChanged(nameof(MediaFromDatabase)); } } }
        public ObservableCollection<Media> SelectedMedia { get { return _selectedMedia; } set { if (_selectedMedia != value) { _selectedMedia = value; OnPropertyChanged(nameof(SelectedMedia)); } } }
        public bool DialogResult { get { return _dialogResult; } set { if (_dialogResult != value) { _dialogResult = value; OnPropertyChanged(nameof(DialogResult)); } } }

        public Action Close { get; set; }
        public Command AddMedia { get; private set; }
        public Command CancelAddMedia { get; }

        public AddMediaFromDbViewModel(ICollection<Media> mediaFromDb)
        {
            SelectedMedia = new ObservableCollection<Media>();
            MediaFromDatabase = new ObservableCollection<Media>();
            AddMedia = new Command(AddNewMedia, CanAddMedia);
            CancelAddMedia = new Command(CancelAddNewMedia, CanCancelAddMedia);

            foreach (Media media in mediaFromDb)
                MediaFromDatabase.Add(media);
        }

        private bool CanAddMedia() => true;

        private bool CanCancelAddMedia() => true;

        private void AddNewMedia()
        {
            DialogResult = true;
            Close?.Invoke();
        }

        private void CancelAddNewMedia()
        {
            DialogResult = false;
            Close?.Invoke();
        }
    }
}
