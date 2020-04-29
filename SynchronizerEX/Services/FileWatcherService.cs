using SynchronizerEX.Contracts;
using SynchronizerEX.Model;
using SynchronizerEX.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynchronizerEX.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        public List<FileInformationToSynchronize> ListOfFilesInWatcherService { get; set; }
        public List<ParentDirectoryInfo> ParentDirectoryInfo { get; set; }
        public List<string> ListOfFilesReadyToSynchronize { get; set; }
        public List<string> ListOfFilesToRename { get; set; }

        public static int ParentCatalogId { get; set; } = 0;

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
            

            ParentCatalogId++;

            DirectoryInfo di = new DirectoryInfo(path);
            var parentDirObject = new Model.ParentDirectoryInfo(ParentCatalogId, di);
            ParentDirectoryInfo.Add(parentDirObject);

            DirSearch(parentDirObject, path);
        }

        private void DirSearch(ParentDirectoryInfo parent, string sDir)
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(sDir);
                foreach (var item in directoryInfo.GetFiles())
                {
                    var file = new FileInformationToSynchronize(parent);
                    file.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo(
                            RelativePath(parent.DirectoryInfo.FullName,item.FullName),
                            item.LastWriteTime,
                            this.GetHashString(item.FullName),
                            parent.Id));
                    ListOfFilesInWatcherService.Add(file);
                    //using (FileStream fs = File.Open(item.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                    //{

                    //}
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    DirSearch(parent, d);
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public string GetHashString(string pathToFile)
        {
            MD5 mD5 = MD5.Create();
            using (var stream = File.OpenRead(pathToFile))
            {
                var a = mD5.ComputeHash(stream);
                var res = String.Empty;
                foreach (byte b in a)
                { 
                    res += b.ToString();
                }
                return res;
            }
        }

        public string RelativePath(string absPath, string relTo)
        {
            string[] absDirs = absPath.Split('\\');
            string[] relDirs = relTo.Split('\\');

            // Get the shortest of the two paths
            int len = absDirs.Length < relDirs.Length ? absDirs.Length :
            relDirs.Length;

            // Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            // Find common root
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
                else break;
            }

            // If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }

            // Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // Add on the ..
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0) relativePath.Append("..\\");
            }

            // Add on the folders
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + "\\");
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);

            return relativePath.ToString();
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (Directory.Exists(e.FullPath))
            {
                foreach (var chosenDirectory in ParentDirectoryInfo)
                {
                    var trimmedString = e.OldFullPath.Replace(chosenDirectory.DirectoryInfo.FullName + "\\", String.Empty);

                    if (trimmedString != chosenDirectory.DirectoryInfo.FullName)
                    {

                        foreach (var item in ListOfFilesInWatcherService)
                        {
                            if (item.FileChangesHistory.LastOrDefault().Path.Contains(trimmedString))
                            {
                                //System.IO.FileInfo fileInfo = new System.IO.FileInfo(item.FileChangesHistory.LastOrDefault().Path
                                //    .Replace(trimmedString, e.Name));
                                item.FileChangesHistory.Add(
                                    new Model.SynchronizerFileInfo(
                                       /* RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, fileInfo.FullName)*/
                                        item.FileChangesHistory.LastOrDefault().Path.Replace(e.OldName, e.Name),
                                        item.FileChangesHistory.LastOrDefault().LastSaveTime,
                                        item.FileChangesHistory.LastOrDefault().Hash,
                                        item.ParentDirInfo.Id));
                            }
                        }
                        break;
                    }
                }




            }
            else
            {
                foreach (var item in ListOfFilesInWatcherService)
                {
                    if (item.FileChangesHistory.LastOrDefault().Path == e.OldFullPath)
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                        item.FileChangesHistory.Add(
                            new Model.SynchronizerFileInfo(
                                RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath),
                                fileInfo.LastWriteTime,
                                this.GetHashString(e.FullPath),
                                item.ParentDirInfo.Id));
                        break;
                    }
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
                var trimPath = RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath);

                if (item.FileChangesHistory.LastOrDefault().Path == trimPath)
                {
                //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                    item.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo("deleted", new DateTime(), String.Empty, -1));
                    break;
                }
            }
            System.Windows.MessageBox.Show($"Deleted. File: {e.FullPath} {e.ChangeType}");
        }

        public void OnCreated(object sender, FileSystemEventArgs e)
        {
            //FileAttributes file = File.GetAttributes(e.FullPath);
            //if (!file.HasFlag(FileAttributes.Hidden))

            if (!Directory.Exists(e.FullPath))
            {

                var watcher = (FileSystemWatcher)sender;
                var b = watcher.Path;

                foreach (var item in ParentDirectoryInfo)
                {
                    if (item.DirectoryInfo.FullName == watcher.Path)
                    {
                        var file = new FileInformationToSynchronize(item);
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                        file.FileChangesHistory.Add(
                                new Model.SynchronizerFileInfo(
                                    RelativePath(item.DirectoryInfo.FullName, e.FullPath),
                                    fileInfo.LastWriteTime,
                                    this.GetHashString(e.FullPath),
                                    item.Id));
                        ListOfFilesInWatcherService.Add(file);
                        break;
                    }
                }


            }



            System.Windows.MessageBox.Show($"Created. File: {e.FullPath} {e.ChangeType}");
        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            //FileAttributes file = File.GetAttributes(e.FullPath);
            //if (!file.HasFlag(FileAttributes.Hidden))
            foreach (var item in ListOfFilesInWatcherService)
            {
                if (item.FileChangesHistory.LastOrDefault().Path == e.FullPath)
                {
                    
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                    item.FileChangesHistory.Add(
                        new Model.SynchronizerFileInfo(
                            RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath), 
                            fileInfo.LastWriteTime,
                            this.GetHashString(e.FullPath),
                            item.ParentDirInfo.Id));
                    break;
                }
            }
            System.Windows.MessageBox.Show($"Changed. File: {e.FullPath} {e.ChangeType}");
        }


        public FileWatcherService()
        {
            ListOfFilesInWatcherService = new List<FileInformationToSynchronize>();
            ParentDirectoryInfo = new List<ParentDirectoryInfo>();
        }
    }
}
