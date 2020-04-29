using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Model
{
    public class SynchronizerFileInfo
    {
        public string Path { get; set; }//если поменяется имя дериктории в контролируемой Watcher'ом директории и следовательно поменяется путь
        public DateTime LastSaveTime { get; set; } // файл был изменен
        public string Hash { get; set; }
        public int ParentFolderId { get; set; }
        public SynchronizerFileInfo(string fullPath, DateTime lastSaveTime, string hash, int parentId)
        {
            Path = fullPath;
            LastSaveTime = lastSaveTime;
            Hash = hash;
            ParentFolderId = parentId;
        }
    }
}
