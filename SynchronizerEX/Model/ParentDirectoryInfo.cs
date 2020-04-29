using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Model
{
    public class ParentDirectoryInfo
    {
        public int Id { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public ParentDirectoryInfo(int id, DirectoryInfo directoryInfo)
        {
            Id = id;
            DirectoryInfo = directoryInfo;
        }
    }
}
