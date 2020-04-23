using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Model
{
    public class SynchronizerFileInfo
    {
        public string FullPath { get; set; }
        public DateTime LastSaveTime { get; set; } // файл был изменен
        public int Hash { get; set; }
        public SynchronizerFileInfo(string fullPath, DateTime lastSaveTime, int hash)
        {
            FullPath = fullPath;
            LastSaveTime = lastSaveTime;
            Hash = hash;
        }
    }
}
