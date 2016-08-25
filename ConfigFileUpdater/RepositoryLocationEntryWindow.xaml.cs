using System.Windows;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for RepositoryLocationEntryWindow.xaml
    /// </summary>
    public partial class RepositoryLocationEntryWindow : Window
    {
        MainWindow Window;

        private const string defaultRepoLocation = "C:\\Sequoia\\";

        public RepositoryLocationEntryWindow(MainWindow window)
        {
            InitializeComponent();
            Window = window;
            txtRepoLocation.Focus();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            Window.repoLocation = defaultRepoLocation;
            _updateRepositoryLocation(defaultRepoLocation);
        }

        private void btnSetRepoLocation_Click(object sender, RoutedEventArgs e)
        {
            if (txtRepoLocation.Text != null && txtRepoLocation.Text != "")
            {
                Window.repoLocation = txtRepoLocation.Text;
                _updateRepositoryLocation(txtRepoLocation.Text);
            }
        }

        private void _updateRepositoryLocation(string value)
        {
            Properties.Settings.Default.CurrentRepoLocation = value;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource settingsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("settingsViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // settingsViewSource.Source = [generic data source]
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            
        }
    }
}
