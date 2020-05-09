using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SynchronizerEX.Helpers
{
    public static class SettingsHelper
    {
        public static int LogInSwitch 
        { 
            get 
            {
                return Properties.Settings.Default.CanNavigate;
            }
            set 
            {
                Properties.Settings.Default.CanNavigate = value;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            } 
        }

        public static int WelcomeWindowSwitch
        {
            get
            {
                return Properties.Settings.Default.CanGreet;
            }
            set
            {
                Properties.Settings.Default.CanGreet = value;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }

        public static int AutorunSwitch
        {
            get
            {
                return Properties.Settings.Default.CanAutorun;
            }
            set
            {
                Properties.Settings.Default.CanAutorun = value;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }


    }
}
