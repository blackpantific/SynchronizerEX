using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Model
{
    public class FileInformationToSynchronize
    {
        public List<SynchronizerFileInfo> FileChangesHistory { get; set; }
        //public int ParentHash { get; set; }
        public ParentDirectoryInfo ParentDirInfo { get; set; }
        public FileInformationToSynchronize(/*int parentHash*/ ParentDirectoryInfo parentDirectoryInfo)
        {
            FileChangesHistory = new List<SynchronizerFileInfo>();
            ParentDirInfo = parentDirectoryInfo;
            //ParentHash = parentHash;
        }
        //public bool IsBusy { get; set; }
    }
}
