using Prism.Commands;
using Prism.Mvvm;
using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using SynchronizerEX.Managers;
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

            if(SettingsHelper.AutorunSwitch == -1)
            {
                StartUpManager.AddApplicationToCurrentUserStartup();
                SettingsHelper.AutorunSwitch = 1;
            }




        }


        public ToolsWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            DownloadFilesPath = "null";



        }

    }
}
