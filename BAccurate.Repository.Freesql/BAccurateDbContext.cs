using BAccurate.Repository.Freesql.Entities;
using FreeSql;
using System;

namespace BAccurate.Repository.Freesql
{
    public class BAccurateDbContext : DbContext
    {
        public BAccurateDbContext(IFreeSql freeSql) : base(freeSql, null)
        {
        }
        
         public DbSet<S_StaffinfoEntity> S_StaffinfoDb { get; set; }

        
        /// <summary>
        /// 写db与实体的映射配置
        /// </summary>
        /// <param name="freeSql"></param>
        public static void Fluent(IFreeSql freeSql){
            freeSql.CodeFirst.ConfigEntity<S_StaffinfoEntity>(cfg =>
            {
            cfg.Name("S_Staffinfo");
            
cfg.Property(m => m.Birthday).DbType("datetime").IsNullable(true);
cfg.Property(m => m.BirthPlace).DbType("varchar(200)").IsNullable(true);
cfg.Property(m => m.BloodType).DbType("varchar(10)").IsNullable(true);
cfg.Property(m => m.CardNum).DbType("int").IsNullable(false);
cfg.Property(m => m.ClassesInfoID).DbType("bigint").IsNullable(true);
cfg.Property(m => m.ContractID).DbType("smallint").IsNullable(true);
cfg.Property(m => m.CreateTime).DbType("datetime").IsNullable(true);
cfg.Property(m => m.Creator).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.Degree).DbType("varchar(10)").IsNullable(true);
cfg.Property(m => m.DepartID).DbType("bigint").IsNullable(false);
cfg.Property(m => m.distance).DbType("int").IsNullable(true);
cfg.Property(m => m.DutyID).DbType("bigint").IsNullable(false);
cfg.Property(m => m.Electricity).DbType("smallint").IsNullable(true);
cfg.Property(m => m.EntryDate).DbType("datetime").IsNullable(true);
cfg.Property(m => m.Height).DbType("smallint").IsNullable(true);
cfg.Property(m => m.HomeAddress).DbType("varchar(200)").IsNullable(true);
cfg.Property(m => m.Id).DbType("bigint").IsNullable(false);
cfg.Property(m => m.IDNumber).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.ImageData).DbType("varbinary(MAX)").IsNullable(true);
cfg.Property(m => m.ImageMimeType).DbType("varchar(10)").IsNullable(true);
cfg.Property(m => m.IsClassCaptain).DbType("varchar(5)").IsNullable(true);
cfg.Property(m => m.IsDelete).DbType("smallint").IsNullable(true);
cfg.Property(m => m.IsLeader).DbType("varchar(5)").IsNullable(true);
cfg.Property(m => m.JobTitleID).DbType("bigint").IsNullable(true);
cfg.Property(m => m.JoinWorkDate).DbType("datetime").IsNullable(true);
cfg.Property(m => m.LastUpdateTime).DbType("datetime").IsNullable(true);
cfg.Property(m => m.LightNum).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.Major).DbType("varchar(100)").IsNullable(true);
cfg.Property(m => m.MarriageID).DbType("smallint").IsNullable(true);
cfg.Property(m => m.MobileTel).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.ModifyUser).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.Name).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.Nationality).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.PerMonthCount).DbType("smallint").IsNullable(true);
cfg.Property(m => m.PerWorkTime).DbType("smallint").IsNullable(true);
cfg.Property(m => m.PoliticsID).DbType("smallint").IsNullable(true);
cfg.Property(m => m.PYJM).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.Register).DbType("varchar(200)").IsNullable(true);
cfg.Property(m => m.rr).DbType("text").IsNullable(true);
cfg.Property(m => m.School).DbType("varchar(50)").IsNullable(true);
cfg.Property(m => m.SexFlag).DbType("varchar(5)").IsNullable(true);
cfg.Property(m => m.ShiftsInfoID).DbType("bigint").IsNullable(true);
cfg.Property(m => m.Team).DbType("varchar(50)").IsNullable(true);
cfg.Property(m => m.Weight).DbType("numeric(18,5)").IsNullable(true);
cfg.Property(m => m.WorkerNum).DbType("varchar(20)").IsNullable(true);
cfg.Property(m => m.WorkID).DbType("bigint").IsNullable(false);
});

        }
        
    }
}
