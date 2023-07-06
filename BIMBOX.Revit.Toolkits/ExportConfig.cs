using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension
{
    public class ExportConfig
    {
        public string OutputPath { get; set; }

        public int LevelofDetail { get; set; }

        public List<int> Filter { get; set; }

        public bool UnicodeSupport { get; set; }
    }
}
