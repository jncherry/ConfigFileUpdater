using System.Windows;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for RepositoryLocationEntryWindow.xaml
    /// </summary>
    public partial class RepositoryLocationEntryWindow : Window
    {
        MainWindow Window;
        UtilityMethods UtilityMethods;

        private const string defaultRepoLocation = "C:\\Sequoia\\";

        public RepositoryLocationEntryWindow(MainWindow window)
        {
            InitializeComponent();
            Window = window;
            txtRepoLocation.Focus();
            UtilityMethods = new UtilityMethods(window, this);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (Window.repoLocation == defaultRepoLocation)
            {
                Close();
                return;
            }                
            Window.repoLocation = defaultRepoLocation;
            UtilityMethods.UpdateRepositoryLocation(defaultRepoLocation);
        }

        private void btnSetRepoLocation_Click(object sender, RoutedEventArgs e)
        {
            string newRepoPath;

            if (txtRepoLocation.Text != null && txtRepoLocation.Text != "")
            {
                if (!txtRepoLocation.Text.EndsWith("\\"))
                {
                    newRepoPath = txtRepoLocation.Text + "\\";
                }
                else
                {
                    newRepoPath = txtRepoLocation.Text;
                }
                Window.repoLocation = newRepoPath;
                UtilityMethods.UpdateRepositoryLocation(newRepoPath);
                Window.lastSelected = "";
                UtilityMethods.PopulateComboBox();
            }            
        }
    }
}
