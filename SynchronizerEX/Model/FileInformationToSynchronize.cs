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
        public FileInformationToSynchronize()
        {
            FileChangesHistory = new List<SynchronizerFileInfo>();
        }
        //public bool IsBusy { get; set; }
    }
}
