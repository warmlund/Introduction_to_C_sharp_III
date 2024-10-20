using System.Windows.Controls;

namespace MediaPlayerPL
{
    /// <summary>
    /// Interaction logic for LimitedInputControl.xaml
    /// </summary>
    public partial class LimitedInputControl : UserControl
    {
        public LimitedInputControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public int MaxLength { get; set; }
    }
}
