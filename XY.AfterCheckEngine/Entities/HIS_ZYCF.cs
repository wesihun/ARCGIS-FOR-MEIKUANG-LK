using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("HIS_ZYCF")]
    public class HIS_ZYCF
    {
        /// <summary>
        /// 唯一标识码
        /// </summary>
        public string CRowId { get; set; }
        /// <summary>
        /// 医疗机构编号
        /// </summary>
        public string YLJGBH { get; set; }
        /// <summary>
        /// 住院编号
        /// </summary>
        public string ZYBH { get; set; }
        /// <summary>
        /// 病人标识号
        /// </summary>
        public string RYBH { get; set; }
        /// <summary>
        /// 处方编号
        /// </summary>
        public string CFBH { get; set; }
        /// <summary>
        /// 处方序号
        /// </summary>
        public string CFXH { get; set; }
        /// <summary>
        /// 票据编号
        /// </summary>
        public string PJBH { get; set; }
        /// <summary>
        /// 医保对照项目编码（很重要，不能空）
        /// </summary>
        public string YBDZXMBM { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string XMBM { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string XMMC { get; set; }
        /// <summary>
        /// 费用类别编码
        /// </summary>
        public string XMLBBM { get; set; }
        /// <summary>
        /// 费用类别名称
        /// </summary>
        public string XMLBMC { get; set; }
        /// <summary>
        /// 开单科室编码
        /// </summary>
        public string KDKSBM { get; set; }
        /// <summary>
        /// 开单科室名称
        /// </summary>
        public string KDKSMC { get; set; }
        /// <summary>
        /// 开单医生
        /// </summary>
        public string KDYS { get; set; }
        /// <summary>
        /// 执行科室编码
        /// </summary>
        public string ZXKSBM { get; set; }
        /// <summary>
        /// 执行科室名称
        /// </summary>
        public string ZXKSMC { get; set; }
        /// <summary>
        /// 执行医生
        /// </summary>
        public string ZXYS { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string GG { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string DW { get; set; }
        /// <summary>
        /// 一次用量
        /// </summary>
        public string YCYL { get; set; }
        /// <summary>
        /// 用量单位
        /// </summary>
        public string YLDW { get; set; }
        /// <summary>
        /// 频次
        /// </summary>
        public string PC { get; set; }
        /// <summary>
        /// 数量（支持传负数冲正）
        /// </summary>
        public int? SL { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? DJ { get; set; }
        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal? SJJE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SCSJ { get; set; }
    }
}
