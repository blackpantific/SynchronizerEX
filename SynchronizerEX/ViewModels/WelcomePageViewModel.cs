using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SynchronizerEX.ViewModels
{
    class WelcomePageViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        private DelegateCommand<string> _logInCommand;
        public DelegateCommand<string> LogInCommand =>
            _logInCommand ?? (_logInCommand = new DelegateCommand<string>(ExecuteLogInCommand));

        

        void ExecuteLogInCommand(string navigatePath)
        {
            if(navigatePath != null)
            {
                _regionManager.RequestNavigate("WelcomePageRegion", navigatePath);
            }
        }



        public WelcomePageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
           
        }
    }
}
