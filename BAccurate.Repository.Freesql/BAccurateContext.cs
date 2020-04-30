using BAccurate.Domain;
using BAccurate.Repository.Freesql.Auth.Entities;
using BAccurate.Repository.Freesql.Entities;
using FreeSql;
using SAM.Framework;
using System;

namespace BAccurate.Repository.Freesql
{
    public class BAccurateContext : DbContext
    {
        public BAccurateContext(IFreeSql freeSql, IFMapper fMapper) : base(freeSql, null)
        {
            this.TokenRepository = new Auth.TokenRepository(freeSql, fMapper);
        }

        public ITokenRepository TokenRepository { get; protected set; }

        public DbSet<ResourceEntity> ResourceRepository { get; set; }

        public DbSet<RoleEntity> RoleRepository { get; set; }

        public DbSet<RoleResourceRelationEntity> RoleResRepository { get; set; }

        public DbSet<RoleResourceRelationPlusEntity> RoleResPlusRepository { get; set; }

        public DbSet<UserEntity> UserRepository { get; set; }

        public DbSet<UserRoleRelationEntity> UserRoleRepository { get; set; }

         
        /// <summary>
        /// 写db与实体的映射配置
        /// </summary>
        /// <param name="freeSql"></param>
        public static void Fluent(IFreeSql freeSql)
        {
            freeSql.CodeFirst.ConfigEntity<TokendbEntity>(cfg =>
            {
                cfg.Name("T_Tokens");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
                cfg.Property(m => m.ClientInfo).StringLength(-1);
                cfg.Property(m => m.UserInfo).StringLength(-1);
            });
            freeSql.CodeFirst.ConfigEntity<ResourceEntity>((cfg) =>
            {
                cfg.Name("T_Resources");
                cfg.Property(m => m.Config).StringLength(-1);
            });
            freeSql.CodeFirst.ConfigEntity<RoleEntity>((cfg) =>
            {
                cfg.Name("T_Roles");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
            });
            freeSql.CodeFirst.ConfigEntity<RoleResourceRelationEntity>((cfg) =>
            {
                cfg.Name("T_RoleResRelations");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
            });
            freeSql.CodeFirst.ConfigEntity<RoleResourceRelationPlusEntity>((cfg) =>
            {
                cfg.Name("T_RoleResRelations");
                cfg.DisableSyncStructure(true);
            });
            freeSql.CodeFirst.ConfigEntity<UserEntity>((cfg) =>
            {
                cfg.Name("T_Users");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
                cfg.Property(m => m.Config).StringLength(-1);
            });
            freeSql.CodeFirst.ConfigEntity<UserRoleRelationEntity>((cfg) =>
            {
                cfg.Name("T_UserRoles");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
            });
            freeSql.CodeFirst.SyncStructure<TokendbEntity>();
            freeSql.CodeFirst.SyncStructure<ResourceEntity>();
            freeSql.CodeFirst.SyncStructure<RoleEntity>();
            freeSql.CodeFirst.SyncStructure<RoleResourceRelationEntity>();
            freeSql.CodeFirst.SyncStructure<UserEntity>();
            freeSql.CodeFirst.SyncStructure<UserRoleRelationEntity>();
        }
    }
}
