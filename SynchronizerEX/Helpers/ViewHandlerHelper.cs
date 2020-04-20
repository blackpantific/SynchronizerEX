using GalaSoft.MvvmLight.Messaging;
using SynchronizerEX.Model;
using SynchronizerEX.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Helpers
{
    public class ViewHandlerHelper
    {
        public ViewHandlerHelper()
        {
            Messenger.Default.Register<RaiseWindowMessage>(this, raiseNextWindow);
        }

        private void raiseNextWindow(RaiseWindowMessage obj)
        {
            // determine which window to raise and show it
            switch (obj.WindowName)
            {
                case "ToolsWindow":
                    ToolsWindow view = new ToolsWindow();
                    if (obj.ShowAsDialog)
                        view.ShowDialog();
                    else
                    { 
                        view.Show();
                    }
                    
                    break;
                // some other case here...
                default:
                    break;
            }
        }
    }
}
