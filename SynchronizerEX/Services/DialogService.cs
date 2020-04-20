using SynchronizerEX.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynchronizerEX.Services
{
    public class DialogService : IDialogService
    {
        public string DownloadFilesPath { get; set; }

        public bool SetFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            fbd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            DialogResult result = fbd.ShowDialog();

            if(result == DialogResult.OK)
            {
                DownloadFilesPath = fbd.SelectedPath;
                return true;
            }
            return false;

        }

        public string SelectFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            fbd.ShowNewFolderButton = false;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            return String.Empty;
        }

    }
}
