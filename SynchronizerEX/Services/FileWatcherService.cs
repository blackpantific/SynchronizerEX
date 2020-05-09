using Newtonsoft.Json;
using SynchronizerEX.Contracts;
using SynchronizerEX.Model;
using SynchronizerEX.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynchronizerEX.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        public static List<ParentDirectoryInfo> ParentDirectoryInfo { get; set; } = new List<ParentDirectoryInfo>();
        public static List<FileInformationToSynchronize> ListOfFilesInWatcherService { get; set; } = new List<FileInformationToSynchronize>();
        public List<string> ListOfFilesReadyToSynchronize { get; set; }
        public List<string> ListOfFilesToRename { get; set; }

        public static string workingDirectory = Environment.CurrentDirectory;
        public static string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;


        public static int ParentCatalogId { get; set; } = 0;

        public void CreateWatcher(string path)
        {
            //    ПРОТЕСТИРОВАТЬ РАБОТУ КЛАССА ПРИ ДОБАВЛЕНИИ ДЛЯ ОТСЛЕЖИВАНИЯ НЕСКОЛЬКИХ КАТАЛОГОВ
            //        ДОРАБОТАТЬ ФУНКЦИОНАЛ МЕТОДОВ OnCreated(), OnDeleted 


            CreateWatcherObject(path);

            ParentCatalogId++;

            DirectoryInfo di = new DirectoryInfo(path);
            var parentDirObject = new Model.ParentDirectoryInfo(ParentCatalogId, di);
            ParentDirectoryInfo.Add(parentDirObject);

            DirSearch(parentDirObject, path);
        }

        public void CreateWatcherObject(string path)
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


                    System.Timers.Timer timer = new System.Timers.Timer(10000);
                    timer.Elapsed += (sender, e) => Timer_Elapsed(sender, e, file, item);
                    timer.AutoReset = false;

                    file.FileTimer = timer;

                    timer.Enabled = true;
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

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, FileInformationToSynchronize toSynchronize,
            FileInfo file = null)
        {
            var fileBusy = IsLocked(file.FullName);

            if (fileBusy)
            {
                var timer = new System.Timers.Timer(30000);
                timer.Elapsed += (sender1, e1) => Timer_Elapsed(sender, e, toSynchronize, file);
                timer.AutoReset = false;
                timer.Enabled = true;
            }

        }

        public bool IsLocked(string fileName)
        {
            try
            {
                using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    fs.Close();

                    //добавляем файл в стек на отправку

                    return false;
                }
            }
            catch (Exception ex)
            {

                if (ex.HResult == -2147024894)
                    return false;
            }
            return true;
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
                                item.FileTimer.Stop();

                                item.FileChangesHistory.Add(
                                    new Model.SynchronizerFileInfo(
                                       /* RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, fileInfo.FullName)*/
                                        item.FileChangesHistory.LastOrDefault().Path.Replace(e.OldName, e.Name),
                                        item.FileChangesHistory.LastOrDefault().LastSaveTime,
                                        item.FileChangesHistory.LastOrDefault().Hash,
                                        item.ParentDirInfo.Id));
                                item.FileTimer.Start();
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
                    if (item.FileChangesHistory.LastOrDefault().Path.Contains(e.OldName)) 
                    {
                        item.FileTimer.Stop();

                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                        item.FileChangesHistory.Add(
                            new Model.SynchronizerFileInfo(
                                RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath),
                                fileInfo.LastWriteTime,
                                this.GetHashString(e.FullPath),
                                item.ParentDirInfo.Id));

                        item.FileTimer.Start();

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

            //ЗАМЕЧАНИЕ!
            //    ПОКА ЧТО ОТСУТСТВУЕТ ВОЗМОЖНОСТЬ УДАЛЕНИЯ КАТАЛОГА И СЧИТЫВАНИЕ ВСЕХ ЕГО ВНУТРЕННИХ ФАЙЛОВ
            //    ИЗ СПИСКА. ВЫПОЛНЯТЬ РАБОТУ ТОЛЬКО С ТЕКУЩИМИ ФАЙЛАМИ В КАТАЛОГЕ. ПОЗЖЕ ФУНКЦИОНАЛ ДОРАБОТАТЬ.

            foreach (var item in ListOfFilesInWatcherService)
            {
                var trimPath = RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath);

                if (item.FileChangesHistory.LastOrDefault().Path == trimPath)
                {
                    item.FileTimer.Stop();
                    item.FileTimer.Dispose();
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

            //ЗАМЕЧАНИЕ!
            //    ПОКА ЧТО ОТСУТСТВУЕТ ВОЗМОЖНОСТЬ ДОБАВЛЕНИЯ КАТАЛОГА И СЧИТЫВАНИЕ ВСЕХ ЕГО ВНУТРЕННИХ ФАЙЛОВ
            //    В СПИСОК. ВЫПОЛНЯТЬ РАБОТУ ТОЛЬКО С ТЕКУЩИМИ ФАЙЛАМИ В КАТАЛОГЕ. ПОЗЖЕ ФУНКЦИОНАЛ ДОРАБОТАТЬ.

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

                        System.Timers.Timer timer = new System.Timers.Timer(10000);
                        timer.Elapsed += (sender2, e2) => Timer_Elapsed(sender2, e2, file, fileInfo);
                        timer.AutoReset = false;

                        file.FileTimer = timer;

                        timer.Enabled = true;


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

            if (!Directory.Exists(e.FullPath))
            {

                foreach (var item in ListOfFilesInWatcherService)
                {
                    if (item.FileChangesHistory.LastOrDefault().Path == e.FullPath)
                    {

                        item.FileTimer.Stop();


                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(e.FullPath);
                        item.FileChangesHistory.Add(
                            new Model.SynchronizerFileInfo(
                                RelativePath(item.ParentDirInfo.DirectoryInfo.FullName, e.FullPath),
                                fileInfo.LastWriteTime,
                                this.GetHashString(e.FullPath),
                                item.ParentDirInfo.Id));

                        item.FileTimer.Start();
                        break;
                    }
                }
            }
            System.Windows.MessageBox.Show($"Changed. File: {e.FullPath} {e.ChangeType}");
        }

        public void SerializeFileWatcherServiceData()
        {
            //var path = String.Format("{0}JsonData\\ParentDirectoryInfo.txt", AppDomain.CurrentDomain.);
            //TextWriter textWriter = new StreamWriter("JsonData\\ParentDirectoryInfo.txt");

            //using (StreamWriter stream = )

            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT directory

            //FileStream fs = new FileStream(projectDirectory + "\\JsonData\\ParentDirectoryInfo.txt", FileMode.OpenOrCreate);
            //fs.Close();



            var listOfFiles = ListOfFilesInWatcherService.Select(item => {

                var le = item.FileChangesHistory.LastOrDefault();

                return new FileInformationToSynchronize(item.ParentDirInfo)
                {

                    FileChangesHistory = new List<SynchronizerFileInfo>()
                    {
                        new SynchronizerFileInfo(le.Path,
                            le.LastSaveTime,
                            le.Hash,
                            le.ParentFolderId)
                    }
                };
            });

            var listToSerialize =new List<FileInformationToSynchronize>();
            foreach (var item in listOfFiles)
            {
                listToSerialize.Add(item);
            }


            using (StreamWriter file = File.CreateText(projectDirectory + "\\JsonData\\ParentDirectoryInfo.json"))
            {
                file.WriteLine(JsonConvert.SerializeObject(ParentDirectoryInfo));
            }

            using (StreamWriter file = File.CreateText(projectDirectory + "\\JsonData\\ListOfFilesInWatcherService.json"))
            {
                //var listOfFiles = ListOfFilesInWatcherService.Select(item => new
                //{
                //    ParentDirectoryInfo = item.ParentDirInfo,
                //    FileChangeHistory = item.FileChangesHistory.LastOrDefault()
                //});


                file.WriteLine(JsonConvert.SerializeObject(listToSerialize));




            }

        }
        public void SerializeApplicationData() { }

        public void DeserializeFileWatcherServiceData() 
        {



            using (StreamReader file = File.OpenText(projectDirectory + "\\JsonData\\ParentDirectoryInfo.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                ParentDirectoryInfo = (List<ParentDirectoryInfo>)serializer.Deserialize(file, typeof(List<ParentDirectoryInfo>));
            }

            foreach (var item in ParentDirectoryInfo)
            {
                CreateWatcherObject(item.DirectoryInfo.FullName);
            }

            using (StreamReader file = File.OpenText(projectDirectory + "\\JsonData\\ListOfFilesInWatcherService.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                ListOfFilesInWatcherService = (List<FileInformationToSynchronize>)serializer.Deserialize(file, typeof(List<FileInformationToSynchronize>));
            }

            foreach (var item in ListOfFilesInWatcherService)
            {

                System.Timers.Timer timer = new System.Timers.Timer(10000);
                timer.Elapsed += (sender2, e2) => Timer_Elapsed(sender2, e2, item);
                timer.AutoReset = false;

                item.FileTimer = timer;

            }


        }
        
        public void DeserializeApplicationData() { }




        public FileWatcherService()
        {
        }
    }
}
