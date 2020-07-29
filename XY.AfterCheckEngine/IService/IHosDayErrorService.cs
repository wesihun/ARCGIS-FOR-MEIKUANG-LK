using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.IService
{
    public interface IHosDayErrorService
    {
        #region 获取数据
        /// <summary>
        /// 根据条件获取住院天数异常信息列表并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="keyword">条件值</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetPageListByCondition(string condition, string keyword, string idnumber, string yljgbh, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据条件获取分解住院列表并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="keyword">条件值</param>
        /// <param name="querystr">条件字符串</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetDecomposeHos(string condition, string keyword, string querystr, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据编码获取分解住院信息
        /// </summary>
        /// <param name="idnumber">身份证号</param>
        /// <param name="institutioncode">就诊机构编码</param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetDecomposehosByCode(string idnumber,string institutioncode);
        /// <summary>
        /// 根据条件获取入出院日期异常列表并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="keyword">条件值</param>
        /// <param name="querystr">条件字符串</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetInOutDate(string condition, string keyword, string querystr, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据编码获取入出院日期异常信息
        /// </summary>
        /// <param name="idnumber">身份证号</param>
        /// <param name="institutioncode">就诊机构编码</param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetInOutDateByCode(string idnumber, string institutioncode);
        /// <summary>
        /// 获取处方明细并分页
        /// </summary>
        /// <param name="hosregistercode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<YBHosPreInfoEntity> GetCFDeatilList(string hosregistercode, int pageIndex, int pageSize, ref int totalCount);

        List<YBClinicPreInfoEntity> GetCFDeatilListCli(string hosregistercode, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取医院信息list
        /// </summary>
        /// <returns></returns>
        List<YYXXDto> GetYYXXList(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取疾病信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<DiseaseDirectoryDto> GetDiseaseList(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount);

        /// <summary>
        /// 获取住院数据，首页
        /// </summary>
        /// <returns></returns>
        List<ParameterInfoEntity> GetParameterList();
        /// <summary>
        /// 获取门诊数据，首页
        /// </summary>
        /// <returns></returns>
        List<ParameterInfoEntity> GetParameterList_Clinic();
        /// <summary>
        /// 加主页缓存
        /// </summary>
        bool AddHomeIndexParameterRedis();
        #endregion
    }
}
