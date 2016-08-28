using System;
using System.Diagnostics;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    public class UtilityMethods
    {        
        MainWindow Window;
        FileOperations FileOperations;
        RepositoryLocationEntryWindow RepositoryLocationEntryWindow;

        public UtilityMethods(MainWindow window, RepositoryLocationEntryWindow repositoryLocationEntryWindow)
        {
            Window = window;
            RepositoryLocationEntryWindow = repositoryLocationEntryWindow;
            FileOperations = new FileOperations(window);
        }

        public UtilityMethods()
        { }

        public void PopulateComboBox()
        {
            if (FileOperations.RepoFound(Window.repoLocation))
            {
                try
                {
                    Window.fileList = FileOperations.GetFileList(Window.repoLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\");

                    foreach (var t in Window.fileList)
                    {
                        Window.cboFileList.Items.Add(t);
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

        public void UpdateLastSelected(string value)
        {
            Properties.Settings.Default.LastSelectedFile = value;
            Properties.Settings.Default.Save();
        }

        public int? GetIndexOfLastSelected(string lastSelectedFile)
        {
            int? index = null;
            for (int i = 0; i < Window.cboFileList.Items.Count; i++)
            {
                if (Window.cboFileList.Items[i].ToString().Contains(lastSelectedFile))
                {
                    index = i;
                    break;
                }
            }

            if (index == null)
            {
                Window.tbNotifications.Text = "Last selected item not found!";
            }
            return index;
        }

        public void SetRepoNotFoundMessage()
        {
            Window.tbNotifications.Background = new SolidColorBrush(Colors.Red);
            Window.tbNotifications.Foreground = new SolidColorBrush(Colors.White);
            Window.tbNotifications.Text = ("The selected repo '" + Window.repoLocation + "' was not found! Set Repo location via: File -> Set Repo Location.");
            Window.cboFileList.Items.Clear();
            Window.btnViewCurrent.IsEnabled = false;
        }

        public void UpdateRepositoryLocation(string value)
        {
            if (FileOperations.RepoFound(value))
            {
                Properties.Settings.Default.CurrentRepoLocation = value;
                Properties.Settings.Default.Save();
                if (FileOperations.RepoFound(value))
                {
                    Window.tbNotifications.Text = "";
                    PopulateComboBox();
                    CheckNewDirForLastSelected(value);
                }
                else
                {
                    SetRepoNotFoundMessage();
                }
                RepositoryLocationEntryWindow.Close();
            }
            else
            {
                RepositoryLocationEntryWindow.Close();
            }
        }

        public void CheckNewDirForLastSelected(string repoLocation)
        {
            string lastSelected = Properties.Settings.Default.LastSelectedFile;

            if (!FileOperations.GetFileList(repoLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\").Contains(lastSelected))
            {
                Window.cboFileList.SelectedIndex = -1;
                Window.tbNotifications.Text = "The last selected file " + lastSelected +
                    " was not found in the current repository.";
            }
            else
            {
                Window.cboFileList.SelectedIndex = (int)GetIndexOfLastSelected(lastSelected);
                Window.tbNotifications.Text = "The last selected backup file was:";
            }
        }

        public void RunTextEditor()
        {
            try
            {
                Process.Start("Notepad++.exe", Window.repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");
            }
            catch
            {
                Process.Start("Notepad.exe", Window.repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");
            }
        }
    }
}
