using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Auth
{
    public class LoginResultInfo
    {
        public string Token { get; set; }

        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string RealName { get; set; }

        public string UserName { get; set; }

        public int Id { get; set; }

        public string Config { get; set; }

        public string MobilePhone { get; set; }
    }
}
