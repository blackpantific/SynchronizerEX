using SynchronizerEX.Contracts;
using SynchronizerEX.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynchronizerEX.Views
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page, INavigationService
    {
        private IDialogService _dialogService;
        private IFileWatcherService _watcherService;
        public WelcomePage(IDialogService dialogService, IFileWatcherService watcherService)
        {
            InitializeComponent();
            _dialogService = dialogService;
            _watcherService = watcherService;
            DataContext = new WelcomePageViewModel(this);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainPage();
        }

        public void NavigateToMainPage()
        {
            MainPage mp = new MainPage(_dialogService, _watcherService);
            this.NavigationService.Navigate(mp);
        }
    }
}
