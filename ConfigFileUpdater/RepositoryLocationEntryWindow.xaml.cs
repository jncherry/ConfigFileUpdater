using System.Windows;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for RepositoryLocationEntryWindow.xaml
    /// </summary>
    public partial class RepositoryLocationEntryWindow : Window
    {
        MainWindow Window;
        FileOperations FileOperations;

        private const string defaultRepoLocation = "C:\\Sequoia\\";

        public RepositoryLocationEntryWindow(MainWindow window)
        {
            InitializeComponent();
            Window = window;
            FileOperations = new FileOperations(window);
            txtRepoLocation.Focus();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (Window.repoLocation == defaultRepoLocation)
            {
                Close();
                return;
            }
                
            Window.repoLocation = defaultRepoLocation;
            _updateRepositoryLocation(defaultRepoLocation);
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
                _updateRepositoryLocation(newRepoPath);
                Window.lastSelected = "";                
                Window.PopulateComboBox();
            }            
        }

        //____________________________________________________________________________________________________

        private void _updateRepositoryLocation(string value)
        {
            if (FileOperations.RepoFound(value))
            {
                Properties.Settings.Default.CurrentRepoLocation = value;
                Properties.Settings.Default.Save();
                if (FileOperations.RepoFound(value))
                {
                    Window.tbNotifications.Text = "";
                    Window.PopulateComboBox();
                    CheckNewDirForLastSelected(value);
                }
                else
                {
                    Window.SetRepoNotFoundMessage();
                }
                Close();
            }
            else
            {
                Close();
            }
            
        }

        private void CheckNewDirForLastSelected(string repoLocation)
        {
            string lastSelected = Properties.Settings.Default.LastSelectedFile;

            if (!FileOperations.GetFileList(repoLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\").Contains(lastSelected))
            {
                Window.cboFileList.SelectedIndex = -1;
                Window.tbNotifications.Text = "The last selected file " + lastSelected +
                    " was not found in the current repository " + repoLocation + ".";
            }
            else
            {
                Window.cboFileList.SelectedIndex = (int)Window.GetIndexOfLastSelected(lastSelected);
                Window.tbNotifications.Text = "The last selected backup file was:";
            }
        }
    }
}
