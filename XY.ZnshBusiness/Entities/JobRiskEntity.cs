using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 岗位风险管理
    /// </summary>
    [SugarTable("jobrisk")]
    public class JobRiskEntity
    {
        /// <summary>
		/// 唯一标识码
		/// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleId
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserId
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string JobId
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string JobName
        {
            get; set;
        }   
        /// <summary>
        /// 
        /// </summary>
        public string RiskPointBH
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RiskPointName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgId
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DepId
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DepName
        {
            get; set;
        }
        /// <summary>
        /// 序号
        /// </summary>
        public string SortCode
        {
            get; set;
        }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int? DeleteMark
        {
            get; set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get; set;
        }
    }
}
