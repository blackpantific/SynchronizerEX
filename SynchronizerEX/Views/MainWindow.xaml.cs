using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SynchronizerEX.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDialogService _dialogService;
        private IFileWatcherService _watcherService;


        public MainWindow(IDialogService dialogService, IFileWatcherService watcherService)
        {
            InitializeComponent();

            _dialogService = dialogService;
            _watcherService = watcherService;
            
            if(SettingsHelper.LogInSwitch == -1)
            {
                WelcomePage wp = new WelcomePage(_dialogService, _watcherService);
                this.ApplicationMainFrame.NavigationService.Navigate(wp);
                SettingsHelper.LogInSwitch = 1;
            }
            else
            {
                this.ApplicationMainFrame.NavigationService.Navigate(new MainPage(_dialogService, _watcherService));
            }
                
            
            

        }
    }
}
