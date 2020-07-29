using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities.Dto
{
    /// <summary>
    /// 功能描述：CheckComplaintMainDto 审核结果查询及申诉功能用
    /// 创 建 者：LK
    /// 创建日期：2019/7/25 16:29:36
    /// 最后修改者：LK
    /// 最后修改日期：2019/7/25 16:29:36
    /// </summary>
    public class CheckComplaintMainDto: YBHosInfoEntity
    {
        public string ComplaintCode { get; set; }

        public string RegisterCode { get; set; }

        public string RulesCode { get; set; }
    }
    /// <summary>
    /// 初审改动  包裹类
    /// </summary>
    public class DataListByCS
    {
        public Record Record;

        public List<CheckResultInfoEntity> checkResultInfoEntities;

        public List<Check_ResultInfoMainEntity> checkResultInfoEntities2;
    }
    /// <summary>
    /// 其它改动  包裹类
    /// </summary>
    public class DataListByOther
    {
        public Record Record;

        public List<Check_Complain_MZLEntity>  check_Complain_MZLEntities;
    }
    public class Record
    {
        /// <summary>
        /// 病例数量
        /// </summary>
        public int BLSL { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public decimal? FY { get; set; }
        /// <summary>
        /// 待审核条数
        /// </summary>
        public int? DSHTS { get; set; }
        /// <summary>
        /// 待审核费用
        /// </summary>
        public decimal? DSHFY { get; set; }
    }
    public class WGConfirmEntity
    {
        /// <summary>
        /// 处方主键
        /// </summary>
        public string CheckResultPreInfoCode { get; set; }
        /// <summary>
        /// 审核结果表主键
        /// </summary>
        public string CheckResultInfoCode { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RulesName { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public string DetailName { get; set; }
        /// <summary>
        /// 各阶段状态
        /// </summary>
        public string states { get; set; }
        /// <summary>
        /// 疑点描述
        /// </summary>
        public string YDDescription { get; set; }

        public decimal? Price { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ItemCode { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 有效金额
        /// </summary>
        public decimal? YXJE { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 不可报销金额
        /// </summary>
        public decimal? BKBXJE { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 报销比例
        /// </summary>
        public decimal? CompRatio { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 数目
        /// </summary>
        public int? Count { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? DJ { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 限价
        /// </summary>
        public decimal? LimitPrice { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 超限价
        /// </summary>
        public decimal? CLimitPrice { get; set; }
    }
    public class JsonArray
    {
        /// <summary>
        /// 初审提交流程状态
        /// </summary>
        public string states { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 违规确认操作与处方表相关
        /// </summary>
        public List<WGQR> WGQR { get; set; }
        /// <summary>
        /// 违规信息添加
        /// </summary>
        public List<WGInfo> WGInfo { get; set; }
    }
    public class WGQR
    {
        public string Code { get; set; }
        /// <summary>
        /// 1 违规   0 不违规   -1 没操作
        /// </summary>
        public string Value { get; set; }
        public decimal? Price { get; set; }
    }
    public class WGInfo
    {
        public string Code { get; set; }

        public string Value { get; set; }
        public decimal? Price { get; set; }

    }
    public class DataListPreList {
        public string type;
        public List<YBHosPreInfoEntity> yBHosPreInfoEntities;
    }
}
