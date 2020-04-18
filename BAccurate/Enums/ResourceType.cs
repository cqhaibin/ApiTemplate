using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Enums
{
    [Flags]
    public enum ResourceType : short
    {
        [Description("资源")]
        Resource = 1,
        [Description("菜单")]
        Menu = 2,
        [Description("公用资源")]
        Public = 4
    }
}
