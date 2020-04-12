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
        public MainWindow()
        {
            InitializeComponent();

            
            if(SettingsHelper.UserId == -1)
            {
                WelcomePage wp = new WelcomePage();
                this.ApplicationMainFrame.NavigationService.Navigate(wp);
            }
            else
            {
                this.ApplicationMainFrame.NavigationService.Navigate(new MainPage());
            }
                
            
            

        }
    }
}
