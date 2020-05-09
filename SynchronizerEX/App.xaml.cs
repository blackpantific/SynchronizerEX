using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using SynchronizerEX.Contracts;
using SynchronizerEX.Services;
using SynchronizerEX.ViewModels;
using SynchronizerEX.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SynchronizerEX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override Window CreateShell()
        {
            //return Container.Resolve<MainWindow>();
            return null;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.RegisterSingleton<INavigationService, WelcomePage>();
            containerRegistry.RegisterSingleton<IToolWindowNavigation, MainPage>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<IFileWatcherService, FileWatcherService>();



        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var fileWatcher = Container.Resolve<FileWatcherService>();
            //fileWatcher.DeserializeFileWatcherServiceData();



            MainWindow = Container.Resolve<MainWindow>();
            MainWindow.Closing += MainWindow_Closing;


            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = SynchronizerEX.Properties.Resources.system_tray;
            _notifyIcon.Visible = true;

            CreateContextMenu();

            //задавание иконки при помощи путей к месту, где она лежит(по канону)

            //notifyIcon = new NotifyIcon();
            //using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
            //    "<project namespace>.<folder path>" + "filename.ico"))
            //{
            //    notifyIcon.Icon = new Icon(stream);
            //}




        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {

            var fileWatcherService = Container.Resolve<FileWatcherService>();
            fileWatcherService.SerializeFileWatcherServiceData();

            base.OnExit(e);
        }

        //protected override void OnInitialized()
        //{
        ////    var mainWindow = Container.Resolve<MainWindow>();
        ////    mainWindow.Show();

        //    //var shellWindow = Container.Resolve<ShellWindow>();
        //    //shellWindow.Show();
        //    //MainWindow = shellWindow.ParentOfType<Window>();

        //    //можно добавить некст строки, если че-то не выйдет
        //    //// there lines was not executed because of null Shell - so must duplicate here. Originally called from PrismApplicationBase.Initialize
        //    //RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
        //    //RegionManager.UpdateRegions();
        //    //InitializeModules();


        //    base.OnInitialized();

            
        //    //срабатывает раньше, чем OnStartup()
        //}

    }
}
