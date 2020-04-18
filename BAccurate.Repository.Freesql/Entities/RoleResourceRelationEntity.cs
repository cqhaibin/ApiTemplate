namespace BAccurate.Repository.Freesql.Entities
{
    public class RoleResourceRelationEntity:BaseEntity<int>
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string ResourceId { get; set; }

        /// <summary>
        /// 1:选中；2：树型依赖
        /// </summary>
        public int Status { get; set; }
 
    }
    public class RoleResourceRelationPlusEntity : RoleResourceRelationEntity
    {
        public string ResourceCode { get; set; }
    }
}
