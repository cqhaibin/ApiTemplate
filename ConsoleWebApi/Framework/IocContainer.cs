using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using BAccurate;
using AutoMapper;
using System.Web.Http;
using Autofac.Integration.WebApi;
using BAccurate.Repository.Freesql;
using System.Configuration; 
using SAM.Framework;
using Autofac.Core;
using BAccurate.Implement.Domain;
using BAccurate.Domain;

namespace ConsoleWebApi.Framework
{
    public static class IocContainer
    {
        public static IContainer IOCContainer;

        public static void Builder(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();
            var cfg = CreateCfg();

            builder.RegisterInstance(cfg).SingleInstance();

            #region 注册freesql

            var build = new FreeSql.FreeSqlBuilder();
            if (cfg.DbType == ConstCfg.DbType_MSsql)
            {
                build = build.UseConnectionString(FreeSql.DataType.SqlServer, cfg.AccurateConn);
            }
            else if (cfg.DbType == ConstCfg.DbType_Sqlite)
            {
                build = build.UseConnectionString(FreeSql.DataType.Sqlite, cfg.AccurateConn);
            }
            else if (cfg.DbType == ConstCfg.DbType_Mysql)
            {
                build = build.UseConnectionString(FreeSql.DataType.MySql, cfg.AccurateConn);
            }
            IFreeSql freeSql = build.UseAutoSyncStructure(false)
             .Build();
            builder.RegisterInstance(freeSql).SingleInstance();
            BAccurateContext.Fluent(freeSql);
            builder.RegisterType<BAccurateContext>().InstancePerLifetimeScope();

            #endregion

            #region 注册服务
            AppDomain.CurrentDomain.Load("BAccurate.AppService.Implement");
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembiles = allAssemblies.Where(x => x.ManifestModule.Name.Contains("BAccurate")).ToArray();
            builder.RegisterAssemblyTypes(assembiles)
                .Where(type => typeof(ITransient).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency();
            #endregion


            //注册controller
            builder.RegisterApiControllers(allAssemblies);

            // 注册AutoMapper
            builder.Register(c => DtoMapper.CreateConfig()).SingleInstance();
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
            builder.RegisterType<DtoMapper>().As<IFMapper>().InstancePerLifetimeScope();

            // 注册核心业务
            builder.RegisterType<OnlineUserMgr>().As<IOnlineUserMgr>().SingleInstance();



            IOCContainer = builder.Build();

            // webApi部分注册
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(IOCContainer);

        }

        public static BAccurateConfig CreateCfg()
        {
            var cfg = new BAccurateConfig();
            cfg.AccurateConn = ConfigurationManager.ConnectionStrings["ocmConn"].ConnectionString;
            cfg.DbType = ConfigurationManager.AppSettings["dbType"].Trim();
            return cfg;
        }
    }
}