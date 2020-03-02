using BAccurate.AppServices;
using BAccurate.Repository.Freesql;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Models.S_Staffinfo;
using SAM.Framework;
using SAM.Framework.Result;
using System.Collections.Generic;
using System;

namespace BAccurate.AppService.Implement
{
    public class S_StaffinfoService : IS_StaffinfoService
    {
        private BAccurateDbContext context;
        private IFMapper mapper;

        public S_StaffinfoService(BAccurateDbContext context, IFMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ResultListOfPageInfo<S_StaffinfoInfo> GetAll(S_StaffinfoQuery query)
        {
            string sorts = query.Sorts;
            if (string.IsNullOrEmpty(sorts))
            {
                sorts = " id ";
            }
            var select = this.context.S_StaffinfoDb.Select;
            if (!string.IsNullOrEmpty(query.BirthPlace))
            {
                select = select.Where(m => m.BirthPlace == query.BirthPlace);
            }
            if (!string.IsNullOrEmpty(query.BloodType))
            {
                select = select.Where(m => m.BloodType == query.BloodType);
            }
            if (query.CardNum != 0)
            {
                select = select.Where(m => m.CardNum == query.CardNum);
            }
            if (query.ClassesInfoID.HasValue)
            {
                select = select.Where(m => m.ClassesInfoID == query.ClassesInfoID.Value);
            }
            if (!string.IsNullOrEmpty(query.Creator))
            {
                select = select.Where(m => m.Creator == query.Creator);
            }
            if (!string.IsNullOrEmpty(query.Degree))
            {
                select = select.Where(m => m.Degree == query.Degree);
            }
            if (query.DepartID != 0)
            {
                select = select.Where(m => m.DepartID == query.DepartID);
            }
            if (query.distance.HasValue)
            {
                select = select.Where(m => m.distance == query.distance.Value);
            }
            if (query.DutyID != 0)
            {
                select = select.Where(m => m.DutyID == query.DutyID);
            }
            if (!string.IsNullOrEmpty(query.HomeAddress))
            {
                select = select.Where(m => m.HomeAddress == query.HomeAddress);
            }
            if (query.Id != 0)
            {
                select = select.Where(m => m.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.IDNumber))
            {
                select = select.Where(m => m.IDNumber == query.IDNumber);
            }
            if (!string.IsNullOrEmpty(query.ImageMimeType))
            {
                select = select.Where(m => m.ImageMimeType == query.ImageMimeType);
            }
            if (!string.IsNullOrEmpty(query.IsClassCaptain))
            {
                select = select.Where(m => m.IsClassCaptain == query.IsClassCaptain);
            }
            if (!string.IsNullOrEmpty(query.IsLeader))
            {
                select = select.Where(m => m.IsLeader == query.IsLeader);
            }
            if (query.JobTitleID.HasValue)
            {
                select = select.Where(m => m.JobTitleID == query.JobTitleID.Value);
            }
            if (!string.IsNullOrEmpty(query.LightNum))
            {
                select = select.Where(m => m.LightNum == query.LightNum);
            }
            if (!string.IsNullOrEmpty(query.Major))
            {
                select = select.Where(m => m.Major == query.Major);
            }
            if (!string.IsNullOrEmpty(query.MobileTel))
            {
                select = select.Where(m => m.MobileTel == query.MobileTel);
            }
            if (!string.IsNullOrEmpty(query.ModifyUser))
            {
                select = select.Where(m => m.ModifyUser == query.ModifyUser);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                select = select.Where(m => m.Name == query.Name);
            }
            if (!string.IsNullOrEmpty(query.Nationality))
            {
                select = select.Where(m => m.Nationality == query.Nationality);
            }
            if (!string.IsNullOrEmpty(query.PYJM))
            {
                select = select.Where(m => m.PYJM == query.PYJM);
            }
            if (!string.IsNullOrEmpty(query.Register))
            {
                select = select.Where(m => m.Register == query.Register);
            }
            if (!string.IsNullOrEmpty(query.rr))
            {
                select = select.Where(m => m.rr == query.rr);
            }
            if (!string.IsNullOrEmpty(query.School))
            {
                select = select.Where(m => m.School == query.School);
            }
            if (!string.IsNullOrEmpty(query.SexFlag))
            {
                select = select.Where(m => m.SexFlag == query.SexFlag);
            }
            if (query.ShiftsInfoID.HasValue)
            {
                select = select.Where(m => m.ShiftsInfoID == query.ShiftsInfoID.Value);
            }
            if (!string.IsNullOrEmpty(query.Team))
            {
                select = select.Where(m => m.Team == query.Team);
            }
            if (!string.IsNullOrEmpty(query.WorkerNum))
            {
                select = select.Where(m => m.WorkerNum == query.WorkerNum);
            }
            if (query.WorkID != 0)
            {
                select = select.Where(m => m.WorkID == query.WorkID);
            }


            long total = select.Count();
            var entites = select.OrderBy(sorts).Page(query.PageIndex, query.PageSize).ToList();
            return new ResultListOfPageInfo<S_StaffinfoInfo>()
            {
                List = this.mapper.Map<List<S_StaffinfoInfo>>(entites),
                Total = total
            };
        }

        public ResultInfo Remove(Int64 id)
        {
            this.context.S_StaffinfoDb.Remove(m => m.Id == id);
            this.context.SaveChanges();
            return new ResultInfo();
        }

        public ResultInfo Save(S_StaffinfoInfo info)
        {
            if (this.context.S_StaffinfoDb.Select.Any(m => m.Id == info.Id))
            {
                this.context.S_StaffinfoDb.Update(this.mapper.Map<S_StaffinfoEntity>(info));
            }
            else
            {
                this.context.S_StaffinfoDb.Add(this.mapper.Map<S_StaffinfoEntity>(info));
            }
            this.context.SaveChanges();
            return new ResultInfo();
        }
    }
}
