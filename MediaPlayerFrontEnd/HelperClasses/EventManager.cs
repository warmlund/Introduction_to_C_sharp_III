using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayerPL
{
    public class EventManager
    {

        public static bool GetEnableVideoEvents(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableVideoEventsProperty);
        }

        public static void SetEnableVideoEvents(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableVideoEventsProperty, value);
        }

        public static readonly DependencyProperty EnableVideoEventsProperty = DependencyProperty.RegisterAttached("EnableVideoEvents", typeof(bool), typeof(EventManager), new PropertyMetadata(false, EnableVideoEventsChanged));


        private static void EnableVideoEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaElement mediaElement)
            {
                mediaElement.LoadedBehavior = MediaState.Manual;

                if (mediaElement.DataContext is MediaPLViewModel mediaPLViewModel)
                {
                    mediaElement.MediaOpened += (s, e) =>
                    {
                        GetVideoDuration(mediaElement, mediaPLViewModel);
                        // Ensure we update VideoProgress after the media opens
                        if (mediaPLViewModel.VideoProgress > 0)
                        {
                            mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress);
                        }
                    };

                    mediaPLViewModel.PlayRequested += (s, e) =>
                    {
                        mediaElement.Play();
                        mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress); 
                    };

                    mediaPLViewModel.PauseRequested += (s, e) =>
                    {
                        mediaElement.Pause();
                        mediaElement.Position = TimeSpan.FromSeconds(mediaPLViewModel.VideoProgress);
                    };

                    
                    mediaElement.MediaEnded += (s, e) =>
                    {
                          mediaElement.Stop();
                          mediaPLViewModel.VideoProgress = 0;
                    };
                }

            }
        }

        private static void GetVideoDuration(MediaElement mediaElement, MediaPLViewModel mediaPLViewModel)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                if (mediaPLViewModel.VideoDuration == 0)
                {
                    mediaPLViewModel.VideoDuration = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                }
                // Trigger the task completion for PlayMediaAsync to continue
                mediaPLViewModel.TaskComplete?.TrySetResult(true);
            }
        }
    }
}
