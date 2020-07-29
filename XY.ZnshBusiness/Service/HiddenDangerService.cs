using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class HiddenDangerService: IHiddenDangerService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public HiddenDangerService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region 获取数据
        /// <summary>
        /// 根据条件列表并分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        public List<HiddenDangersReportEntity> GetPageListByCondition(string states,string userid, string qdstates, string orgid, string currOrgId, string riskpointname, string hiddenlevel, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersReportEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<HiddenDangersReportEntity, CheckTableEntity,DataDictEntity>((de1, de2,de3) => new object[] {
                JoinType.Left,de1.CheckTableId == de2.Id,
                JoinType.Left,de1.HiddenDangersLevel == de3.ItemCode && de3.DataType == DataDictConst.HIDDEN_LEVEL
                }).WhereIF(!string.IsNullOrEmpty(qdstates), (de1, de2, de3) => de1.States == qdstates)
                .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2, de3) => de1.OrgId == orgid)
                .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2, de3) => de1.OrgId == currOrgId && de3.OrgId == currOrgId)
                .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2, de3) => de1.RiskPointName.Contains(riskpointname))
                .WhereIF(!string.IsNullOrEmpty(hiddenlevel), (de1, de2, de3) => de1.HiddenDangersLevel == hiddenlevel)
                    .WhereIF(states == "0", (de1, de2, de3) => de1.DeleteMark == 1)
                    .WhereIF(states == "1", (de1, de2, de3) => de1.DeleteMark == 1 && de1.TBUserId == userid)
                    .WhereIF(states == "2", (de1, de2, de3) => de1.DeleteMark == 1 && de1.ZRR == userid && de1.PushMark == 0)
                    .WhereIF(states == "3", (de1, de2, de3) => de1.DeleteMark == 1 && de1.PushUserId == userid && de1.PushMark == 1)
                    .OrderBy((de1, de2, de3) => de1.CreateTime, OrderByType.Desc)
                    .Select((de1, de2, de3) => new HiddenDangersReportEntity
                    {
                        Id = de1.Id,
                        Position = de1.Position,
                        CheckTableId = de1.CheckTableId,
                        RiskPointBH = de1.RiskPointBH,
                        RiskPointLevel = de1.RiskPointLevel,
                        RiskPointName = de1.RiskPointName,
                        HiddenDangersDescribe = de1.HiddenDangersDescribe,
                        HiddenDangersLevel = de3.ItemName,
                        HiddenDangersType = de1.HiddenDangersType,
                        ZRDW = de1.ZRDW,
                        ZRDWName = de1.ZRDWName,
                        ZRR = de1.ZRR,
                        ZRRName = de1.ZRRName,
                        TBUserId = de1.TBUserId,
                        TBUserName = de1.TBUserName,
                        CreateTime = de1.CreateTime,
                        ImageUrl = de1.ImageUrl,
                        PushMark = de1.PushMark,
                        PushUserId = de1.PushUserId,
                        States = de1.States,
                        OrgId = de1.OrgId,
                        OrgName = de1.OrgName,
                        IsSupervisor = de1.IsSupervisor,
                        TroubleshootingItems = de2.TroubleshootingItems
                    }).ToPageList(page, limit, ref totalCount);
            }
            return DataResult;
        }
        public List<HiddenDangersNoticeEntity> GetNoticeList(string states,string userid,string orgid,string currOrgId,string zlrname,string hiddenlevel, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersNoticeEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string depname = db.Queryable<UserEntity>().Where(it => it.UserId == userid).First().DepName;
                string roleid = "";
                string rolename = "";

                if(db.Queryable<UserRoleEntity>().Any(it => it.UserId == userid))
                {
                    roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == userid).First().RoleId;
                    if(db.Queryable<RoleEntity>().Any(it => it.DeleteMark == 1 && it.RoleId == roleid))
                    {
                        rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                    }
                }
                if(rolename == "系统管理员")
                {
                    DataResult = db.Queryable<HiddenDangersNoticeEntity, HiddenDangersReportEntity,DataDictEntity>((de1, de2,de3) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id,
                            JoinType.Left,de1.HiddenDangerLevel == de3.ItemCode && de3.DataType == DataDictConst.HIDDEN_LEVEL
                            }).Where((de1, de2, de3) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2, de3) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2, de3) => de2.OrgId == currOrgId && de3.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(hiddenlevel), (de1, de2, de3) => de1.HiddenDangerLevel == hiddenlevel)
                            .WhereIF(!string.IsNullOrEmpty(zlrname), (de1, de2, de3) => de1.PersonName.Contains(zlrname))
                                .OrderBy((de1, de2, de3) => de1.CreateTime, OrderByType.Desc)
                                .Select((de1, de2, de3) => new HiddenDangersNoticeEntity
                                {
                                    Id = de1.Id,
                                    HiddenDangerLevel = de3.ItemName,
                                    ReportId = de1.ReportId,
                                    IsSupervisor = de1.IsSupervisor,
                                    Method = de1.Method,
                                    PersonId = de1.PersonId,
                                    PersonName = de1.PersonName,
                                    ZRRId = de1.ZRRId,
                                    ZRRName = de1.ZRRName,
                                    ZRDWName = de2.ZRDWName,
                                    OrgId = de2.OrgId,
                                    OrgName = de2.OrgName,
                                    RiskBH = de2.RiskPointBH,
                                    RiskLevel = de2.RiskPointLevel,
                                    RiskName = de2.RiskPointName,
                                    HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                    CreateTime = de1.CreateTime,
                                    States = de1.States
                                }).ToPageList(page, limit, ref totalCount);
                }
                //else if (depname == DataDictConst.SAFE_DEPARTMENT || rolename == "部门(车间)负责人" || rolename == "公司经理级管理人员")
                else
                {                 
                    if(states == "1")
                    {
                        
                        //var reportId = db.Queryable<HiddenDangersNoticeEntity>().Where(it => it.DeleteMark == 1 && it.PersonId == userid).First().ReportId;
                        //if (db.Queryable<HiddenDangersRecheckEntity>().Any(it => it.ReportId == reportId))    
                            DataResult = db.Queryable<HiddenDangersNoticeEntity, HiddenDangersReportEntity, HiddenDangersRecheckEntity, DataDictEntity>((de1, de2, de3, de4) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id,
                            JoinType.Left,de1.ReportId == de3.ReportId,
                             JoinType.Left,de1.HiddenDangerLevel == de4.ItemCode && de4.DataType == DataDictConst.HIDDEN_LEVEL
                            }).Where((de1, de2, de3, de4) => de1.DeleteMark == 1)
                                 .Where((de1, de2, de3, de4) => de1.PersonId == userid)
                                 .WhereIF(!string.IsNullOrEmpty(hiddenlevel), (de1, de2, de3,de4) => de1.HiddenDangerLevel == hiddenlevel)
                                 .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2, de3, de4) => de2.OrgId == orgid)
                                 .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2, de3, de4) => de2.OrgId == currOrgId && de4.OrgId == currOrgId)
                                .WhereIF(!string.IsNullOrEmpty(zlrname), (de1, de2, de3, de4) => de1.PersonName.Contains(zlrname))
                                .OrderBy((de1, de2, de3, de4) => de1.CreateTime, OrderByType.Desc)
                                .Select((de1, de2, de3, de4) => new HiddenDangersNoticeEntity
                                {
                                    Id = de1.Id,
                                    HiddenDangerLevel = de4.ItemName,
                                    ReportId = de1.ReportId,
                                    IsSupervisor = de1.IsSupervisor,
                                    Method = de1.Method,
                                    PersonId = de1.PersonId,
                                    PersonName = de1.PersonName,
                                    ZRRId = de1.ZRRId,
                                    ZRRName = de1.ZRRName,
                                    ZRDWName = de2.ZRDWName,
                                    RiskBH = de2.RiskPointBH,
                                    OrgId = de2.OrgId,
                                    OrgName = de2.OrgName,
                                    RiskLevel = de2.RiskPointLevel,
                                    RiskName = de2.RiskPointName,
                                    HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                    States = de1.States,
                                    ReProposal = de3.ReProposal,
                                    Result = de3.Result,
                                    CreateTime = de1.CreateTime,
                                    ReTime = de3.ReTime
                                }).ToPageList(page, limit, ref totalCount);
                        
                        
                    }
                    else if (states == "2")
                    {
                        DataResult = db.Queryable<HiddenDangersNoticeEntity, HiddenDangersReportEntity, DataDictEntity>((de1, de2,de3) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id,
                            JoinType.Left,de1.HiddenDangerLevel == de3.ItemCode && de3.DataType == DataDictConst.HIDDEN_LEVEL
                            }).Where((de1, de2,de3) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                                .Where((de1, de2, de3) => de1.ZRRId == userid)
                                .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2, de3) => de2.OrgId == orgid)
                                .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2, de3) => de2.OrgId == currOrgId && de3.OrgId == currOrgId)
                                .WhereIF(!string.IsNullOrEmpty(hiddenlevel), (de1, de2, de3) => de1.HiddenDangerLevel == hiddenlevel)
                                .WhereIF(!string.IsNullOrEmpty(zlrname), (de1, de2, de3) => de1.PersonName.Contains(zlrname))
                                .OrderBy((de1, de2, de3) => de1.CreateTime, OrderByType.Desc)
                                .Select((de1, de2, de3) => new HiddenDangersNoticeEntity
                                {
                                    Id = de1.Id,
                                    HiddenDangerLevel = de3.ItemName,
                                    ReportId = de1.ReportId,
                                    IsSupervisor = de1.IsSupervisor,
                                    Method = de1.Method,
                                    PersonId = de1.PersonId,
                                    PersonName = de1.PersonName,
                                    ZRRId = de1.ZRRId,
                                    ZRRName = de1.ZRRName,
                                    ZRDWName = de2.ZRDWName,
                                    RiskBH = de2.RiskPointBH,
                                    OrgId = de2.OrgId,
                                    OrgName = de2.OrgName,
                                    RiskLevel = de2.RiskPointLevel,
                                    CreateTime = de1.CreateTime,
                                    RiskName = de2.RiskPointName,
                                    HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                    States = de1.States
                                }).ToPageList(page, limit, ref totalCount);
                    }
                    

                }
            }
            return DataResult;
        }
        public List<HiddenDangersModifyEntity> GetModifyList(string states,string userid, string orgid,string currOrgId, string qdstates, string riskpointname, string zlrname, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersModifyEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string depname = db.Queryable<UserEntity>().Where(it => it.UserId == userid).First().DepName;
                string roleid = "";
                string rolename = "";

                if (db.Queryable<UserRoleEntity>().Any(it => it.UserId == userid))
                {
                    roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == userid).First().RoleId;
                    if (db.Queryable<RoleEntity>().Any(it => it.DeleteMark == 1 && it.RoleId == roleid))
                    {
                        rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                    }
                }
                if (rolename == "系统管理员")
                {
                    DataResult = db.Queryable<HiddenDangersModifyEntity, HiddenDangersReportEntity>((de1, de2) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id
                            }).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(qdstates), (de1, de2) => de1.States == qdstates)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2) => de2.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2) => de2.RiskPointName.Contains(riskpointname))
                            .WhereIF(!string.IsNullOrEmpty(zlrname), (de1, de2) => de1.ModifyUserName.Contains(zlrname))
                               .OrderBy((de1, de2) => de1.CreateTime, OrderByType.Desc)
                               .Select((de1, de2) => new HiddenDangersModifyEntity
                               {
                                   Id = de1.Id,
                                   CreateTime = de1.CreateTime,
                                   ImageUrl = de1.ImageUrl,
                                   IsSupervisor = de1.IsSupervisor,
                                   ModifyResult = de1.ModifyResult,
                                   ModifySituation = de1.ModifySituation,
                                   ModifyStates = de1.ModifyStates,
                                   ModifyUserId = de1.ModifyUserId,
                                   ModifyUserName = de1.ModifyUserName,
                                   NoticeId = de1.NoticeId,
                                   ReportId = de1.ReportId,
                                   ZRRId = de1.ZRRId,
                                   ZRRName = de1.ZRRName,
                                   RiskBH = de2.RiskPointBH,
                                   RiskLevel = de2.RiskPointLevel,
                                   RiskName = de2.RiskPointName,
                                   HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   States = de1.States
                               }).ToPageList(page, limit, ref totalCount);
                }
                else
                {
                    DataResult = db.Queryable<HiddenDangersModifyEntity, HiddenDangersReportEntity>((de1, de2) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id
                            }).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(qdstates), (de1, de2) => de1.States == qdstates)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2) => de2.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2) => de2.RiskPointName.Contains(riskpointname))
                            .WhereIF(!string.IsNullOrEmpty(zlrname), (de1, de2) => de1.ModifyUserName.Contains(zlrname))
                            .WhereIF(states == "1", (de1, de2) => de1.ModifyUserId == userid)
                            .WhereIF(states == "2", (de1, de2) => de1.ZRRId == userid && de1.States == "2")
                               .OrderBy((de1, de2) => de1.CreateTime, OrderByType.Desc)
                               .Select((de1, de2) => new HiddenDangersModifyEntity
                               {
                                   Id = de1.Id,
                                   CreateTime = de1.CreateTime,
                                   ImageUrl = de1.ImageUrl,
                                   IsSupervisor = de1.IsSupervisor,
                                   ModifyResult = de1.ModifyResult,
                                   ModifySituation = de1.ModifySituation,
                                   ModifyStates = de1.ModifyStates,
                                   ModifyUserId = de1.ModifyUserId,
                                   ModifyUserName = de1.ModifyUserName,
                                   NoticeId = de1.NoticeId,
                                   ReportId = de1.ReportId,
                                   ZRRId = de1.ZRRId,
                                   ZRRName = de1.ZRRName,
                                   RiskBH = de2.RiskPointBH,
                                   RiskLevel = de2.RiskPointLevel,
                                   RiskName = de2.RiskPointName,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                   States = de1.States
                               }).ToPageList(page, limit, ref totalCount);
                }
            }
            return DataResult;
        }
        public List<HiddenDangersRecheckEntity> GetRecheckList(string userid,string orgid,string currOrgId,string states,string riskpointname,string fcrname, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersRecheckEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string depname = db.Queryable<UserEntity>().Where(it => it.UserId == userid).First().DepName;
                string roleid = "";
                string rolename = "";

                if (db.Queryable<UserRoleEntity>().Any(it => it.UserId == userid))
                {
                    roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == userid).First().RoleId;
                    if (db.Queryable<RoleEntity>().Any(it => it.DeleteMark == 1 && it.RoleId == roleid))
                    {
                        rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                    }
                }
                if (rolename == "系统管理员")
                {
                    DataResult = db.Queryable<HiddenDangersRecheckEntity, HiddenDangersReportEntity>((de1, de2) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id
                            }).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(states), (de1, de2) => de1.States == states)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2) => de2.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2) => de2.RiskPointName.Contains(riskpointname))
                            .WhereIF(!string.IsNullOrEmpty(fcrname), (de1, de2) => de1.ReUserName.Contains(fcrname))
                            .OrderBy((de1, de2) => de1.ReTime, OrderByType.Desc)
                               .Select((de1, de2) => new HiddenDangersRecheckEntity
                               {
                                   Id = de1.Id,
                                   IsSupervisor = de1.IsSupervisor,
                                   ReTime = de1.ReTime,
                                   ReImageUrl = de1.ReImageUrl,
                                   Remark = de1.Remark,
                                   ReportId = de1.ReportId,
                                   ReportUserId = de1.ReportUserId,
                                   ReportUserName = de1.ReportUserName,
                                   ReProposal = de1.ReProposal,
                                   Result = de1.Result,
                                   ReUserId = de1.ReUserId,
                                   ReUserName = de1.ReUserName,
                                   States = de1.States,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   RiskPointName = de2.RiskPointName 
                               }).ToPageList(page, limit, ref totalCount);
                }
                else if (depname == DataDictConst.SAFE_DEPARTMENT || rolename == "部门(车间)负责人" || rolename == "公司经理级管理人员")
                {
                    DataResult = db.Queryable<HiddenDangersRecheckEntity, HiddenDangersReportEntity>((de1, de2) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id
                            }).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(userid), (de1, de2) => de1.ReUserId == userid)
                            .WhereIF(!string.IsNullOrEmpty(states), (de1, de2) => de1.States == states)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2) => de2.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2) => de2.RiskPointName.Contains(riskpointname))
                            .WhereIF(!string.IsNullOrEmpty(fcrname), (de1, de2) => de1.ReUserName.Contains(fcrname))
                            .OrderBy((de1, de2) => de1.ReTime, OrderByType.Desc)
                               .Select((de1, de2) => new HiddenDangersRecheckEntity
                               {
                                   Id = de1.Id,
                                   IsSupervisor = de1.IsSupervisor,
                                   ReTime = de1.ReTime,
                                   ReImageUrl = de1.ReImageUrl,
                                   Remark = de1.Remark,
                                   ReportId = de1.ReportId,
                                   ReportUserId = de1.ReportUserId,
                                   ReportUserName = de1.ReportUserName,
                                   ReProposal = de1.ReProposal,
                                   Result = de1.Result,
                                   ReUserId = de1.ReUserId,
                                   ReUserName = de1.ReUserName,
                                   States = de1.States,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   RiskPointName = de2.RiskPointName
                               }).ToPageList(page, limit, ref totalCount);
                }
                else
                {
                    DataResult = db.Queryable<HiddenDangersRecheckEntity, HiddenDangersReportEntity>((de1, de2) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id
                            }).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1)
                            .WhereIF(!string.IsNullOrEmpty(userid), (de1, de2) => de1.ReportUserId == userid)
                            .WhereIF(!string.IsNullOrEmpty(states), (de1, de2) => de1.States == states)
                            .WhereIF(!string.IsNullOrEmpty(orgid), (de1, de2) => de2.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), (de1, de2) => de2.OrgId == currOrgId)
                            .WhereIF(!string.IsNullOrEmpty(riskpointname), (de1, de2) => de2.RiskPointName.Contains(riskpointname))
                            .WhereIF(!string.IsNullOrEmpty(fcrname), (de1, de2) => de1.ReUserName.Contains(fcrname))
                            .OrderBy((de1, de2) => de1.ReTime, OrderByType.Desc)
                               .Select((de1, de2) => new HiddenDangersRecheckEntity
                               {
                                   Id = de1.Id,
                                   IsSupervisor = de1.IsSupervisor,
                                   ReTime = de1.ReTime,
                                   ReImageUrl = de1.ReImageUrl,
                                   Remark = de1.Remark,
                                   ReportId = de1.ReportId,
                                   ReportUserId = de1.ReportUserId,
                                   ReportUserName = de1.ReportUserName,
                                   ReProposal = de1.ReProposal,
                                   Result = de1.Result,
                                   ReUserId = de1.ReUserId,
                                   ReUserName = de1.ReUserName,
                                   States = de1.States,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   RiskPointName = de2.RiskPointName
                               }).ToPageList(page, limit, ref totalCount);
                }
            }
            return DataResult;
        }

        public List<HiddenDangersNoticeEntity> GetHistoryList1(string userid, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersNoticeEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<HiddenDangersNoticeEntity, HiddenDangersReportEntity, HiddenDangersRecheckEntity, DataDictEntity,CheckTableEntity, DataDictEntity>((de1, de2, de3, de4,de5,de6) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id,
                            JoinType.Left,de1.ReportId == de3.ReportId,
                             JoinType.Left,de1.HiddenDangerLevel == de4.ItemCode && de4.DataType == DataDictConst.HIDDEN_LEVEL,
                             JoinType.Left,de2.CheckTableId == de5.Id,
                             JoinType.Left,de2.HiddenDangersLevel == de6.ItemCode && de6.DataType == DataDictConst.HIDDEN_LEVEL,
                            }).Where((de1, de2, de3, de4, de5, de6) => de1.ZRRId == userid)                              
                               .OrderBy((de1, de2, de3, de4, de5, de6) => de1.CreateTime, OrderByType.Desc)
                               .Select((de1, de2, de3, de4, de5, de6) => new HiddenDangersNoticeEntity
                               {
                                   HiddenDangerLevel = de4.ItemName,
                                   IsSupervisor = de1.IsSupervisor,
                                   Method = de1.Method,
                                   PersonId = de1.PersonId,
                                   PersonName = de1.PersonName,
                                   RiskBH = de2.RiskPointBH,
                                   ReportHiddenDangerLevel = de6.ItemName,
                                   OrgId = de2.OrgId,
                                   OrgName = de2.OrgName,
                                   ReportImageUrl = de2.ImageUrl,
                                   RiskLevel = de2.RiskPointLevel,
                                   RiskName = de2.RiskPointName,
                                   ReportTime = de2.CreateTime,
                                   HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                   States = de1.States,
                                   ReProposal = de3.ReProposal,
                                   Result = de3.Result,
                                   CreateTime = de1.CreateTime,
                                   ReTime = de3.ReTime, 
                                   ControlMeasures = de5.ControlMeasures,
                                   TroubleshootingItems = de5.TroubleshootingItems,
                                   RiskFactor = de5.RiskFactor
                               }).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        public List<HiddenDangersRecheckEntity> GetHistoryList2(string userid, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<HiddenDangersRecheckEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<HiddenDangersRecheckEntity, HiddenDangersReportEntity,DataDictEntity, DataDictEntity, CheckTableEntity>((de1, de2,de3, de4,de5) => new object[] {
                            JoinType.Left,de1.ReportId == de2.Id,
                            JoinType.Left,de2.RiskPointLevel == de3.ItemCode && de3.DataType == DataDictConst.RISK_LEVEL,
                             JoinType.Left,de2.HiddenDangersLevel == de4.ItemCode && de4.DataType == DataDictConst.HIDDEN_LEVEL,
                             JoinType.Left,de2.CheckTableId == de5.Id,
                            }).Where((de1, de2, de3, de4, de5) => de1.ReUserId == userid)
                               .OrderBy((de1, de2, de3, de4, de5) => de1.ReTime, OrderByType.Desc)
                               .Select((de1, de2, de3, de4, de5) => new HiddenDangersRecheckEntity
                               {
                                   HiddenDangerLevel = de4.ItemName,
                                   IsSupervisor = de1.IsSupervisor,
                                   OrgName = de2.OrgName,
                                   States = de1.States,
                                   RiskPointName = de2.RiskPointName,                                  
                                   ReProposal = de1.ReProposal,
                                   HiddenDangersDescribe = de2.HiddenDangersDescribe,
                                   RiskPointLevel = de3.ItemName,
                                   ReportUserId = de2.TBUserId,
                                   Remark = de1.Remark,
                                   ReportUserName = de2.TBUserName,
                                   Result = de1.Result,
                                   ReportTime = de2.CreateTime,
                                   ReTime = de1.ReTime,
                                   ControlMeasures = de5.ControlMeasures,
                                   TroubleshootingItems = de5.TroubleshootingItems,
                                   RiskFactor = de5.RiskFactor,
                                   ReUserName = de1.ReUserName,
                                   ReImageUrl = de1.ReImageUrl,
                                   ReportImageUrl = de2.ImageUrl
                               }).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        public List<HiddenDangersModifyEntity> GetHistoryList3(string userid, int pageIndex, int pageSize, ref int totalCount)
        {
            //var DataResult = new List<HiddenDangersModifyEntity>();
            //using (var db = _dbContext.GetIntance())
            //{
            //    DataResult = db.Queryable<HiddenDangersModifyEntity, HiddenDangersReportEntity, DataDictEntity, DataDictEntity, CheckTableEntity>((de1, de2, de3, de4, de5) => new object[] {
            //                JoinType.Left,de1.ReportId == de2.Id,
            //                JoinType.Left,de2.RiskPointLevel == de3.ItemCode && de3.DataType == DataDictConst.RISK_LEVEL,
            //                 JoinType.Left,de2.HiddenDangersLevel == de4.ItemCode && de4.DataType == DataDictConst.HIDDEN_LEVEL,
            //                 JoinType.Left,de2.CheckTableId == de5.Id,
            //                }).Where((de1, de2, de3, de4, de5) => de1.ModifyUserId == userid)
            //                   .OrderBy((de1, de2, de3, de4, de5) => de1.CreateTime, OrderByType.Desc)
            //                   .Select((de1, de2, de3, de4,de5) => new HiddenDangersModifyEntity
            //                   {
            //                       ModifyStates = de1.ModifyStates,
            //                       ModifySituation = de1.ModifySituation,
            //                       ModifyResult = de1.ModifyResult,
            //                       ModifyUserId = de1.ModifyUserId,
            //                       ModifyUserName =de1.ModifyUserName,
            //                       IsSupervisor = de1.IsSupervisor,
            //                       OrgName = de2.OrgName,
            //                       States = de1.States,
            //                       RiskPointLevel = de3.ItemName,
            //                       HiddenDangerLevel = de4.ItemName,
            //                       CreateTime = de1.CreateTime,
            //                       ControlMeasures = de5.ControlMeasures,
            //                       HiddenDangersDescribe = de2.HiddenDangersDescribe,
            //                       TroubleshootingItems = de5.TroubleshootingItems,
            //                       ReportImageUrl = de2.ImageUrl,
            //                       ReportTime = de2.CreateTime,
            //                       RiskFactor = de5.RiskFactor
            //                   }).ToPageList(pageIndex, pageSize, ref totalCount);
            //}
            //return DataResult;
            var DataResult = new List<HiddenDangersModifyEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<HiddenDangersReportEntity, CheckTableEntity, DataDictEntity>((de1, de2, de3) => new object[] {
                JoinType.Left,de1.CheckTableId == de2.Id,
                JoinType.Left,de1.HiddenDangersLevel == de3.ItemCode && de3.DataType == DataDictConst.HIDDEN_LEVEL
                }).Where((de1, de2, de3) => de1.TBUserId == userid && de1.DeleteMark == 1)
                    .OrderBy((de1, de2, de3) => de1.CreateTime, OrderByType.Desc)
                    .Select((de1, de2, de3) => new HiddenDangersModifyEntity
                    {
                        Id = de1.Id,
                        ControlMeasures = de2.ControlMeasures,
                        ModifyUserName = de1.TBUserName,
                        IsSupervisor = de1.IsSupervisor,
                        States = de1.States,
                        RiskPointLevel = de1.RiskPointLevel,
                        HiddenDangerLevel = de3.ItemName,
                        CreateTime = de1.CreateTime,
                        HiddenDangersDescribe = de1.HiddenDangersDescribe,
                        ReportImageUrl = de1.ImageUrl,
                        TroubleshootingItems = de2.TroubleshootingItems,
                        RiskFactor = de2.RiskFactor
                    }).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 上报新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool CreateReport(HiddenDangersReportEntity entity,string OrgId,string userid,string username)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var pointEntity = db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.RiskPointBH == entity.RiskPointBH && it.OrgId == OrgId).First();
                    if (pointEntity != null)
                    {
                        entity.ZRDW = pointEntity.ZRDW;
                        entity.ZRR = pointEntity.ZRR;
                        entity.ZRDWName = pointEntity.ZRDWName;
                        entity.ZRRName = pointEntity.ZRRName;
                    }
                    db.Insertable(entity).ExecuteCommand();
                    var palanEntity = db.Queryable<CheckPlanEnity>().Where(it => it.UserId == userid && it.RiskBH.Contains(entity.RiskPointBH)).First();
                    CheckResultRecordEntity result = new CheckResultRecordEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CheckPlanName = palanEntity.PlanName,
                        CheckPlanId = palanEntity.Id,
                        CheckTableId = entity.CheckTableId,
                        RiskPointBH = entity.RiskPointBH,
                        RiskPointName = entity.RiskPointName,
                        CreateTime = DateTime.Now,
                        UserId = userid,
                        UserName = username,
                        States = 1,
                        OrgId = palanEntity.OrgId,
                        OrgName = palanEntity.OrgName
                    };
                    db.Insertable(result).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }


            }
            return true;
        }
        /// <summary>
        /// 下发通知新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool CreateNotice(HiddenDangersNoticeEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var report = db.Queryable<HiddenDangersReportEntity>().Where(it => it.Id == entity.ReportId).First();
                    if (report.PushMark == 1)
                    {
                        entity.ZRRId = report.PushUserId;
                        entity.ZRRName = report.PushZRR;
                    }
                    db.Insertable(entity).ExecuteCommand();
                    HiddenDangersReportEntity entity2 = new HiddenDangersReportEntity()
                    {
                        States = "5"
                    };
                    db.Updateable(entity2).UpdateColumns(it => new { it.States })
                            .Where(it => it.Id == entity.ReportId).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 整改新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool CreateModify(HiddenDangersModifyEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Insertable(entity).ExecuteCommand();

                    HiddenDangersReportEntity entity2 = new HiddenDangersReportEntity()
                    {
                        States = "2"
                    };
                    HiddenDangersNoticeEntity entity3 = new HiddenDangersNoticeEntity()

                    {
                        States = "2"
                    };
                    db.Updateable(entity2).UpdateColumns(it => new { it.States })
                           .Where(it => it.Id == entity.ReportId).ExecuteCommand();
                    db.Updateable(entity3).UpdateColumns(it => new { it.States })
                           .Where(it => it.ReportId == entity.ReportId).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
                return true;
            }
        }
        public bool UpdateModify(HiddenDangersModifyEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(entity)
                .UpdateColumns(it => new { it.States, it.ImageUrl})
                .Where(it => it.ReportId == entity.ReportId)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        public bool IsSecondCreat(string noticeId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                reslut = db.Queryable<HiddenDangersModifyEntity>().Any(it => it.NoticeId == noticeId && it.States == "2" && it.DeleteMark == 1);
            }
            return reslut;
        }
        public bool IsModifyPass(string noticeId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                reslut = db.Queryable<HiddenDangersModifyEntity>().Any(it => it.NoticeId == noticeId && it.States == "4" && it.DeleteMark == 1);
            }
            return reslut;
        }
        public bool IsSecondReport(string checkTableId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                reslut = db.Queryable<HiddenDangersReportEntity>().Any(it => it.CheckTableId == checkTableId && it.DeleteMark ==1);
            }
            return reslut;
        }
        public bool IsSecondRecheck(string reportId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                reslut = db.Queryable<HiddenDangersRecheckEntity>().Any(it => it.ReportId == reportId && it.DeleteMark == 1 && it.States == "3");
            }
            return reslut;
        }
        public string GetIsSupervisor(string reportId)
        {
            string result = string.Empty;
            using (var db = _dbContext.GetIntance())
            {
                result = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.Id == reportId).First().IsSupervisor;
            }
            return result;
        }
        /// <summary>
        /// 复查新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool CreateRecheck(HiddenDangersRecheckEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Insertable(entity).ExecuteCommand();
                    HiddenDangersModifyEntity entity1 = new HiddenDangersModifyEntity()
                    {
                        States = entity.States
                    };
                    HiddenDangersReportEntity entity2 = new HiddenDangersReportEntity()
                    {
                        States = entity.States
                    };
                    db.Updateable(entity1).UpdateColumns(it => new { it.States })
                            .Where(it => it.ReportId == entity.ReportId).ExecuteCommand();
                    db.Updateable(entity2).UpdateColumns(it => new { it.States })
                            .Where(it => it.Id == entity.ReportId).ExecuteCommand();
                    if(entity.States == "3")
                    {
                        HiddenDangersNoticeEntity entity3 = new HiddenDangersNoticeEntity()
                        {
                            DeleteMark = 0
                        };
                        db.Updateable(entity3).UpdateColumns(it => new { it.DeleteMark })
                            .Where(it => it.ReportId == entity.ReportId).ExecuteCommand();
                    }

                    if (entity.States == "4")
                    {
                        HiddenDangersNoticeEntity entity3 = new HiddenDangersNoticeEntity()
                        {
                            States = "4"
                        };
                        db.Updateable(entity3).UpdateColumns(it => new { it.States })
                            .Where(it => it.ReportId == entity.ReportId).ExecuteCommand();
                    }
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
                return true;
            }
        }
        public bool PushSafeDepart(string id,string zrrid)
        {
            using (var db = _dbContext.GetIntance())
            {
                HiddenDangersReportEntity entity = new HiddenDangersReportEntity()
                {
                    PushMark = 1,
                    PushUserId = zrrid,
                    PushZRR = db.Queryable<UserEntity>().Where(it => it.UserId == zrrid).First().RealName
                };
                var count = db.Updateable(entity).UpdateColumns(it => new { it.PushMark,it.PushUserId })
                            .Where(it => it.Id == id).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Update(HiddenDangersReportEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(entity)
                .IgnoreColumns(it => new { it.DeleteMark, it.CreateTime,it.PushMark })
                .Where(it => it.Id == entity.Id)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键List</param>
        public bool DeleteBatch(List<string> keyValues,string type)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    if(type == "1")
                    {
                        var entity = new HiddenDangersReportEntity();
                        entity.DeleteMark = 0;
                        //逻辑删除
                        var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                            .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                        result = counts > 0 ? result = true : false;
                    }
                    if (type == "2")
                    {
                        var entity = new HiddenDangersNoticeEntity();
                        entity.DeleteMark = 0;
                        //逻辑删除
                        var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                            .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                        result = counts > 0 ? result = true : false;
                    }
                    if (type == "3")
                    {
                        var entity = new HiddenDangersModifyEntity();
                        entity.DeleteMark = 0;
                        //逻辑删除
                        var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                            .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                        result = counts > 0 ? result = true : false;
                    }
                    if (type == "4")
                    {
                        var entity = new HiddenDangersRecheckEntity();
                        entity.DeleteMark = 0;
                        //逻辑删除
                        var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                            .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                        result = counts > 0 ? result = true : false;
                    }


                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion

        public List<SafeUserDto> GetSafeUserList(string currOrgId)
        {
            var dataResult = new List<SafeUserDto>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<UserEntity>().Where(it => it.DeleteMark == 1 && it.DepName == DataDictConst.SAFE_DEPARTMENT && it.OrganizeId == currOrgId)
                            .Select(it => new SafeUserDto
                            {
                                 UserId = it.UserId,
                                 UserName = it.RealName
                            }).ToList();
                return dataResult;
            }

        }
    }
}
