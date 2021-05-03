using xLiAdProjectTemplate.Entities;
using xLiAdProjectTemplate.Entities.Dtos;
using xLiAdProjectTemplate.Entities.QueryDtos;
using xLiAdProjectTemplate.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using xLiAd.DiagnosticLogCenter;
using xLiAd.Limiter.Core;
using xLiAdProjectTemplate.Services.Assets;

namespace xLiAdProjectTemplate.Services
{
    [AspectExceptionReturn, AspectLog]
    public class AuthRoleService : IAuthRoleService
    {
        private readonly IAuthRoleRepository authRoleRepository;
        private readonly IJsonCacheRepository jsonCacheRepository;
        private readonly IHttpClientFactory httpClientFactory;
        public AuthRoleService(IAuthRoleRepository authRoleRepository, IJsonCacheRepository jsonCacheRepository, IHttpClientFactory httpClientFactory)
        {
            this.authRoleRepository = authRoleRepository;
            this.jsonCacheRepository = jsonCacheRepository;
            this.httpClientFactory = httpClientFactory;
        }

        public IApiResultModel GetAll()
        {
            var result = authRoleRepository.All();
            DiagnosticLogCenter.AdditionLog("在这里添加额外的日志");
            return ApiListResultModel.FromData(result);
        }

        public IApiResultModel GetPageList(PageQueryDto queryDto)
        {
            Expression<Func<AuthRole, bool>> expression;
            if (queryDto.Key.NullOrEmpty())
                expression = x => true;
            else
                expression = x => x.RoleId == queryDto.Key || x.RoleName.Contains(queryDto.Key);

            var result = authRoleRepository.PageList(expression, x => x.Id, queryDto.PageIndex, queryDto.PageSize, true);
            return ApiListResultModel.FromPageData(result.Items, result.Total, result.PageIndex, result.PageSize);
        }

        public IApiResultModel Add(AuthRole authRole)
        {
            var id = authRoleRepository.Add(authRole);
            return ApiResultModel.FromData(true);
        }

        public IApiResultModel Edit(AuthRole authRole)
        {
            var result = authRoleRepository.Update(authRole);
            return ApiResultModel.FromData(result > 0);
        }

        public IApiResultModel Delete(int id)
        {
            var result = authRoleRepository.Delete(id);
            return ApiResultModel.FromData(true);
        }

        /// <summary>
        /// 切片缓存测试方法
        /// </summary>
        /// <returns></returns>
        public IApiResultModel GetModelsByRoleId()
        {
            var result = authRoleRepository.GetModelsByRoleId("RC01");
            return ApiResultModel.FromData(result);
        }
        /// <summary>
        /// 切片事务测试方法
        /// </summary>
        /// <returns></returns>
        [AspectTransaction]
        public IApiResultModel TestAopTransaction()
        {
            var maxId = jsonCacheRepository.WhereOrderSelect(x => true, x => x.Id, x => x.Id, 1, true).FirstOrDefault();
            var id = jsonCacheRepository.Add(new Entities.JsonCache()
            {
                Name = "测试",
                Value = $"这次取得的最大ID是：{maxId}，本记录ID应该是{maxId + 1}"
            });
            var i = id - id;
            var j = 1 / i;
            var updateCount = authRoleRepository.UpdateWhere(x => x.Id == 376, x => x.RoleName, "运营审批人1");

            return ApiResultModel.FromData(updateCount);
        }

        double timeOut = 0.1;
        /// <summary>
        /// 方法重试
        /// </summary>
        /// <returns></returns>
        [AspectRetry(MaxRetryTimes = 1)]
        public IApiResultModel getAnnualLeave()
        {
            var url = "http://github.com/";
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(timeOut++);
            var task = client.GetStringAsync(url);
            var back = task.Result;
            var jo = Newtonsoft.Json.JsonConvert.DeserializeObject<AnnelBack>(back);
            return ApiResultModel.FromData(jo.UserAnnelNum);
        }
        class AnnelBack
        {
            public int UserAnnelNum { get; set; }
        }

        /// <summary>
        /// 方法调用限制
        /// </summary>
        /// <returns></returns>
        [Limiter(KeyType = xLiAd.Limiter.Abstractions.KeyTypeEnum.ClientIp, LimitPolicieString = "15s:1")]
        public IApiResultModel TestLimit()
        {
            return ApiResultModel.FromData(true);
        }
    }

    public interface IAuthRoleService
    {
        IApiResultModel GetAll();
        IApiResultModel GetPageList(PageQueryDto queryDto);
        IApiResultModel Add(AuthRole authRole);
        IApiResultModel Edit(AuthRole authRole);
        IApiResultModel Delete(int id);
        IApiResultModel GetModelsByRoleId();
        IApiResultModel TestAopTransaction();
        IApiResultModel getAnnualLeave();
        IApiResultModel TestLimit();
    }
}
