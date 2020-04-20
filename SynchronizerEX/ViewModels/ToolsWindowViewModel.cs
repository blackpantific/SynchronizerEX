using Prism.Commands;
using Prism.Mvvm;
using SynchronizerEX.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.ViewModels
{
    class ToolsWindowViewModel : BindableBase
    {
        private IDialogService _dialogService;

        

        private string _downloadFilesPath;
        public string DownloadFilesPath
        {
            get { return _downloadFilesPath; }
            set { SetProperty(ref _downloadFilesPath, value); }
        }

        private DelegateCommand _addDownloadPathCommand;
        public DelegateCommand AddDownloadPathCommand =>
            _addDownloadPathCommand ?? (_addDownloadPathCommand = new DelegateCommand(ExecuteAddDownloadPathCommand));

        void ExecuteAddDownloadPathCommand()
        {
            if(_dialogService.SetFolder() == true)
            {
                DownloadFilesPath = _dialogService.DownloadFilesPath;
            }
        }


        public ToolsWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            DownloadFilesPath = "null";



        }

    }
}
