using System.Windows;
using System.Windows.Controls;
using MediaDTO;

namespace MediaPlayerPL
{
    public class EventManager
    {
        public static bool GetEnableSelectListViewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSelectListViewItemProperty);
        }

        public static void SetEnableSelectListViewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSelectListViewItemProperty, value);
        }

        public static readonly DependencyProperty EnableSelectListViewItemProperty = DependencyProperty.RegisterAttached("EnableSelectListViewItem", typeof(bool), typeof(EventManager), new PropertyMetadata(false, EnableSelectListViewItemChanged));

        private static void EnableSelectListViewItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                listView.SelectionChanged += (s, e) =>
                {
                    if (listView.DataContext is MediaPLViewModel mediaPLViewModel)
                    {
                        mediaPLViewModel.CurrentIndex = listView.SelectedIndex;
                    }
                };
            }
        }
    }
}
