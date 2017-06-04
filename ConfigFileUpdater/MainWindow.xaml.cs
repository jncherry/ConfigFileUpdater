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
        protected FileOperations FileOperations;
        protected UtilityMethods UtilityMethods;
        protected RepositoryLocationEntryWindow RepositoryLocationEntryWindow;

        public string RepoLocation { get; set; }
        public string LastSelected { get; set; }
        public ObservableCollection<string> FileList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FileOperations = new FileOperations(this);            
            UtilityMethods = new UtilityMethods(this, RepositoryLocationEntryWindow);            
            RepoLocation = Properties.Settings.Default.CurrentRepoLocation;

            if (FileOperations.RepoFound(RepoLocation))
            {
                LastSelected = Properties.Settings.Default.LastSelectedFile;
            }
            else
            {
                UtilityMethods.SetRepoNotFoundMessage();
            }
            
            cboFileList.Items.Clear();
            UtilityMethods.PopulateComboBox();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastSelectedFile) && FileOperations.RepoFound(RepoLocation))
                try
                {
                    var indexOfLastSelected =
                        UtilityMethods.GetIndexOfLastSelected(Properties.Settings.Default.LastSelectedFile);
                    if (indexOfLastSelected != null)
                        cboFileList.SelectedItem =
                            cboFileList.Items[
                                (int) indexOfLastSelected];
                    tbNotifications.Text = "The last selected backup file was:";
                }
                catch (Exception)
                {
                    cboFileList.SelectedIndex = -1;
                    tbNotifications.Text = "";
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource settingsViewSource = (System.Windows.Data.CollectionViewSource)FindResource("settingsViewSource");
            if (settingsViewSource == null) throw new ArgumentNullException(nameof(settingsViewSource));
        }        

        private void cboFileList_DropDownOpened(object sender, EventArgs e)
        {
            cboFileList.Items.Clear();
            UtilityMethods.PopulateComboBox();
        }

        private void btnSwapFiles_Click(object sender, RoutedEventArgs e)
        {
            if (cboFileList.SelectedIndex == -1 && cboFileList.SelectedItem.ToString() == "") return;
            FileOperations.CopyToDeviceConfig(cboFileList.SelectedItem.ToString());
            LastSelected = cboFileList.SelectedItem.ToString();
            UtilityMethods.UpdateLastSelected(cboFileList.SelectedItem.ToString());
        }

        private void btnViewCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (!FileOperations.RepoFound(RepoLocation))
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
                if (LastSelected != "" && FileOperations.RepoFound(RepoLocation) && FileOperations.DoesFileListContainLastSelected(RepoLocation))
                {
                    var indexOfLastSelected = UtilityMethods.GetIndexOfLastSelected(LastSelected);
                    if (indexOfLastSelected != null)
                        cboFileList.SelectedIndex = (int) indexOfLastSelected;
                }
            }

            if (cboFileList.SelectedItem != null)
                btnSwapFiles.IsEnabled = cboFileList.SelectedItem.ToString() != LastSelected;
        }
    }
}

