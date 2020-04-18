using System.Collections.Generic;

namespace BAccurate.Models.Auth
{
    public class IdentityInfo
    {
        public IList<MenuInfo> Menus { get; set; }

        public UserInfo User { get; set; }
    }
}
