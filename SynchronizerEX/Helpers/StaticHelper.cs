using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizerEX.Helpers
{
    public class StaticHelper
    {
        public static string WorkingDirectory { get; set; }
        public static string ProjectDirectory { get; set; }

        public StaticHelper()
        {
           WorkingDirectory  = Environment.CurrentDirectory;
           ProjectDirectory = Directory.GetParent(WorkingDirectory).Parent.FullName;
        }
    }
}
