using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.Utilities;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;

namespace XY.SystemManage.Service
{
    public class OrganizeService : IOrganizeService
    {
        
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public OrganizeService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public List<OrganizeDto> GetAll()
        {
            var DataResult = new List<OrganizeDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<OrganizeEntity>().Where(oe => oe.DeleteMark == 1)
                .OrderBy((oe) => oe.SortCode)
                .Select((oe) => new OrganizeDto
                {
                    OrganizeId = oe.OrganizeId,
                    ParentId = oe.ParentId,
                    OrgName = oe.OrgName,
                    OrgCode = oe.OrgCode,
                    OrgBrevityCode = oe.OrgBrevityCode,
                    AreaName = oe.PAreaName + oe.SAreaName +oe.XAreaName,
                    Manager = oe.Manager,
                    ManagerPhone = oe.ManagerPhone,
                    Address = oe.Address,
                    Remark = oe.Remark,
                    SortCode = oe.SortCode
                }).ToList();
            }
            return DataResult;
        }

        public List<OrganizeDto> GetPageListByCondition(string orgname, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<OrganizeDto>();
            using (var db = _dbContext.GetIntance())
            {               
                DataResult = db.Queryable<OrganizeEntity>().Where(oe => oe.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(orgname), (oe) => oe.OrgName.Contains(orgname))
               .OrderBy((oe) => oe.SortCode)
               .Select((oe) => new OrganizeDto
               {
                   OrganizeId = oe.OrganizeId,
                   ParentId = oe.ParentId,
                   OrgName = oe.OrgName,
                   OrgCode = oe.OrgCode,
                   OrgBrevityCode = oe.OrgBrevityCode,
                   PAreaCode = oe.PAreaCode,
                   SAreaCode = oe.SAreaCode,
                   XAreaCode = oe.XAreaCode,
                   PAreaName = oe.PAreaName,
                   SAreaName = oe.SAreaName,
                   XAreaName = oe.XAreaName,
                   Manager = oe.Manager,
                   ManagerPhone = oe.ManagerPhone,
                   Address = oe.Address,
                   Remark = oe.Remark,
                   SortCode = oe.SortCode
               }).ToPageList(pageIndex, pageSize, ref totalCount);
                foreach(var list in DataResult)
                {
                    list.AreaName = list.PAreaName + list.SAreaName + list.XAreaName;
                }
            }
            return DataResult;
        }

        public List<OrganizeDto> GetTableSelect(string orgName,string orgid, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<OrganizeDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<OrganizeEntity>().Where(oe => oe.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(orgName), (oe) => oe.OrgName.Contains(orgName))
                .WhereIF(!string.IsNullOrEmpty(orgid), (oe) => oe.OrganizeId == orgid)
               .OrderBy((oe) => oe.SortCode)
               .Select((oe) => new OrganizeDto
               {
                   OrganizeId = oe.OrganizeId,
                   ParentId = oe.ParentId,
                   OrgName = oe.OrgName,
                   OrgCode = oe.OrgCode,
                   OrgBrevityCode = oe.OrgBrevityCode,
                   PAreaCode = oe.PAreaCode,
                   SAreaCode = oe.SAreaCode,
                   XAreaCode = oe.XAreaCode,
                   PAreaName = oe.PAreaName,
                   SAreaName = oe.SAreaName,
                   XAreaName = oe.XAreaName,
                   Manager = oe.Manager,
                   ManagerPhone = oe.ManagerPhone,
                   Address = oe.Address,
                   Remark = oe.Remark,
                   SortCode = oe.SortCode
               }).ToPageList(pageIndex, pageSize, ref totalCount);
                foreach (var list in DataResult)
                {
                    list.AreaName = list.PAreaName + list.SAreaName + list.XAreaName;
                }
            }
            return DataResult;
        }

        /// <summary>
        /// 根据父Id获取子机构信息
        /// </summary>
        /// <returns></returns>
        public List<OrganizeDto> GetList(string ParentId)
        {
            var DataResult = new List<OrganizeDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<OrganizeEntity>().Where(oe => oe.DeleteMark == 1 && oe.ParentId == ParentId )
                .OrderBy((oe) => oe.SortCode)
                .Select((oe) => new OrganizeDto
                {
                    OrganizeId = oe.OrganizeId,
                    ParentId = oe.ParentId,
                    OrgName = oe.OrgName,
                    OrgCode = oe.OrgCode,
                    OrgBrevityCode = oe.OrgBrevityCode,
                    AreaCode = oe.XAreaCode,
                    Manager = oe.Manager,
                    ManagerPhone = oe.ManagerPhone,
                    Address = oe.Address,
                    Remark = oe.Remark,
                    SortCode = oe.SortCode,
                    CreateUserId = oe.CreateUserId,
                    CreateUserName = oe.CreateUserName,
                    CreateDate = oe.CreateDate,
                    ModifyDate = oe.ModifyDate,
                    ModifyUserId = oe.ModifyUserId,
                    ModifyUserName = oe.ModifyUserName,
                    DeleteMark = oe.DeleteMark
                }).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrganizeDto GetEntityById(string keyValue)
        {
            var DataResult = new OrganizeDto();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<OrganizeEntity>().Where(oe => oe.DeleteMark == 1 && oe.OrganizeId == keyValue )
                .OrderBy((oe) => oe.SortCode)
                .Select((oe) => new OrganizeDto
                {
                    OrganizeId = oe.OrganizeId,
                    ParentId = oe.ParentId,
                    OrgName = oe.OrgName,
                    OrgCode = oe.OrgCode,
                    OrgBrevityCode = oe.OrgBrevityCode,
                    AreaCode = oe.XAreaCode,
                    Manager = oe.Manager,
                    ManagerPhone = oe.ManagerPhone,
                    Address = oe.Address,
                    Remark = oe.Remark,
                    SortCode = oe.SortCode,
                    CreateUserId = oe.CreateUserId,
                    CreateUserName = oe.CreateUserName,
                    CreateDate = oe.CreateDate,
                    ModifyDate = oe.ModifyDate,
                    ModifyUserId = oe.ModifyUserId,
                    ModifyUserName = oe.ModifyUserName,
                    DeleteMark = oe.DeleteMark

                }).ToList().SingleOrDefault();
            }
            return DataResult;
        }

       
        #endregion

        #region 验证数据
        /// <summary>
        /// 机构代码不能重复
        /// </summary>
        /// <param name="orgCode">机构代码</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string orgCode, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<OrganizeEntity>().Any(it => it.OrgCode == orgCode && it.DeleteMark == 1);
                else
                    return db.Queryable<OrganizeEntity>().Any(it => it.OrgCode == orgCode && it.OrganizeId != keyValue && it.DeleteMark == 1);
            }
        }
        /// <summary>
        /// 机构名称不能重复
        /// </summary>
        /// <param name="orgName">机构名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string orgName, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<OrganizeEntity>().Any(it => it.OrgName == orgName && it.DeleteMark == 1);
                else
                    return db.Queryable<OrganizeEntity>().Any(it => it.OrgName == orgName && it.OrganizeId != keyValue && it.DeleteMark == 1);
            }
        }
        /// <summary>
        /// 是否存在下级机构
        /// </summary>
        /// <param name="keyValue">parentId</param>
        /// <returns></returns>
        public bool ExistLower(string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                return db.Queryable<OrganizeEntity>().Any(it => it.ParentId == keyValue && it.DeleteMark == 1);
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public bool Insert(OrganizeEntity organizeEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(organizeEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public bool Update(OrganizeEntity organizeEntity)
        {

            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(organizeEntity)
                .IgnoreColumns(it => new { it.CreateDate, it.CreateUserId, it.CreateUserName, it.DeleteMark })
                .Where(it => it.OrganizeId == organizeEntity.OrganizeId)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool Delete(string keyValue, string UserId, string UserName)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                using (var db = _dbContext.GetIntance())
                {
                    //物理删除
                    //var t0 = db.Deleteable<OrganizeEntity>().Where(it => it.OrganizeId == keyValue).ExecuteCommand();
                    //result = (t0 > 0) ? true : false;

                    var organizeEntity = new OrganizeEntity();
                    organizeEntity.DeleteMark = 0;
                    organizeEntity.ModifyUserId = UserId;
                    organizeEntity.ModifyUserName = UserName;
                    organizeEntity.ModifyDate = DateTime.Now;
                    //逻辑删除
                    var count = db.Updateable(organizeEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                    .Where(it => it.OrganizeId == keyValue)
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
        /// <summary>
        /// 批量删除机构
        /// </summary>
        /// <param name="keyValues">主键List</param>
        public bool DeleteBatch(List<string> keyValues, string UserId, string UserName)
        {
            if (keyValues.Count() > 0)
            {

                using (var db = _dbContext.GetIntance())
                {
                    var organizeEntity = new OrganizeEntity();
                    organizeEntity.DeleteMark = 0;
                    organizeEntity.ModifyUserId = UserId;
                    organizeEntity.ModifyUserName = UserName;
                    organizeEntity.ModifyDate = DateTime.Now;

                    //逻辑删除
                    var counts = db.Updateable(organizeEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                        .Where(it => keyValues.Contains(it.OrganizeId)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;

                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public List<OrganizeDto> GetPageListToTreeTable(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
