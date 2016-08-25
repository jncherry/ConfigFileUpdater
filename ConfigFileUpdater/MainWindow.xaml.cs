using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        FileOperations FileOperations;

        //public string repoLocation;
        public string repoLocation { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FileOperations = new FileOperations(this);
            repoLocation = Properties.Settings.Default.CurrentRepoLocation;
        }

        public void populateComboBox()
        {
            List<string> fileList = FileOperations.GetFileList(repoLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\");

            for (int i = 0; i < fileList.Count; i++)
            {
                cboFileList.Items.Add(fileList[i]);
            }
        }

        private void btnSwapFiles_Click(object sender, RoutedEventArgs e)
        {
            if (cboFileList.SelectedIndex != -1 || cboFileList.SelectedItem.ToString() != "")
            {
                FileOperations.CopyToDeviceConfig(cboFileList.SelectedItem.ToString());
            }            
        }

        private void cboFileList_DropDownOpened(object sender, EventArgs e)
        {
            cboFileList.Items.Clear();
            populateComboBox();            
        }

        private void cboFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string messageText = "";
            tbNotifications.Background = new SolidColorBrush(Colors.White);
            tbNotifications.Foreground = new SolidColorBrush(Colors.Black);
            tbNotifications.Text = messageText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource settingsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("settingsViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // settingsViewSource.Source = [generic data source]
        }
    }
}

