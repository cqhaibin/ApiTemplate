using System.Collections.Generic;

namespace BAccurate.Models.User
{
    public class UserDetailInfo
    {
        public UserInfo UserInfo { get; set; }

        public IList<RoleInfoOfUser> Roles { get; set; }
        
        public UserDetailInfo()
        {
            this.Roles = new List<RoleInfoOfUser>();
        }
    }

    public class RoleInfoOfUser
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        public string RoleName { get; set; }
    }
}
