namespace MediaPlayerPL.HelperClasses
{
    interface ICloseWindow
    {
        Action Close { get; set; }
        bool DialogResult { get; set; }
    }
}
