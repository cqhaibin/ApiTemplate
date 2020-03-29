using SAM.Framework;
using AutoMapper;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Repository.Freesql.Auth.Entities;
using BAccurate.Domain;
using Newtonsoft.Json;
using BAccurate.Models.Auth;
using BAccurate.Models;

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