using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayerPL
{
    public class EventManager
    {
        public static bool GetEnableSwitchPlayPause(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSwitchPlayPauseProperty);
        }

        public static void SetEnableSwitchPlayPause(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSwitchPlayPauseProperty, value);
        }

        public static readonly DependencyProperty EnableSwitchPlayPauseProperty =
            DependencyProperty.RegisterAttached(
                "EnableSwitchPlayPause",    
                typeof(bool),               
                typeof(System.Windows.EventManager),       
                new PropertyMetadata(false, EnableSwitchPlayPauseChanged));  

        private static void EnableSwitchPlayPauseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button && button.Template != null)
            {
                var textBlock = button.Template.FindName("Play", button) as TextBlock;

                if (textBlock != null)
                {
                    bool isPlaying = (bool)e.NewValue;
                    textBlock.Text = isPlaying ? "\uF5B1" : "\uF5B0"; 
                }
            }
        }
    }
}
