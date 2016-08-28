using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        FileOperations FileOperations;
        UtilityMethods UtilityMethods;
        RepositoryLocationEntryWindow RepositoryLocationEntryWindow;

        public string repoLocation { get; set; }
        public string lastSelected { get; set; }
        public ObservableCollection<string> fileList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FileOperations = new FileOperations(this);            
            UtilityMethods = new UtilityMethods(this, RepositoryLocationEntryWindow);            
            repoLocation = Properties.Settings.Default.CurrentRepoLocation;

            if (FileOperations.RepoFound(repoLocation))
            {
                lastSelected = Properties.Settings.Default.LastSelectedFile;
            }
            else
            {
                UtilityMethods.SetRepoNotFoundMessage();
            }
            
            cboFileList.Items.Clear();
            UtilityMethods.PopulateComboBox();

            if (Properties.Settings.Default.LastSelectedFile == null || Properties.Settings.Default.LastSelectedFile == "" || !FileOperations.RepoFound(repoLocation))
            {
                return;
            }
            else
            {
                try
                {
                    cboFileList.SelectedItem = cboFileList.Items[(int)UtilityMethods.GetIndexOfLastSelected(Properties.Settings.Default.LastSelectedFile)];
                    tbNotifications.Text = "The last selected backup file was:";
                }
                catch (Exception)
                {
                    cboFileList.SelectedIndex = -1;
                    tbNotifications.Text = "";
                }                
            }            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource settingsViewSource = ((System.Windows.Data.CollectionViewSource)(FindResource("settingsViewSource")));
        }        

        private void cboFileList_DropDownOpened(object sender, EventArgs e)
        {
            cboFileList.Items.Clear();
            UtilityMethods.PopulateComboBox();
        }

        private void btnSwapFiles_Click(object sender, RoutedEventArgs e)
        {
            if (cboFileList.SelectedIndex != -1 || cboFileList.SelectedItem.ToString() != "")
            {
                FileOperations.CopyToDeviceConfig(cboFileList.SelectedItem.ToString());
                lastSelected = cboFileList.SelectedItem.ToString();
                UtilityMethods.UpdateLastSelected(cboFileList.SelectedItem.ToString());
            }
        }

        private void btnViewCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (!FileOperations.RepoFound(repoLocation))
            {
                tbNotifications.Background = new SolidColorBrush(Colors.Red);
                tbNotifications.Foreground = new SolidColorBrush(Colors.White);
                tbNotifications.Text = "DeviceConfig_DISPENSER.ini not found! Please verify your repository location and try again.";
                return;
            }
            UtilityMethods.RunTextEditor();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            RepositoryLocationEntryWindow = new RepositoryLocationEntryWindow(this);
            RepositoryLocationEntryWindow.Show();
        }
        
        private void cboFileList_DropDownClosed(object sender, EventArgs e)
        {
            if (cboFileList.SelectedItem == null || cboFileList.SelectedItem.ToString() == "")
            {
                if (lastSelected != "" && FileOperations.RepoFound(repoLocation) && FileOperations.DoesFileListContainLastSelected(repoLocation))
                {
                    cboFileList.SelectedIndex = (int)UtilityMethods.GetIndexOfLastSelected(lastSelected);
                }
            }
        }

        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
        }
    }
}

