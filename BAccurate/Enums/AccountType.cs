using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Enums
{
    [Flags]
    public enum AccountType:short
    {
        [Description("用户")]
        User = 1
    }
}
