using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace xLiAdProjectTemplate.WebApp.Controllers
{
    /// <summary>
    /// 0，基本的增删改查。
    /// 1，修改模板 刷新即生效。
    /// 2，修改配置 刷新即生效。
    /// 3，日志（手动、切片）
    /// 4，切片缓存
    /// 5，切片事务
    /// 6，超时与重试
    /// 7，行为限制
    /// 8，登录信息丢失后提示登录
    /// 9，导出Excel
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginCallback()
        {
            var url = Request.Query["RedirectUrl"].ToString();
            if (url.NullOrEmpty())
                url = "/";
            return Content("<script type=\"text/javascript\">location=\"" + url + "\";</script>", "text/html");
        }

        public IActionResult DialogLoginCallback()
        {
            var js = Request.Query["code"].ToString();
            return Content("<script type=\"text/javascript\">" + js + "</script>", "text/html");
        }
    }
}
