using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    public class FileOperations
    {
        MainWindow Window;

        public FileOperations(MainWindow window)
        {
            Window = window;
        }

        public FileOperations()
        { }

        public ObservableCollection<string> GetFileList(string filePath)
        {
            ObservableCollection<string> fileList = new ObservableCollection<string>();
            var directoryContents = Directory.GetFiles(filePath);

            Directory.SetCurrentDirectory(filePath);

            foreach (var file in directoryContents)
            {
                string directory = directoryContents.ToString();
                if (file.Contains(".bak"))
                {
                    fileList.Add(Path.GetFileName(file));
                }                
            }
            fileList.Add("Default");
            return fileList;
        }

        public void CopyToDeviceConfig(string sourceFileName)
        {
            StreamReader reader = null;
            StreamWriter writer = null;

            try
            {
                if (sourceFileName == "Default" && RepoFound(Window.repoLocation))
                {
                    reader = new StreamReader(Window.repoLocation + "Programs\\DeviceConfig_DISPENSER.dft");
                    writer = new StreamWriter(Window.repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");

                    while (!reader.EndOfStream)
                    {
                        writer.WriteLine(reader.ReadLine());
                    }

                    Window.tbNotifications.Text = "Swap complete Sequoia now using " + sourceFileName + " configuration.";

                    reader.Close();
                    writer.Close();
                }
                else if (sourceFileName != "Default" && RepoFound(Window.repoLocation))
                {
                    reader = new StreamReader(Directory.GetCurrentDirectory() + "\\" + sourceFileName);
                    writer = new StreamWriter(Window.repoLocation + "Programs\\DeviceConfig_DISPENSER.ini");

                    while (!reader.EndOfStream)
                    {
                        writer.WriteLine(reader.ReadLine());
                    }

                    Window.tbNotifications.Text = "Swap complete Sequoia now using " + sourceFileName + " configuration.";

                    reader.Close();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RepoFound(string directoryName)
        {
            bool repoFound = false;

            if (Directory.Exists(directoryName))
            {
                repoFound = true;
                Window.tbNotifications.Background = new SolidColorBrush(Colors.White);
                Window.tbNotifications.Foreground = new SolidColorBrush(Colors.Black);
                Window.btnViewCurrent.IsEnabled = true;
            }

            return repoFound;
        }

        public bool DoesFileListContainLastSelected(string directoryLocation)
        {
            bool itemFound = false;
            string lastSelected = Properties.Settings.Default.LastSelectedFile;
            var list = GetFileList(directoryLocation + "AcceptanceTests\\CommonData\\IniFilesForAAT\\");

            if (list.Contains(lastSelected))
            {
                itemFound = true;
            }

            return itemFound;
        }
    }
}