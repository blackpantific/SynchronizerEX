using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SynchronizerEX.ViewModels
{
    class WelcomePageViewModel : BindableBase
    {
        private DelegateCommand _logInCommand;
        public DelegateCommand LogInCommand =>
            _logInCommand ?? (_logInCommand = new DelegateCommand(ExecuteLogInCommand));

        private INavigationService _navigationService;
        

        void ExecuteLogInCommand()
        {
            //var property_name = "PropToLogIn";
            //var a = Properties.Settings.Default.Properties[property_name];
            //SettingsProperty prop = null;

            //if (Properties.Settings.Default.Properties[property_name] != null)
            //{
            //    prop = Properties.Settings.Default.Properties[property_name];

            //}
            //else
            //{
            //    prop = new System.Configuration.SettingsProperty(property_name);
            //    prop.PropertyType = typeof(int);
            //    Properties.Settings.Default.Properties.Add(prop);
            //    Properties.Settings.Default.Save();

            //}
            //Properties.Settings.Default.Properties[property_name].DefaultValue = -1;
            //Properties.Settings.Default.Save();

            



            _navigationService.NavigateToMainPage();
        }

        public WelcomePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
