using BAccurate.Models;
using BAccurate.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Domain
{
    public class TokenEntity
    {
        public string Token { get; set; }
        public RequestClientInfo ClientInfo { get; set; }

        public UserInfo UserInfo { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime ExpiredTime { get; set; }
    }
}
