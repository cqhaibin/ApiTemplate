using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Auth
{
    public class ResourceInfo
    {
        public string Id { get; set; }
        public string ResourceCode {get;set;}
        public string ResourceName { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// 1：菜单；2：接口
        /// </summary>
        public int ResourceType { get; set; }
        public bool Enable { get; set; }
        public string ParentId { get; set; }
        /// <summary>
        /// 扩展字段：json字符串
        /// </summary>
        public string Config { get; set; }
        public int OrderNum { get; set; }
    }

    /// <summary>
    /// 资源与角色的关系
    /// </summary>
    public class RoleAndResInfo
    {

        public string ResourceId { get; set; }

        public string ResourceCode { get; set; }

        public int RoleId { get; set; }
    }
}
