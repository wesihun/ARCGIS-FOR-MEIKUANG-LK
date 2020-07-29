using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.ZnshBusiness.Entities;
using XY.Universal.Models.ViewModels;
namespace XY.AfterCheckEngine.IService
{
    public interface IHealthCareCheckResultService
    {
        List<HealthCareCheckResultDto> healthCareCheckResultDtos(string resgisterCode,string flag);

        List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string code,string flag);
    }
}
