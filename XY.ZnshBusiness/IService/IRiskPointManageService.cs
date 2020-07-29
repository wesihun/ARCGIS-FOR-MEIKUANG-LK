using System.Collections.Generic;
using XY.ZnshBusiness.Entities;

namespace XY.ZnshBusiness.IService
{
    public interface IRiskPointManageService
    {
        #region 获取数据
        /// <summary>
        /// 根据风险点列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<RiskPointEntity> GetPageListByCondition(string orgid,string currOrgId,string riskpointbh, string riskpointname,string riskunitbh,int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据主键获取风险编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetQRCodeUrl(string id);
        #endregion

        #region 验证是否重复添加
        bool IsExist(string orgid, string bh, string id);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="riskpointEntity">实体</param>
        /// <returns></returns>
        bool Insert(RiskPointEntity riskpointEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="riskpointEntity">实体</param>
        /// <returns></returns>
        bool Update(RiskPointEntity riskpointEntity);
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
