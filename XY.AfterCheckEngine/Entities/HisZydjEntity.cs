using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("HIS_ZYDJ")]
    public class HisZydjEntity
    {
        public string CRowId { get; set; }
        public string YLJGBH { get; set; }
        public string ZYBH { get; set; }
        public string RYBH { get; set; }
        public string BABH { get; set; }
        public string FYLBBM { get; set; }
        public string FYLBMC { get; set; }
        public string KLXBM { get; set; }
        public string KLXMC { get; set; }
        public string KH { get; set; }
        public string HZXM { get; set; }
        public string XMJM { get; set; }
        public string SFZH { get; set; }
        public string XBBM { get; set; }
        public string XBMC { get; set; }
        public string NL { get; set; }
        public string CSRQ { get; set; }
        public string LXR { get; set; }
        public string LXDH { get; set; }
        public string MZZD { get; set; }
        public string MZZDDM { get; set; }
        public string ZYJBZD { get; set; }
        public string ZYJBZDDM { get; set; }
        public string CYJBZDONE { get; set; }
        public string CYJBZDONEDM { get; set; }
        public string CYJBZDTWO { get; set; }
        public string CYJBZDTWODM { get; set; }
        public string RYKSBM { get; set; }
        public string RYKSMC { get; set; }
        public string RYRQ { get; set; }
        public string BQ { get; set; }
        public string CWH { get; set; }
        public DateTime? BCDJRQ { get; set; }
        public string JZYS { get; set; }
        public string GCYS { get; set; }
        public string YJJE { get; set; }
        public string YLFKFSDM { get; set; }
        public string Remark { get; set; }
        public int DeleteMark { get; set; }
        public DateTime SCSJ { get; set; }
        /// <summary>
        /// 是否办理出院  1出院  0住院
        /// </summary>
        public int IsOutHos { get; set; }
        public string CYRQ { get; set; }
        public string CYBLR { get; set; }
        public string CYKSBM { get; set; }
        public string CYKSMC { get; set; }
    }
}
