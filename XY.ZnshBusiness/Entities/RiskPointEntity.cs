using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    [SugarTable("riskpoint")]
    public class RiskPointEntity
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
        /// 风险单元编号
        /// </summary>
        public string RiskUnitBH
        {
            get; set;
        }
        /// <summary>
        /// 风险点编号
        /// </summary>
        public string RiskPointBH
        {
            get; set;
        }
        /// <summary>
        /// 编号
        /// </summary>
        public string BH
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
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
        /// 生成二维码路径
        /// </summary>
        public string QRCodeUrl
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
        public string ZRR
        {
            get; set;
        }
        public string ZRDW
        {
            get; set;
        }
        public string ZRRName
        {
            get; set;
        }
        public string ZRDWName
        {
            get; set;
        }

    }
}
