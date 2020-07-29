using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class RiskUnitManageService: IRiskUnitManageService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public RiskUnitManageService(IXYDbContext dbContext)
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
        public List<RiskUnitEntity> GetPageListByCondition(string orgid, string currOrgId, string unitBH, string unitName, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<RiskUnitEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RiskUnitEntity>()
                    .WhereIF(!string.IsNullOrEmpty(unitBH), it => it.UnitBH.Contains(unitBH))
                    .WhereIF(!string.IsNullOrEmpty(unitName), it => it.UnitName.Contains(unitName))
                    .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                    .Where(it => it.DeleteMark == 1)
                    .OrderBy(it => it.UnitOneCode)
                    .OrderBy(it => it.UnitTwoCode)
                    .OrderBy(it => it.UnitThreeCode)
                    .ToPageList(page, limit, ref totalCount);
                
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
                    reslut = db.Queryable<RiskUnitEntity>().Any(it => it.OrgId == orgid && it.UnitBH == bh && it.DeleteMark == 1);
                }
                else
                {
                    reslut = db.Queryable<RiskUnitEntity>().Any(it => it.OrgId == orgid && it.UnitBH == bh && it.Id != id && it.DeleteMark == 1);
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
        public bool Insert(RiskUnitEntity entity)
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
        public bool Update(RiskUnitEntity entity)
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

                    var entity = new RiskUnitEntity();
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
