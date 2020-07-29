using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("YB_ClinicPreInfo")]
    public class YBClinicPreInfoEntity
    {

        /// <summary>
        /// 门诊登记编码
        /// </summary>		
        public string ClinicRegisterCode { get; set; }
        /// <summary>
        /// 处方编码
        /// </summary>		
        public string PreCode { get; set; }
        /// <summary>
        /// 序号
        /// </summary>		
        public int ItemIndex { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>		
        public string ItemCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>		
        public string ItemName { get; set; }
        /// <summary>
        /// 收费类别编码（费用类型编码）
        /// </summary>		
        public string CollectFeesCategoryCode { get; set; }
        /// <summary>
        /// 收费类别名称
        /// </summary>		
        public string CollectFeesCategoryName { get; set; }
        /// <summary>
        /// 收费项目等级编码
        /// </summary>		
        public string CollectFeesProjectGradeCode { get; set; }
        /// <summary>
        /// 收费项目等级名称
        /// </summary>		
        public string CollectFeesProjectGradeName { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>		
        public decimal ZFY { get; set; }
        /// <summary>
        /// 有效金额
        /// </summary>		
        public decimal YXJE { get; set; }
        /// <summary>
        /// 不可报销金额
        /// </summary>		
        public decimal BKBXJE { get; set; }
        /// <summary>
        /// 数量
        /// </summary>		
        public int COUNT { get; set; }
        /// <summary>
        /// 单价
        /// </summary>		
        public decimal PRICE { get; set; }
        /// <summary>
        /// 报销比例
        /// </summary>		
        public decimal CompRatio { get; set; }
        /// <summary>
        /// His项目编码
        /// </summary>		
        public string HisItemCode { get; set; }
        /// <summary>
        /// His项目名称
        /// </summary>		
        public string HisItemName { get; set; }
    }
}
