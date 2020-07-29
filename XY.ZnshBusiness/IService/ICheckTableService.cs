using System;
using System.Collections.Generic;
using System.Text;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;

namespace XY.ZnshBusiness.IService
{
    public interface ICheckTableService
    {
        #region 获取数据
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<CheckTableEntity> GetPageListByCondition(string condition, string keyword, string orgid,string currOrgId, int pageIndex, int pageSize, ref int totalCount);

        List<CheckTableEntity> GetCheckPointSelect(string orgid, string userid, int page, int limit, ref int totalCount);
        /// <summary>
        /// 获取过滤的风险点分级列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<RiskClassIficationEntity> GetClassClassIficationList(int pageIndex, int pageSize, ref int totalCount);

        List<CheckTableEntity> GetCheckItems(string userid,string riskpointbh);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classificationId"></param>
        /// <returns></returns>
        bool Insert(List<string> classificationId);
        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues);

        bool InsertBatch(List<AuthorizedDto> models);
        #endregion
    }
}
