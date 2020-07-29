using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class RiskClassIficationService: IRiskClassIficationService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public RiskClassIficationService(IXYDbContext dbContext)
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
        public List<RiskClassIficationEntity> GetPageListByCondition(string orgid,string currOrgId, string riskpointbh, string riskpointname, string risklevel,int page, int limit, ref int totalCount)
        {
            var DataResult = new List<RiskClassIficationEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RiskClassIficationEntity, DataDictEntity, DataDictEntity, DataDictEntity, DataDictEntity>((re, oe1,oe2, oe3,oe4) => new object[] {
                    JoinType.Left,re.RiskLevel == oe1.ItemCode && oe1.DataType == DataDictConst.RISK_LEVEL,
                    JoinType.Left,re.RiskFactorType == oe2.ItemCode && oe2.DataType == DataDictConst.RISK_FACTOR,
                    JoinType.Left,re.AccidentType == oe3.ItemCode && oe3.DataType == DataDictConst.ACCIDENT_TYPE,
                    JoinType.Left,re.MeasuresType == oe4.ItemCode && oe4.DataType == DataDictConst.MEASURES_TYPE
                }).Where((re, oe1, oe2, oe3, oe4) => re.DeleteMark == 1 && oe1.DeleteMark == 1 && oe2.DeleteMark == 1 && oe3.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(riskpointbh), (re, oe1, oe2, oe3, oe4) => re.RiskPointBH.Contains(riskpointbh))
                .WhereIF(!string.IsNullOrEmpty(riskpointname), (re, oe1, oe2, oe3, oe4) => re.RiskPointName.Contains(riskpointname))
                .WhereIF(!string.IsNullOrEmpty(risklevel), (re, oe1, oe2, oe3, oe4) => re.RiskLevel == risklevel)
                .WhereIF(!string.IsNullOrEmpty(orgid), (re, oe1, oe2, oe3, oe4) => re.OrgId == orgid)
                .WhereIF(!string.IsNullOrEmpty(currOrgId), (re, oe1, oe2, oe3, oe4) => re.OrgId == currOrgId && oe1.OrgId == currOrgId && oe2.OrgId == currOrgId && oe3.OrgId == currOrgId)
                .OrderBy((re, oe1, oe2, oe3, oe4) => re.CreateTime,OrderByType.Desc)
                .Select((re, oe1, oe2, oe3, oe4) => new RiskClassIficationEntity
                {
                    Id = re.Id,
                    OrgId = re.OrgId,
                    OrgName = re.OrgName,
                    TroubleshootingItems = re.TroubleshootingItems,
                    RiskLevel = oe1.ItemName,
                    RiskFactor = re.RiskFactor,
                    AccidentType = oe3.ItemName,
                    ControlMeasures = re.ControlMeasures,
                    RiskPointBH = re.RiskPointBH,
                    EmergencyMeasures = re.EmergencyMeasures,
                    MeasuresType = oe4.ItemName,
                    RiskPointName = re.RiskPointName,
                    RiskFactorType = oe2.ItemName
                }).ToPageList(page, limit, ref totalCount);
            }
            return DataResult;
        }

        public List<RiskClassIficationEntity> GetRiskPointList(string orgid,string currOrgId, string riskpointbh)
        {
            var DataResult = new List<RiskClassIficationEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RiskClassIficationEntity, CheckPlanEnity, DataDictEntity>((re, oe, dt) => new object[] {
                    JoinType.Left,re.RiskPointBH == oe.RiskBH && re.OrgId == oe.OrgId,
                    JoinType.Left,oe.ExecutionMode == dt.ItemCode && dt.DataType == DataDictConst.RISK_ExecutionMode
                }).Where((re, oe, dt) => re.DeleteMark == 1 && (oe.DeleteMark == 1 || oe.DeleteMark == null))
                .WhereIF(!string.IsNullOrEmpty(orgid), (re, oe, dt) => re.OrgId == orgid)
                .WhereIF(!string.IsNullOrEmpty(currOrgId), (re, oe, dt) => re.OrgId == currOrgId && oe.OrgId == currOrgId)
                .WhereIF(!string.IsNullOrEmpty(riskpointbh), (re, oe, dt) => re.RiskPointBH == riskpointbh)
                .Select((re, oe, dt) => new RiskClassIficationEntity
                {
                    OrgId = re.OrgId,
                    OrgName = re.OrgName,
                    TroubleshootingItems = re.TroubleshootingItems,
                    RiskLevel = re.RiskLevel,
                    RiskFactor = re.RiskFactor,
                    AccidentType = re.AccidentType,
                    ControlMeasures = re.ControlMeasures,
                    ExecutionMode = dt.ItemName,
                    RiskPointBH = re.RiskPointBH,
                    RiskPointName = re.RiskPointName,
                }).ToList();
            }
            return DataResult;
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 判断风险单元编码是否重复添加
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="bh">风险点编号</param>
        /// <returns></returns>
        public bool IsExist(string orgid, string bh, string id)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(id))
                {
                    reslut = db.Queryable<RiskClassIficationEntity>().Any(it => it.OrgId == orgid && it.RiskPointBH == bh && it.DeleteMark == 1);
                }
                else
                {
                    reslut = db.Queryable<RiskClassIficationEntity>().Any(it => it.OrgId == orgid && it.RiskPointBH == bh && it.Id != id && it.DeleteMark == 1);
                }
            }
            return reslut;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Insert(RiskClassIficationEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(entity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Update(RiskClassIficationEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(entity)
                .IgnoreColumns(it => new { it.DeleteMark, it.CreateTime })
                .Where(it => it.Id == entity.Id)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool Delete(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                using (var db = _dbContext.GetIntance())
                {

                    var entity = new RiskClassIficationEntity();
                    entity.DeleteMark = 0;
                    //逻辑删除
                    var count = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                    .Where(it => it.Id == keyValue)
                    .ExecuteCommand();
                    return count > 0 ? true : false;

                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键List</param>
        public bool DeleteBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var entity = new RiskUnitEntity();
                    entity.DeleteMark = 0;
                    //逻辑删除
                    var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                        .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}
