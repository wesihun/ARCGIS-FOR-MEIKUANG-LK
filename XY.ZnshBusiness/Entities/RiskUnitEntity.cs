using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    [SugarTable("riskunit")]
    public class RiskUnitEntity
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
        /// 机构Id
        /// </summary>
        public string OrgId
        {
            get; set;
        }
        /// <summary>
        /// 机构名
        /// </summary>
        public string OrgName
        {
            get; set;
        }
        /// <summary>
        /// 一级单元序号
        /// </summary>
        public string UnitOneCode
        {
            get; set;
        }
        /// <summary>
        /// 一级单元名
        /// </summary>
        public string UnitOneName
        {
            get; set;
        }
        /// <summary>
        /// 二级子单元序号
        /// </summary>
        public string UnitTwoCode
        {
            get; set;
        }
        /// <summary>
        /// 二级子单元名
        /// </summary>
        public string UnitTwoName
        {
            get; set;
        }
        /// <summary>
        /// 三级子单元
        /// </summary>
        public string UnitThreeCode
        {
            get; set;
        }
        /// <summary>
        /// 三级子单元名
        /// </summary>
        public string UnitThreeName
        {
            get; set;
        }
        /// <summary>
        /// 风险单元编号
        /// </summary>
        public string UnitBH
        {
            get; set;
        }
        /// <summary>
        /// 风险单元名称
        /// </summary>
        public string UnitName
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
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
