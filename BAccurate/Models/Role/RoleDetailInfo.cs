using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Role
{
    public class RoleDetailInfo
    {
        public RoleInfo Info { get; set; }

        public IList<ResourceInfoOfRole> Resources { get; set; }
    }

    public class ResourceInfoOfRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// 1:选中；2：树型依赖
        /// </summary>
        public int Status { get; set; }
    }
}
