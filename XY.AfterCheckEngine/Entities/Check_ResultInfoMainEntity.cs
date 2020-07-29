using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_ResultInfoMain
    /// 创 建 者：LK
    /// 创建日期：2019/12/10 10:05:53
    /// 最后修改者：LK
    /// 最后修改日期：2019/12/10 10:05:53
    /// </summary>
    [SugarTable("Check_ResultInfoMain")]
    public class Check_ResultInfoMainEntity
    {
        /// <summary>
		/// 
		/// </summary>
		public string CheckResultInfoMainCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PersonalCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionLevelName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DiseaseName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SettlementDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResultDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string YDDJ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SHLCZT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DSHWGJ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? DSHWGS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YSHWGJ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? YSHWGS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ZWGJE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ZWGSL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RuleLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SHStates { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FamilyCode { get; set; }
    }
}
