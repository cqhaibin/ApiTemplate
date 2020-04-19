using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.Models.Auth;
using SAM.Framework;

namespace BAccurate.Repository.Freesql.Auth
{
    public class ReadAuthRepository : IReadAuthRepository
    {
        private BAccurateContext context;
        private IFMapper mapper;
        public ReadAuthRepository(BAccurateContext context, IFMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public UserInfo GetUserInfo(int userId)
        {
            var entity = this.context.UserRepository.Select.Where(m => m.Id == userId).ToOne();
            var userInfo = this.mapper.Map<UserInfo>(entity);
            var roles = this.context.UserRoleRepository.Select.Where(m => m.UserId == userId).ToList();
            if (roles != null)
            {
                userInfo.Roles = roles.Select(m => m.RoleId).ToArray();
            }
            return userInfo;
        }
    }
}
