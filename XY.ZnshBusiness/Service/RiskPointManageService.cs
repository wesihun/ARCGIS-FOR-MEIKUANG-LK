using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class RiskPointManageService : IRiskPointManageService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public RiskPointManageService(IXYDbContext dbContext)
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
        public List<RiskPointEntity> GetPageListByCondition(string orgid,string currOrgId, string riskpointbh, string riskpointname,string riskunitbh,int page, int limit, ref int totalCount)
        {
            var DataResult = new List<RiskPointEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RiskPointEntity>()
                    .WhereIF(!string.IsNullOrEmpty(riskpointbh), it => it.BH.Contains(riskpointbh))
                    .WhereIF(!string.IsNullOrEmpty(riskunitbh), it => it.RiskUnitBH.Contains(riskunitbh))
                    .WhereIF(!string.IsNullOrEmpty(riskpointname), it => it.Name.Contains(riskpointname))
                    .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                    .Where(re => re.DeleteMark == 1)
                    .OrderBy((re) => re.SortCode).ToPageList(page, limit, ref totalCount);                   
                
            }
            return DataResult;
        }
        public string GetQRCodeUrl(string id)
        {
            string bh = string.Empty;
            using (var db = _dbContext.GetIntance())
            {
                bh = db.Queryable<RiskPointEntity>()
                    .Where(it => it.DeleteMark == 1 && it.Id == id)
                    .First().QRCodeUrl;
            }
            return bh;
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 判断风险点编码是否重复添加
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="bh">风险点编号</param>
        /// <returns></returns>
        public bool IsExist(string orgid, string bh,string id)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(id))
                {
                    reslut = db.Queryable<RiskPointEntity>().Any(it => it.OrgId == orgid && it.RiskPointBH == bh && it.DeleteMark == 1);
                }
                else
                {
                    reslut = db.Queryable<RiskPointEntity>().Any(it => it.OrgId == orgid && it.RiskPointBH == bh && it.Id != id && it.DeleteMark == 1);
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
        public bool Insert(RiskPointEntity entity)
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
        public bool Update(RiskPointEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();               
                    var updatalist = db.Queryable<RiskPointEntity>().Where(it => it.Id == entity.Id).First();
                    if (db.Queryable<RiskClassIficationEntity>().Any(it => it.RiskPointBH == updatalist.RiskPointBH && it.OrgId == updatalist.OrgId))
                    {
                        RiskClassIficationEntity classEntity = new RiskClassIficationEntity()
                        {
                            RiskPointBH = entity.RiskPointBH,
                            RiskPointName = entity.Name
                        };
                        db.Updateable(classEntity).UpdateColumns(it => new { it.RiskPointBH, it.RiskPointName })
                            .Where(it => it.RiskPointBH == updatalist.RiskPointBH).ExecuteCommand();
                    }
                    if (db.Queryable<JobRiskEntity>().Any(it => it.RiskPointBH == updatalist.RiskPointBH && it.OrgId == updatalist.OrgId))
                    {
                        JobRiskEntity jobriskEntity = new JobRiskEntity()
                        {
                            RiskPointBH = entity.RiskPointBH,
                            RiskPointName = entity.Name
                        };
                        db.Updateable(jobriskEntity).UpdateColumns(it => new { it.RiskPointBH, it.RiskPointName })
                            .Where(it => it.RiskPointBH == updatalist.RiskPointBH).ExecuteCommand();
                    }
                    /* if (db.Queryable<CheckTableEntity>().Any(it => it.RiskPointBH == updatalist.RiskPointBH))
                     {
                         CheckTableEntity checkTableEntity = new CheckTableEntity()
                         {
                             RiskPointBH = entity.RiskPointBH,
                             RiskPointName = entity.Name
                         };
                         db.Updateable(checkTableEntity).UpdateColumns(it => new { it.RiskPointBH, it.RiskPointName })
                        .Where(it => it.RiskPointBH == updatalist.RiskPointBH).ExecuteCommand();
                     }*/
                    db.Updateable(entity).IgnoreColumns(it => new { it.DeleteMark, it.CreateTime }).Where(it => it.Id == entity.Id).ExecuteCommand();
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
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool Delete(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                using (var db = _dbContext.GetIntance())
                {

                    var entity = new RiskPointEntity();
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
                    var entity = new RiskPointEntity();
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
