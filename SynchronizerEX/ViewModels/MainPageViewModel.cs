using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using SynchronizerEX.Model;
using SynchronizerEX.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
    //    public List<string> ListOfFilesToTrack { get; set; }

        private DelegateCommand _toolsCommand;
        public DelegateCommand ToolsCommand =>
            _toolsCommand ?? (_toolsCommand = new DelegateCommand(ExecuteToolsCommand));

        private DelegateCommand _addFilesCommand;
        public DelegateCommand AddFilesCommand =>
            _addFilesCommand ?? (_addFilesCommand = new DelegateCommand(ExecuteAddFilesCommand));

        void ExecuteAddFilesCommand()
        {
            //ListOfFilesToTrack.Add(_dialogService.SelectFolder());
            var result = _dialogService.SelectFolder();
            if (Directory.Exists(result))
            {
                //DirectoryInfo directoryInfo = new DirectoryInfo(result);

                //foreach (var item in directoryInfo.GetFiles())
                //{
                //    ListOfFilesToTrack.Add(item.FullName);
                //}


                _watcherService.CreateWatcher(result);
            }
        }
             
        private IToolWindowNavigation _toolWindowNavigation;
        private IDialogService _dialogService;
        private IFileWatcherService _watcherService;

        void ExecuteToolsCommand()
        {
            _toolWindowNavigation.NavigateToToolWindow();
        }

        public MainPageViewModel(IToolWindowNavigation toolWindowNavigation, IDialogService dialogService,
            IFileWatcherService watcherService)
        {
            _toolWindowNavigation = toolWindowNavigation;
            _dialogService = dialogService;
            _watcherService = watcherService;

            //ListOfFilesToTrack = new List<string>();

        }

    }
}
