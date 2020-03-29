using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Repository.Freesql.Auth.Entities
{
    public class TokendbEntity
    {
        public int Id { get; set; }

        public string Ip { get; set; }

        public bool IsLogin { get; set; }

        public int UserId { get; set; }

        public string UserInfo { get; set; }

        public string ClientInfo { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime ExpiredTime { get; set; }
    }
}
