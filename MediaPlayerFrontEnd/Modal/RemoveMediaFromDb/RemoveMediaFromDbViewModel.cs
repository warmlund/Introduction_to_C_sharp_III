using MediaDTO;
using System.Collections.ObjectModel;

namespace MediaPlayerPL
{
    public class RemoveMediaFromDbViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<Media> _mediaFromDatabase;
        private ObservableCollection<Media> _selectedMedia;
        private bool _dialogResult;

        public ObservableCollection<Media> MediaFromDatabase { get { return _mediaFromDatabase; } set { if (_mediaFromDatabase != value) { _mediaFromDatabase = value; OnPropertyChanged(nameof(MediaFromDatabase)); } } }
        public ObservableCollection<Media> SelectedMedia { get { return _selectedMedia; } set { if (_selectedMedia != value) { _selectedMedia = value; OnPropertyChanged(nameof(SelectedMedia)); } } }
        public bool DialogResult { get { return _dialogResult; } set { if (_dialogResult != value) { _dialogResult = value; OnPropertyChanged(nameof(DialogResult)); } } }

        public Action Close { get; set; }
        public Command RemoveMedia { get; private set; }
        public Command CancelRemoveMedia { get; }

        public RemoveMediaFromDbViewModel(ICollection<Media> mediaFromDb)
        {
            SelectedMedia = new ObservableCollection<Media>();
            MediaFromDatabase = new ObservableCollection<Media>();
            RemoveMedia = new Command(RemoveSelectedMedia, CanRemoveMedia);
            CancelRemoveMedia = new Command(CancelRemoveSelectedMedia, CanCancelAddMedia);

            foreach (Media media in mediaFromDb)
                MediaFromDatabase.Add(media);
        }

        private bool CanRemoveMedia() => true;

        private bool CanCancelAddMedia() => true;

        private void RemoveSelectedMedia()
        {
            DialogResult = true;
            Close?.Invoke();
        }

        private void CancelRemoveSelectedMedia()
        {
            DialogResult = false;
            Close?.Invoke();
        }
    }
}
