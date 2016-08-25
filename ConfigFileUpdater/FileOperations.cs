using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace ConfigFileUpdater
{
    internal class FileOperations
    {
        MainWindow Window;

        public FileOperations(MainWindow window)
        {
            Window = window;
        }

        public FileOperations()
        { }

        public List<string> GetFileList(string filePath)
        {
            string path = filePath;
            List<string> fileList = new List<string>();
            var directoryContents = Directory.GetFiles(path);

            Directory.SetCurrentDirectory(path);

            foreach (var file in directoryContents)
            {
                fileList.Add(Path.GetFileName(file));
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
                if (sourceFileName == "" || sourceFileName == null)
                {
                    Window.tbNotifications.Text = "No File Selected!";
                    Window.tbNotifications.Background = new SolidColorBrush(Colors.Red);
                    Window.tbNotifications.Foreground = new SolidColorBrush(Colors.White);
                    return;
                }
                else if (sourceFileName == "Default")
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
                else
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
    }
}