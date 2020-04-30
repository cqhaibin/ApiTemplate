namespace BAccurate.Repository.Freesql.Entities
{
    public class RoleEntity:BaseEntity<int>
    { 
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色序号
        /// </summary>
        public string RoleSeq { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;
 
    }
}
