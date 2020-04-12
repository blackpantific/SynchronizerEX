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
        public WelcomePage()
        {
            InitializeComponent();

            DataContext = new WelcomePageViewModel(this);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainPage();
        }

        public void NavigateToMainPage()
        {
            MainPage mp = new MainPage();
            this.NavigationService.Navigate(mp);
        }
    }
}
