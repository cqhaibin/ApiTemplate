using SAM.Framework;
using AutoMapper;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Models.Test;

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
                x.CreateMap<TestEntity, TestInfo>();   
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