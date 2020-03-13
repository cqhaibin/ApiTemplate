using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.Models.Auth;

namespace BAccurate.Repository.Freesql.Auth
{
    public class ReadAuthRepository : IReadAuthRepository
    {
        public UserInfo GetUserInfo(int userId)
        {
            return new UserInfo()
            {
                Id = 1,
                UserName = "admin"
            };
        }
    }
}
