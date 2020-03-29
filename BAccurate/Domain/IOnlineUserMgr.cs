using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Domain
{
    /// <summary>
    /// 在线用户管理接口
    /// </summary>
    public interface IOnlineUserMgr
    {
        /// <summary>
        /// 将用户添加到在线用户列表，此方法需要对登入信息持久化
        /// </summary>
        /// <param name="entity"></param>
        void Add(IUserEntity entity);
        /// <summary>
        /// 根据token移除对应的用户，此方法需要对登出信息持久化
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool Remove(string token);
        /// <summary>
        /// 根据用户Id移除用户，此方法需要对登出信息持久化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(int id);
        /// <summary>
        /// 从持久化层恢复在线用户
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有在线用户
        /// </summary>
        IList<IUserEntity>  GetAll();

        IUserEntity Get(int userId);

        List<Models.Auth.ResourceInfo>  GetAllRes();

        List<Models.Auth.RoleAndResInfo> GetAllRoleAndRes();
        
    }
}
