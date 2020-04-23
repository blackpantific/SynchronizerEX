using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using SynchronizerEX.Contracts;
using SynchronizerEX.Services;
using SynchronizerEX.ViewModels;
using SynchronizerEX.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SynchronizerEX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.Register<INavigationService, WelcomePage>();
            containerRegistry.Register<IToolWindowNavigation, MainPage>();
            containerRegistry.Register<IDialogService, DialogService>();
            containerRegistry.Register<IFileWatcherService, FileWatcherService>();
        }
        
       
    }
}
