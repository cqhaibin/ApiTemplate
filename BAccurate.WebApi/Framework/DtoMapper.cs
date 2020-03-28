using SAM.Framework;
using AutoMapper;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Repository.Freesql.Auth.Entities;
using BAccurate.Domain;

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
                x.CreateMap<TokendbEntity, TokenEntity>();
                var tokenTodbEntity = x.CreateMap<TokenEntity, TokendbEntity>();
                tokenTodbEntity.ForMember(m => m.Ip, (f => f.MapFrom(m => m.ClientInfo.IP)));
                tokenTodbEntity.ForMember(m => m.UserId, (f => f.MapFrom(m => m.UserInfo.Id)));
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