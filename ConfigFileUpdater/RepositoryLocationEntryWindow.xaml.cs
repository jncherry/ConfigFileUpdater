using System.Windows;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for RepositoryLocationEntryWindow.xaml
    /// </summary>
    public partial class RepositoryLocationEntryWindow
    {
        protected MainWindow Window;
        protected UtilityMethods UtilityMethods;

        private const string DefaultRepoLocation = "C:\\Sequoia\\";

        public RepositoryLocationEntryWindow(MainWindow window)
        {
            InitializeComponent();
            Window = window;
            txtRepoLocation.Focus();
            UtilityMethods = new UtilityMethods(window, this);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (Window.RepoLocation == DefaultRepoLocation)
            {
                Close();
                return;
            }                
            Window.RepoLocation = DefaultRepoLocation;
            UtilityMethods.UpdateRepositoryLocation(DefaultRepoLocation);
        }

        private void btnSetRepoLocation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtRepoLocation.Text)) return;
            string newRepoPath;
            if (!txtRepoLocation.Text.EndsWith("\\"))
            {
                newRepoPath = txtRepoLocation.Text + "\\";
            }
            else
            {
                newRepoPath = txtRepoLocation.Text;
            }
            Window.RepoLocation = newRepoPath;
            UtilityMethods.UpdateRepositoryLocation(newRepoPath);
            UtilityMethods.PopulateComboBox();
        }
    }
}
