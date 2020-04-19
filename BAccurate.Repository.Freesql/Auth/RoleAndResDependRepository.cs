using BAccurate.Domain;
using BAccurate.Models.Auth;
using SAM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Repository.Freesql.Entities;

namespace BAccurate.Repository.Freesql.Auth
{
    public class RoleAndResDependRepository : IRoleAndResDepend
    {
        private BAccurateContext context;
        private IFMapper mapper;
        public RoleAndResDependRepository(BAccurateContext context, IFMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<ResourceInfo> GetAllResourceInfos()
        {
            var entities = this.context.ResourceRepository.Select.ToList();
            return this.mapper.Map<List<ResourceInfo>>(entities);
        }

        public List<RoleAndResInfo> GetAllRoleAndRes()
        {
            var entities = this.context.Orm.Select<RoleResourceRelationEntity, ResourceEntity>()
                .LeftJoin((a, b) => a.ResourceId == b.Id)
                .ToList<RoleResourceRelationPlusEntity>();
            
            return this.mapper.Map<List<RoleAndResInfo>>(entities);
        }
    }
}
