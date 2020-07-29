using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Universal.Models.ViewModels;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.IService
{
    public interface IDecisionAnalysisService
    {
        /// <summary>
        /// 获取“万能神药”表的所有数据
        /// </summary>
        /// <returns></returns>
        List<AllPowerfulDrugEntity> GetAll();

        List<TreeModule> GetInstitutionList(string level,string year);
        List<string> GetDrugName();
        bool Insert(AllPowerfulDrugEntity allpowerfuldrugEntity);

        bool InsertOrUpdate(string crowid,string drugcode,string drugname,string commoname);

        bool IsExsitsDrugCode(string drugcode);

        List<StaticsViewModel> GetStaticsViews(string flag,string drugname);

        List<StaticsViewModel> GetStaticsViews_JGJB(string flag, string drugname);

        List<StaticsViewModel> GetStaticsViews_JGMC(string flag, string drugname,int pageIndex, int pageSize, ref int totalCount);

        List<StaticsViewModel> GetStaticsViewsByTable(string flag, string drugname);

        List<StaticsViewModel> GetStaticsViewsByTable_JGJB(string flag, string drugname);

        List<StaticsViewModel> GetStaticsViewsByTable_JGMC(string flag, string drugname, int pageIndex, int pageSize, ref int totalCount);
        List<StaticsViewModel> GetStatisticsDrugInfos();
        bool UpdateResultStatusNew(CheckDrugStatusEntity checkResultStatusNew);

        CheckDrugStatusEntity GetCheckResultStatusNew(string flag);

        List<StaticsViewModel> GetStaticsViewsByRule(string flag,string year);

        List<StaticsViewModel> GetStaticsViewsByJGJB(string flag,string year);

        List<StaticsViewModel> GetStaticsViewsJGMCByRule(string rulename,string flag,string year);

        List<StaticsViewModel> GetStaticsViewsJGMCByDJ(string djname, string flag,string jgbm,string year);

        List<YBHosInfoEntityDto> GetYBHosInfoList(string year,string jgmc, string rulename,string djname, int pageIndex, int pageSize, ref int totalCount);

        string CreateDrug(string commonname);

        string CreateNewDrug(string commonname);
        bool Delete(string commonname);

        List<CheckResultInfoDto> GetCheckResultInfosByRules(string year,string yljgdjName, string institutionName, string ruleCode, string dataType, int pageIndex, int pageSize, ref int totalCount);
    }
}
