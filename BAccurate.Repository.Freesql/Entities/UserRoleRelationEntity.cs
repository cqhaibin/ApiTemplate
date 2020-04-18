namespace BAccurate.Repository.Freesql.Entities
{
    public class UserRoleRelationEntity:BaseEntity<int>
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
 
    }
}
