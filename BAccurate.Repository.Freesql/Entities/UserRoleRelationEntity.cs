namespace BAccurate.Repository.Freesql.Entities
{
    public class UserRoleRelationEntity:BaseEntity<int>
    {
         

        public int UserId { get; set; }

        public int RoleId { get; set; }
 
    }
}
