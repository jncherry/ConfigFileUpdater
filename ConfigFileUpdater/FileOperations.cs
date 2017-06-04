using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    public class FileOperations
    {
        protected MainWindow Window;
        private static StreamReader _streamReader;
        private static StreamWriter _streamWriter;
        private string _sourceFile;
        private string _destinatoinFile;

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
            if (sourceFileName == "Default")
            {
                _sourceFile = Window.RepoLocation + "Programs\\DeviceConfig_DISPENSER.dft";
                _destinatoinFile = Window.RepoLocation + "Programs\\DeviceConfig_DISPENSER.ini";
                SwapFileData(sourceFileName, _sourceFile, _destinatoinFile);
            }
            else
            {
                _sourceFile = Directory.GetCurrentDirectory() + "\\" + sourceFileName;
                _destinatoinFile = Window.RepoLocation + "Programs\\DeviceConfig_DISPENSER.ini";
                SwapFileData(sourceFileName, _sourceFile, _destinatoinFile);
            }
        }

        private void SwapFileData(string sourceFileName, string sourceFile, string destinatoinFile)
        {
            _streamReader = new StreamReader(sourceFile);
            _streamWriter = new StreamWriter(destinatoinFile);

            while (!_streamReader.EndOfStream)
            {
                _streamWriter.WriteLine(_streamReader.ReadLine());
            }

            Window.tbNotifications.Text = "Swap complete Sequoia now using " + sourceFileName + " configuration.";
            _streamReader.Close();
            _streamWriter.Close();
        }

        public bool RepoFound(string directoryName)
        {
            if (!Directory.Exists(directoryName)) return false;
            Window.tbNotifications.Background = new SolidColorBrush(Colors.White);
            Window.tbNotifications.Foreground = new SolidColorBrush(Colors.Black);
            Window.btnViewCurrent.IsEnabled = true;
            return true;
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