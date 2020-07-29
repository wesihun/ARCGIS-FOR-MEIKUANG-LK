using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XY.SystemManage.Entities;

namespace XY.SystemManage.WebApi
{
    public class AutoMapperConfigs : Profile
    {
        //添加实体映射关系.
        public AutoMapperConfigs()
        {
            //RoleEntity转RoleDto.
            CreateMap<RoleEntity, RoleDto>();
            //RoleDto转RoleEntity
            CreateMap<RoleDto, RoleEntity>();

            CreateMap<AreaEntity, AreaDto>();
            CreateMap<AreaDto, AreaEntity>();

            CreateMap<ModuleEntity, ModuleDto>();
            CreateMap<ModuleDto, ModuleEntity>();

            CreateMap<OrganizeEntity, OrganizeDto>();
            CreateMap<OrganizeDto, OrganizeEntity>();

            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();

            CreateMap<DataDictEntity, DataDictDto>();
            CreateMap<DataDictDto, DataDictEntity>();

            CreateMap<UserRoleEntity, UserRoleDto>();
            CreateMap<UserRoleDto, UserRoleEntity>();

            CreateMap<RoleModuleEntity, RoleModuleDto>();
            CreateMap<RoleModuleDto, RoleModuleEntity>();

            CreateMap<UserModuleButtonEntity, UserModuleButtonDto>();
            CreateMap<UserModuleButtonDto, UserModuleButtonEntity>();

            CreateMap<LogEntity, LogDto>();
            CreateMap<LogDto, LogEntity>();
        }
    }
}
