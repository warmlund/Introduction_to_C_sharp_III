using MediaPlayerBL;
using System.Windows;

namespace MediaPlayerPL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var mediaBl = new MediaBL();
            var viewModel = new MediaPLViewModel(mediaBl);
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}