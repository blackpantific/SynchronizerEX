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
        public static int UserId 
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

    }
}
