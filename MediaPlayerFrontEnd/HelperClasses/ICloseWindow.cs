using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayerPL.HelperClasses
{
    interface ICloseWindow
    {
        Action Close { get; set; }
        bool DialogResult { get; set; }
    }
}
