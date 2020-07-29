using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Comp_HosCostMain")]
    public class CompHosCostMainEntity
    {
        public string CantonCode_Ch { get; set; }
        public string Year_Ch { get; set; }
        public string HCM_SettlementCode_Vc { get; set; }
        public string HCM_HosRegisterCode_Vc { get; set; }
        public string HCM_HosRealRegisterCode_Vc { get; set; }
        public decimal? HCM_TotalFee_Dec { get; set; }
        public decimal? HCM_AllowedComp_Dec { get; set; }
        public decimal? HCM_UnallowedComp_Dec { get; set; }
        public decimal? HCM_RealComp_Dec { get; set; }
        public decimal? HCM_SelfPay_Dec { get; set; }
        public decimal? HCM_FirstCheckSum_Dec { get; set; }
        public decimal? HCM_SecondCheckSum_Dec { get; set; }
        public DateTime? HCM_SettlementDate_Dt { get; set; }
        public decimal? HCM_Extra_Dec { get; set; }
        public decimal? HCM_ExtraYear_Dec { get; set; }
        public decimal? HCM_DrugSum_Dec { get; set; }
        public decimal? HCM_ItemSum_Dec { get; set; }
        public decimal? HCM_NextYearFee_Dec { get; set; }
        public decimal? HCM_CompBalance_Dec { get; set; }
        public string HCM_AppropriateCode_Vc { get; set; }
        public string RecordPerson_Vc { get; set; }
        public DateTime? RecordDate_Dt { get; set; }
        public string DeleteFlag_Ch { get; set; }
        public string HCM_FistCheckFlag_Ch { get; set; }
        public string HCM_SecondCheckFlag_Ch { get; set; }
        public int? sjtbbz { get; set; }
        public string HCM_FirstCheckDecision_Vc { get; set; }
        public string HCM_SecondCheckDecision_Vc { get; set; }
        public int? HCM_CompCounts_Int { get; set; }
        public string Mz_SettleCode_Vc { get; set; }
        public decimal? Jfc_CompSum_dec { get; set; }
        public decimal? Jfc_PkandWzzCompSum_dec { get; set; }
        public decimal? HCM_DiseaseTotallComp_Dec { get; set; }
        public decimal? HCM_SpecialItemComp_Dec { get; set; }
        public decimal? HCM_Wgkoufei_Dec { get; set; }
        public string HCM_WgkoufeiResult_Vc { get; set; }
        public string HCM_FirstCheckPerson_Vc { get; set; }
        public string HCM_SecondCheckPerson_Vc { get; set; }
        public DateTime? HCM_FirstCheckDate_Dt { get; set; }
        public DateTime? HCM_SecondCheckDate_Dt { get; set; }
        public decimal? HCM_WaveUpperLineSum_Dec { get; set; }
        public DateTime? HCM_AppropriateDate { get; set; }
        public decimal? HCM_crffje_Dec { get; set; }
        public decimal? HCM_cfffbcje_Dec { get; set; }
        public decimal? HCM_crffce_Dec { get; set; }
        public decimal? HCM_ybzlfje_Dec { get; set; }
        public DateTime? sjc { get; set; }
        public string HCM_fsth_Vc { get; set; }
        public decimal? HCM_qfx_Dec { get; set; }
        public string HCM_sfsbd { get; set; }
        public decimal? HCM_gjjyp { get; set; }
        public decimal? HCM_sjyp { get; set; }
        public decimal? HCM_Mzje { get; set; }
        public decimal? HCM_dbzyycd { get; set; }
        public string znsh_ss { get; set; }
        public string HCM_AppealDescription_Vc { get; set; }
        public decimal? HCM_MZcomp_Dec { get; set; }
        public decimal? HCM_ECBC_Dec { get; set; }
        public string HCM_SecondSettmentFlag_Ch { get; set; }
        public DateTime? HCM_SecondSettmentDate_Dt { get; set; }
        public string ydjy { get; set; }
        public int? BNDCOMPCOUNT { get; set; }
        public decimal? ZTJSBZ { get; set; }
        public decimal? JLFYZE { get; set; }
        public decimal? YLFYZE { get; set; }
        public decimal? YLZLJE { get; set; }
        public decimal? YLYPZLJE { get; set; }
        public decimal? YLZLZLJE { get; set; }
        public decimal? YLCLFZLJE { get; set; }
        public decimal? YLQTFZLJE { get; set; }
        public decimal? GRZLFY { get; set; }
        public decimal? GRZLFFY { get; set; }
        public decimal? CXJZFJE { get; set; }
        public decimal? BCZHZFJE { get; set; }
        public decimal? TCZCJE { get; set; }
        public decimal? JZJZFJE { get; set; }
        public decimal? GRXJZF { get; set; }
        public decimal? QFBZZF { get; set; }
        public decimal? YPFY { get; set; }
        public decimal? ZLFY { get; set; }
        public decimal? FWSSFY { get; set; }
        public decimal? DDYLJGZFFY { get; set; }
        public decimal? TJTZTCZFFY { get; set; }
        public decimal? ZHYE { get; set; }
        public decimal? ZFFY { get; set; }
        public decimal? ZFYPJE { get; set; }
        public decimal? YLTCZFJE { get; set; }
        public decimal? YLJZJZFJE { get; set; }
        public decimal? CWFZE { get; set; }
        public decimal? MZXMFHJBYLFY { get; set; }
        public decimal? MZXMTCZCFY { get; set; }
        public decimal? HCM_CWF { get; set; }
        public string DBBS { get; set; }
        public string SybxFlag { get; set; }
        public string SybxSettleFlag { get; set; }
        public string flag { get; set; }
        public string CheckStates { get; set; }
    }
}
