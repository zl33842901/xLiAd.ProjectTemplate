using xLiAdProjectTemplate.Entities;
using xLiAdProjectTemplate.Entities.Dtos;
using xLiAdProjectTemplate.Entities.QueryDtos;
using xLiAdProjectTemplate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xLiAdProjectTemplate.WebApp.Controllers
{
    //[Authorize]
    public class AuthRoleController : Controller
    {
        private readonly IAuthRoleService authRoleService;
        public AuthRoleController(IAuthRoleService authRoleService)
        {
            this.authRoleService = authRoleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IApiResultModel Add(AuthRole authRole)
        {
            return authRoleService.Add(authRole);
        }

        [HttpPost]
        public IApiResultModel Edit(AuthRole authRole)
        {
            return authRoleService.Edit(authRole);
        }

        [HttpPost]
        public IApiResultModel Delete(int id)
        {
            return authRoleService.Delete(id);
        }

        [HttpPost]
        public IApiResultModel GetListData(PageQueryDto queryDto)
        {
            var result = authRoleService.GetPageList(queryDto);
            return result;
        }

        [HttpPost]
        public IApiResultModel TestAopCache()
        {
            var result = authRoleService.GetModelsByRoleId();
            return result;
        }

        [HttpPost]
        public IApiResultModel TestAopTransaction()
        {
            var result = authRoleService.TestAopTransaction();
            return result;
        }

        [HttpPost]
        public IApiResultModel TestRetry()
        {
            var result = authRoleService.getAnnualLeave();
            return result;
        }

        [HttpPost]
        public IApiResultModel TestLimit()
        {
            var result = authRoleService.TestLimit();
            return result;
        }
    }
}
