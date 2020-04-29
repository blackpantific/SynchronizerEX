using SynchronizerEX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Model
{
    public class RaiseWindowMessage
    {
        public string WindowName { get; set; }
        public bool ShowAsDialog { get; set; }
        public MainPageViewModel Vm { get; set; }
    }
}
