using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        
        public string repoLocation { get; set; }
        public string lastSelected { get; set; }
        private ObservableCollection<string> _fileList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FileOperations = new FileOperations(this);
            repoLocation = Properties.Settings.Default.CurrentRepoLocation;

            if (FileOperations.RepoFound(repoLocation))
            {
                lastSelected = Properties.Settings.Default.LastSelectedFile;
            }
            else
            {
                SetRepoNotFoundMessage();
            }
            
            cboFileList.Items.Clear();
            PopulateComboBox();

            if (Properties.Settings.Default.LastSelectedFile == null || Properties.Settings.Default.LastSelectedFile == "" || !FileOperations.RepoFound(repoLocation))
            {
                return;
            }
            else
            {
                try
                {
                    cboFileList.SelectedItem = cboFileList.Items[(int)GetIndexOfLastSelected(Properties.Settings.Default.LastSelectedFile)];
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
            PopulateComboBox();
        }

        private void btnSwapFiles_Click(object sender, RoutedEventArgs e)
        {
            if (cboFileList.SelectedIndex != -1 || cboFileList.SelectedItem.ToString() != "")
            {
                FileOperations.CopyToDeviceConfig(cboFileList.SelectedItem.ToString());
                lastSelected = cboFileList.SelectedItem.ToString();
                _updateLastSelected(cboFileList.SelectedItem.ToString());
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

            try
            {
                Process.Start("Notepad++.exe", repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");
            }
            catch
            {
                Process.Start("Notepad.exe", repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            RepositoryLocationEntryWindow RepositoryLocationEntry = new RepositoryLocationEntryWindow(this);
            RepositoryLocationEntry.Show();
        }
        
        private void cboFileList_DropDownClosed(object sender, EventArgs e)
        {
            if (cboFileList.SelectedItem == null || cboFileList.SelectedItem.ToString() == "")
            {
                if (lastSelected != "" && FileOperations.RepoFound(repoLocation) && FileOperations.DoesFileListContainLastSelected(repoLocation))
                {
                    cboFileList.SelectedIndex = (int)GetIndexOfLastSelected(lastSelected);
                }
            }
        }

        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
        }

        //______________________________________________________________________________________________________________________

        public void PopulateComboBox()
        {
            if (FileOperations.RepoFound(repoLocation))
            {
                try
                {
                    _fileList = FileOperations.GetFileList(repoLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\");

                    foreach (var t in _fileList)
                    {
                        cboFileList.Items.Add(t);
                    }
                }
                catch (Exception)
                {
                    return;
                }                
            }
            else
            {
                SetRepoNotFoundMessage();
            }
        }

        private void _updateLastSelected(string value)
        {
            Properties.Settings.Default.LastSelectedFile = value;
            Properties.Settings.Default.Save();
        }

        public int? GetIndexOfLastSelected(string lastSelectedFile)
        {
            int? index = null;
            for (int i = 0; i < cboFileList.Items.Count; i++)
            {
                if (cboFileList.Items[i].ToString().Contains(lastSelectedFile))
                {
                    index = i;
                }
            }

            if (index == null)
            {
                tbNotifications.Text = "Last selected item not found!";
            }
            return index;
        }       

        public void SetRepoNotFoundMessage()
        {
            tbNotifications.Background = new SolidColorBrush(Colors.Red);
            tbNotifications.Foreground = new SolidColorBrush(Colors.White);
            tbNotifications.Text = ("The selected repo '" + repoLocation + "' was not found! Set Repo location via: File -> Set Repo Location.");
            cboFileList.Items.Clear();
            btnViewCurrent.IsEnabled = false;
        }   
    }
}

