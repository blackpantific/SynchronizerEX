using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Contracts
{
    public interface IDialogService
    {
        string DownloadFilesPath { get; set; }
        bool SetFolder();
        string SelectFolder();
    }
}
