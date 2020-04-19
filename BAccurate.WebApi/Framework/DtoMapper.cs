using SAM.Framework;
using AutoMapper;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Repository.Freesql.Auth.Entities;
using BAccurate.Domain;
using Newtonsoft.Json;
using BAccurate.Models.Auth;
using BAccurate.Models;
using BAccurate.Models.Role;

namespace BAccurate.WebApi.Framework
{
    public class DtoMapper : IFMapper
    {
        private IMapper originMapper;
        public DtoMapper(IMapper mapper)
        {
            this.originMapper = mapper;
        }
        public T Map<T>(object obj)
        {
            return this.originMapper.Map<T>(obj);
        }

        public static IConfigurationProvider CreateConfig()
        {
            return new MapperConfiguration(x =>
            {
                var tokenToEntity = x.CreateMap<TokendbEntity, TokenEntity>();
                tokenToEntity.ForMember(m => m.UserInfo, (f => f.MapFrom(m => JsonConvert.DeserializeObject<UserInfo>(m.UserInfo))));
                tokenToEntity.ForMember(m => m.ClientInfo, (f => f.MapFrom(m => JsonConvert.DeserializeObject<RequestClientInfo>(m.ClientInfo))));
                var tokenTodbEntity = x.CreateMap<TokenEntity, TokendbEntity>();
                tokenTodbEntity.ForMember(m => m.Ip, (f => f.MapFrom(m => m.ClientInfo.IP)));
                tokenTodbEntity.ForMember(m => m.UserId, (f => f.MapFrom(m => m.UserInfo.Id)));
                tokenTodbEntity.ForMember(m => m.ClientInfo, (f => f.MapFrom(m => JsonConvert.SerializeObject(m.ClientInfo))));
                tokenTodbEntity.ForMember(m => m.UserInfo, (f => f.MapFrom(m => JsonConvert.SerializeObject(m.UserInfo))));
                x.CreateMap<UserEntity, UserInfo>();
                x.CreateMap<ResourceEntity, ResourceInfo>();
                x.CreateMap<RoleResourceRelationPlusEntity, RoleAndResInfo>();

                x.DoubleMap<Models.Resource.ResourceInfo, ResourceEntity>();
                x.CreateMap<RoleEntity, RoleInfo>();
                x.CreateMap<RoleParam, RoleEntity>();
                var resRoleMap = x.CreateMap<ResourceEntity, ResourceInfoOfRole>();
                resRoleMap.ForMember(m => m.Name, opt => opt.MapFrom(r => r.ResourceName));
                resRoleMap.ForMember(m => m.ParentId, opt => opt.MapFrom(r => r.PId));

                x.CreateMap<UserEntity, Models.User.UserInfo>();
                x.CreateMap<Models.User.UserParam, UserEntity>();
                var roleUserMap = x.CreateMap<RoleEntity, Models.User.RoleInfoOfUser>();
                roleUserMap.ForMember(m => m.Id, opt => opt.MapFrom(r => r.Id));
                roleUserMap.ForMember(m => m.RoleName, opt => opt.MapFrom(r => r.RoleName));
            });
        }
    }

    public static class MapperExtend
    {
        public static void DoubleMap<TSource, TDestination>(this IMapperConfigurationExpression ex)
        {
            ex.CreateMap<TSource, TDestination>();
            ex.CreateMap<TDestination, TSource>();
        }
    }
}