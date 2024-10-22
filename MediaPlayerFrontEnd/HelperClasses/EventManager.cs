using MediaDTO;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayerPL
{
    /// <summary>
    /// This class handles the event regarding the media element that shows videos
    /// It consists of a Dependency property that will be used for handling events of the mediaelement in the view 
    /// and setting the duration property in the viewmodel
    /// </summary>
    public class EventManager
    {

        /// <summary>
        /// Gets the value of the attached property
        /// </summary>
        public static bool GetEnableVideoEvents(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableVideoEventsProperty);
        }

        /// <summary>
        /// Sets the value of the attached property
        /// </summary>
        public static void SetEnableVideoEvents(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableVideoEventsProperty, value);
        }

        /// <summary>
        /// Registers the attached property. Determines if the events for the mediaelement should be enabled or not via a boolean.
        /// </summary>
        public static readonly DependencyProperty EnableVideoEventsProperty = DependencyProperty.RegisterAttached("EnableVideoEvents", typeof(bool), typeof(EventManager), new PropertyMetadata(false, EnableVideoEventsChanged));


        /// <summary>
        /// This method is called if the attached property is set to true
        /// </summary>
        private static void EnableVideoEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaElement mediaElement) //checks if the dependencyObject is a mediaelement
            {
                mediaElement.LoadedBehavior = MediaState.Manual; //Set to manual to be able to handle play/pause manually

                if (mediaElement.DataContext is MediaPLViewModel mediaPLViewModel) //checks if the datacontext of the media element is the viewmodel
                {
                    mediaElement.MediaOpened += (s, e) => //Handle mediaOpened event
                    {
                        GetVideoDuration(mediaElement, mediaPLViewModel); //Calls method to get the video's duration

                        if (mediaPLViewModel.VideoProgress > 0) //If there is progress stored in the viewmodel, set the position of the mediaelement with the value
                        {
                            mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress);
                        }
                    };

                    mediaPLViewModel.PlayRequested += (s, e) => //Handle the playRequested event from the viewmodel
                    {
                        mediaElement.Play(); //Plays media
                        mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress); //sets the progress
                    };

                    mediaPLViewModel.PauseRequested += (s, e) => //Handle the PauseRequested event from the viewmodel
                    {
                        mediaElement.Pause(); //Pause media
                        mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress);//sets the progress
                    };


                    mediaElement.MediaEnded += (s, e) => //Handle the mediaended event
                    {
                        mediaElement.Stop(); //Stop the vieo
                        mediaPLViewModel.VideoProgress = 0; //Reset the progress
                    };
                }

            }
        }

        /// <summary>
        /// This method retrieves the video duration from the video and 
        /// sets the value to the VideoDuration property in the viewmodel
        /// </summary>
        private static void GetVideoDuration(MediaElement mediaElement, MediaPLViewModel mediaPLViewModel)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan) //checks if the video has a timespan
            {
                if (mediaPLViewModel.VideoDuration == 0) //checks if the video duration hasn't been set yet
                {
                    mediaPLViewModel.VideoDuration = mediaElement.NaturalDuration.TimeSpan.TotalSeconds; //Set the video duration
                }

                mediaPLViewModel.TaskComplete?.TrySetResult(true); // Signals to the viewmodel that the videoduration property is set
            }
        }


        #region event for closing modal windows
        public static bool GetEnableCloseModalEvents(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableCloseModalEventsProperty);
        }

        public static void SetEnableCloseModalEvents(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableCloseModalEventsProperty, value);
        }

        public static readonly DependencyProperty EnableCloseModalEventsProperty = DependencyProperty.RegisterAttached("EnableCloseModalEvents", typeof(bool), typeof(EventManager), new PropertyMetadata(false, EnableCloseModalChanged));

        private static void EnableCloseModalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.DataContextChanged += (s, e) =>
                {
                    if (window.DataContext is EditPlaylistTitleViewModel vm)
                    {
                        vm.Close = () =>
                        {
                            window.DialogResult = vm.DialogResult;
                            window.Close();
                        };
                    }

                    else if (window.DataContext is AddMediaFromDbViewModel mediaVm)
                    {
                        mediaVm.Close = () =>
                        {
                            window.DialogResult = mediaVm.DialogResult;
                            window.Close();
                        };
                    }
                };
            }
        }
        #endregion

        #region event for closing modal windows
        public static bool GetSelectedItemsDatagridEvents(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSelectedItemsDatagridProperty);
        }

        public static void SetSelectedItemsDatagridEvents(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSelectedItemsDatagridProperty, value);
        }

        public static readonly DependencyProperty EnableSelectedItemsDatagridProperty = DependencyProperty.RegisterAttached("EnableSelectedItemsDatagridEvents", typeof(bool), typeof(EventManager), new PropertyMetadata(false, EnableSelectedItemsDatagridChanged));

        private static void EnableSelectedItemsDatagridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged += (s, e) =>
                {
                    if (dataGrid.DataContext is AddMediaFromDbViewModel mediaVm)
                    {
                        var selectedItems = dataGrid.SelectedItems.Cast<Media>().ToList();

                        mediaVm.SelectedMedia.Clear();

                        foreach (var item in selectedItems)
                            mediaVm.SelectedMedia.Add(item);
                    }
                };
            }
        }
    }
    #endregion
}

