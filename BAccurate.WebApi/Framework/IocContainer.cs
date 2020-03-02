using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using BAccurate;
using AutoMapper;
using System.Web.Http;
using Autofac.Integration.WebApi;
using BAccurate.Repository.Freesql;
using System.Configuration;
using BAccurate.WebApi.Framework;
using SAM.Framework;
using Autofac.Core;

namespace BAccurate.WebApi.Framework
{
    public static class IocContainer
    {
        public static IContainer IOCContainer;

        public static void Builder()
        {
            var builder = new ContainerBuilder();
            var cfg = CreateCfg();
            
            builder.RegisterInstance(cfg).SingleInstance();

            #region 注册freesql

            IFreeSql freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.SqlServer, cfg.AccurateConn)
                .UseAutoSyncStructure(true)
                .Build();
            builder.RegisterInstance(freeSql).SingleInstance();
            BAccurateContext.Fluent(freeSql);
            builder.RegisterType<BAccurateContext>().InstancePerLifetimeScope();
            List<NamedParameter> parameters = new List<NamedParameter>();

            IFreeSql nfrs = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.SqlServer, cfg.AccurateConn)
                .UseAutoSyncStructure(true)
                .Build();
            BAccurateDbContext.Fluent(nfrs);
            builder.RegisterType<BAccurateDbContext>().WithParameter(new NamedParameter("freeSql", nfrs) { });

            #endregion

            #region 注册服务
            var allAssemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var assembiles = allAssemblies.Where(x => x.ManifestModule.Name.Contains("BAccurate")).ToArray();
            builder.RegisterAssemblyTypes(assembiles)
                .Where(type => typeof(ITransient).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency();
            #endregion


            //注册controller
            builder.RegisterApiControllers(assembiles);

            // 注册AutoMapper
            builder.Register(c => DtoMapper.CreateConfig()).SingleInstance();
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
            builder.RegisterType<DtoMapper>().As<IFMapper>().InstancePerLifetimeScope();

            IOCContainer = builder.Build();

            // webApi部分注册
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(IOCContainer);
        }

        public static BAccurateConfig CreateCfg()
        {
            var cfg = new BAccurateConfig();
            cfg.AccurateConn = ConfigurationManager.ConnectionStrings["accConn"].ConnectionString;
            return cfg;
        }
    }
}