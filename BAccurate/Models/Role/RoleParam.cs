using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Role
{
    /// <summary>
    /// 角色提交模型
    /// </summary>
    public class RoleParam
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 角色拥有的资源列表
        /// </summary>
        public IList<ResourceOfRole> Resources { get; set; }
    }

    public class ResourceOfRole
    {
        /// <summary>
        /// 1:选中；2：树型依赖
        /// </summary>
        public int Status { get; set; }

        public string Id { get; set; }
    }
}
