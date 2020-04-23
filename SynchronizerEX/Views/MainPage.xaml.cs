using Prism.Regions;
using SynchronizerEX.Contracts;
using SynchronizerEX.Helpers;
using SynchronizerEX.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IToolWindowNavigation
    {
        //using loaded instead of OnNavigatingTo
        public ToolsWindow tw;
        private IDialogService _dialogService;
        private IFileWatcherService _watcherService;

        public ObservableCollection<Phone> Phones { get; set; }

        

        public MainPage(IDialogService dialogService, IFileWatcherService watcherService)
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            _dialogService = dialogService;
            _watcherService = watcherService;
            DataContext = new MainPageViewModel(this, _dialogService, _watcherService);

            

            Phones = new ObservableCollection<Phone>
        {
            new Phone {Id=1, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/File-Header.png", Title="iPhone 6S", Company="Apple" },
            new Phone {Id=2, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/Folder-vector-icon.png", Title="Lumia 950", Company="Microsoft" },
            new Phone {Id=3, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/File-Header.png", Title="Nexus 5X", Company="Google" },
            new Phone {Id=4, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/File-Header.png", Title="Galaxy S6", Company="Samsung"},
            new Phone {Id=5, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/Folder-vector-icon.png", Title="iPhone 6S", Company="Apple" },
            new Phone {Id=6, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/File-Header.png", Title="Lumia 950", Company="Microsoft" },
            new Phone {Id=7, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/Folder-vector-icon.png", Title="Nexus 5X", Company="Google" },
            new Phone {Id=8, ImagePath="pack://application:,,,/SynchronizerEX;component/Resources/Folder-vector-icon.png", Title="Galaxy S6", Company="Samsung"}
        };
            phonesList.ItemsSource = Phones;
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SettingsHelper.WelcomeWindowSwitch == -1)
            {
                var uww = new UserWelcomeWindow();
                uww.Show();
                SettingsHelper.WelcomeWindowSwitch = 1;
            }
        }

        public void NavigateToToolWindow()
        {

            tw = new ToolsWindow();
            tw.Show();
            tw.WindowStartupLocation = WindowStartupLocation.Manual;
            tw.Top = 100;
            tw.Left = 500;
            tw.Owner = Window.GetWindow(this);
        }

        
    }

    public class Phone
    {
        public int Id { get; set; }
        public string Title { get; set; } // модель телефона
        public string Company { get; set; } // производитель
        public string ImagePath { get; set; } // путь к изображению
    }
}
