using System;
using System.Collections.Generic;
using System.Text;
using XY.ZnshBusiness.Entities;

namespace XY.ZnshBusiness.IService
{
    public interface IRiskUnitManageService
    {
        #region 获取数据
        /// <summary>
        /// 根据风险点列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<RiskUnitEntity> GetPageListByCondition(string orgid,string currOrgId, string unitBH, string unitName, int pageIndex, int pageSize, ref int totalCount);
        #endregion

        #region 验证是否重复添加
        bool IsExist(string orgid, string bh, string id);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="riskunitEntity">实体</param>
        /// <returns></returns>
        bool Insert(RiskUnitEntity riskunitEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="riskunitEntity">实体</param>
        /// <returns></returns>
        bool Update(RiskUnitEntity riskunitEntity);
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool Delete(string keyValue);
        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues);
        #endregion
    }
}
