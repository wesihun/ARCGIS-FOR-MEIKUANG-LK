using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;

namespace XY.SystemManage.Service
{
    public class DepartmentService : IDepartmentService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public DepartmentService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Delete(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                using (var db = _dbContext.GetIntance())
                {
                    //物理删除
                    //var t0 = db.Deleteable<OrganizeEntity>().Where(it => it.OrganizeId == keyValue).ExecuteCommand();
                    //result = (t0 > 0) ? true : false;

                    var departmentEntity = new DepartmentEntity();
                    departmentEntity.DeleteMark = 0;
                    //逻辑删除
                    var count = db.Updateable(departmentEntity).UpdateColumns(it => new { it.DeleteMark })
                    .Where(it => it.Id == keyValue)
                    .ExecuteCommand();
                    return count > 0 ? true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool DeleteBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {

                using (var db = _dbContext.GetIntance())
                {
                    var departmentEntity = new DepartmentEntity();
                    departmentEntity.DeleteMark = 0;
                    //逻辑删除
                    var counts = db.Updateable(departmentEntity).UpdateColumns(it => new { it.DeleteMark })
                        .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool ExistEnCode(string BH, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<DepartmentEntity>().Any(it => it.BH == BH && it.DeleteMark == 1);
                else
                    return db.Queryable<DepartmentEntity>().Any(it => it.BH == BH && it.Id != keyValue && it.DeleteMark == 1);
            }
        }

        public bool ExistFullName(string departmentName, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<DepartmentEntity>().Any(it => it.Name == departmentName && it.DeleteMark == 1);
                else
                    return db.Queryable<DepartmentEntity>().Any(it => it.Name == departmentName && it.Id != keyValue && it.DeleteMark == 1);
            }
        }

        public List<DepartmentEntity> GetPageListByCondition(string orgid,string depname, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<DepartmentEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<DepartmentEntity>()
                .Where(it => it.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(depname), it => it.Name.Contains(depname))
                .WhereIF(!string.IsNullOrEmpty(orgid),it => it.OrgId == orgid)
                .OrderBy(it => it.SortCode,SqlSugar.OrderByType.Asc).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }

        public List<DepartmentEntity> GetListByCondition(string condition, string keyword)
        {
            var DataResult = new List<DepartmentEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<DepartmentEntity>()
                .Where((it) => it.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "BH" && !string.IsNullOrEmpty(keyword), (it) => it.BH.Contains(keyword))
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "Name" && !string.IsNullOrEmpty(keyword), (it) => it.Name.Contains(keyword))
                .OrderBy((oe) => oe.SortCode).ToList();
            }
            return DataResult;
        }

        public bool Insert(DepartmentEntity DepartmentEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(DepartmentEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }

        public bool Update(DepartmentEntity DepartmentEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(DepartmentEntity)
                .IgnoreColumns(it => new { it.DeleteMark,it.CreateTime})
                .Where(it => it.Id == DepartmentEntity.Id)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
    }
}
