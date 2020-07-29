using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Comp_ClinicCostMain")]
    public class CompClinicCostMainEntity
    {
       public string CantonCode_Ch { get; set; }
      public string Year_Ch{ get; set; }
    public string CCM_SettlementCode_Vc{ get; set; }
public string CCM_ClinicRegisterCode_Vc{ get; set; }
        public string CCM_AppropriateCode_Vc{ get; set; }
        public decimal? CCM_Fee_Dec{ get; set; }
        public decimal? CCM_AllowedComp_Dec{ get; set; }
        public decimal? CCM_UnallowedComp_Dec { get; set; }
        public decimal? CCM_RealComp_Dec { get; set; }
        public decimal? CCM_FamilyAccountPay_Dec { get; set; }
        public decimal? CCM_ClinicFundPay_Dec { get; set; }
        public decimal? CCM_SelfPay_Dec { get; set; }
        public string CCM_FistCheckFlag_Ch{ get; set; }
        public decimal? CCM_FirstCheckSum_Dec { get; set; }
        public string CCM_SecondCheckFlag_Ch{ get; set; }
        public decimal? CCM_SecondCheckSum_Dec { get; set; }
        public DateTime? CCM_SettlementDate_Dt{ get; set; }
        public decimal? CCM_ExtraYear_Dec { get; set; }
        public decimal? CCM_Extra_Dec { get; set; }
        public decimal? CCM_NextYearFee_Dec { get; set; }
        public string RecordPerson_Vc{ get; set; }
        public  DateTime? RecordDate_Dt{ get; set; }
        public string DeleteFlag_Ch{ get; set; }
        public decimal? CCM_UpAccountBalance_Dec{ get; set; }
        public decimal? CCM_NowAccountBalance_Dec{ get; set; }
        public int? sjtbbz{ get; set; }
        public string CCM_FirstCheckComment_Vc{ get; set; }
        public string CCM_SecondCheckComment_Vc{ get; set; }
        public decimal? CCM_TcRealComp_Dec{ get; set; }
        public string CCM_FirstCheckPerson_Vc{ get; set; }
        public string CCM_SecondCheckPerson_Vc{ get; set; }
        public DateTime? CCM_FirstCheckDate_Dt{ get; set; }
        public DateTime?  CCM_SecondCheckDate_Dt { get; set; }
        public DateTime? CCM_AppropriateDate { get; set; }
        public decimal? CCM_Wgkoufei_Dec{ get; set; }
        public string  CCM_WgkoufeiResult_Vc{ get; set; }
        public decimal? CCM_GeneralItemFee_Dec{ get; set; }
        //public DateTime? sjc{ get; set; }
        public decimal? CCM_gjjyp { get; set; }
        public decimal? CCM_sjyp { get; set; }
        public string CheckStates{ get; set; }
    }
}
