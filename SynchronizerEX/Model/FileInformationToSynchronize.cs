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
        public ParentDirectoryInfo ParentDirInfo { get; set; }
        public System.Timers.Timer FileTimer { get; set; }
        public FileInformationToSynchronize(ParentDirectoryInfo parentDirectoryInfo)
        {
            FileChangesHistory = new List<SynchronizerFileInfo>();
            ParentDirInfo = parentDirectoryInfo;
        }
    }
}
