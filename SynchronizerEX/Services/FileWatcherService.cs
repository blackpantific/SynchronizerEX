using SynchronizerEX.Contracts;
using SynchronizerEX.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        public List<FileInformationToSynchronize> ListOfFilesInWatcherService { get; set; }



        public void CreateWatcher(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (var item in directoryInfo.GetFiles())
            {
                var file = new FileInformationToSynchronize();
                file.FileChangesHistory.Add(
                    new Model.SynchronizerFileInfo(item.FullName, item.LastWriteTime, item.GetHashCode()));
                ListOfFilesInWatcherService.Add(file);
            }

        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            foreach (var item in ListOfFilesInWatcherService)
            {
                if(item.FileChangesHistory.LastOrDefault().FullPath == e.OldFullPath)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                    item.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo(e.FullPath, fileInfo.LastWriteTime, fileInfo.GetHashCode()));
                    break;
                }
            }

        //FileAttributes file = File.GetAttributes(e.FullPath);
        //    if(!file.HasFlag(FileAttributes.Hidden))
        //    {

                System.Windows.MessageBox.Show($"Renamed. File: {e.OldFullPath} renamed to {e.FullPath}");
            //}

        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            // FileAttributes file = File.GetAttributes(e.FullPath);
            //if (!file.HasFlag(FileAttributes.Hidden))

            foreach (var item in ListOfFilesInWatcherService)
            {
                if (item.FileChangesHistory.LastOrDefault().FullPath == e.FullPath)
                {
                //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                    item.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo("deleted", new DateTime(), -1));
                    break;
                }
            }
            System.Windows.MessageBox.Show($"Deleted. File: {e.FullPath} {e.ChangeType}");
        }

        public void OnCreated(object sender, FileSystemEventArgs e)
        {
            //FileAttributes file = File.GetAttributes(e.FullPath);
            //if (!file.HasFlag(FileAttributes.Hidden))
            var file = new FileInformationToSynchronize();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
            file.FileChangesHistory.Add(
                    new Model.SynchronizerFileInfo(e.FullPath, fileInfo.LastWriteTime, fileInfo.GetHashCode()));
            ListOfFilesInWatcherService.Add(file);
            System.Windows.MessageBox.Show($"Created. File: {e.FullPath} {e.ChangeType}");
        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            //FileAttributes file = File.GetAttributes(e.FullPath);
            //if (!file.HasFlag(FileAttributes.Hidden))
            foreach (var item in ListOfFilesInWatcherService)
            {
                if (item.FileChangesHistory.LastOrDefault().FullPath == e.FullPath)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                    item.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo(e.FullPath, fileInfo.LastWriteTime, fileInfo.GetHashCode()));
                    break;
                }
            }
            System.Windows.MessageBox.Show($"Changed. File: {e.FullPath} {e.ChangeType}");
        }


        public FileWatcherService()
        {
            ListOfFilesInWatcherService = new List<FileInformationToSynchronize>();
        }
    }
}
