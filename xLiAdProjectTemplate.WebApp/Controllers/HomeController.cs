using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using xLiAdProjectTemplate.Entities.Dtos;

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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _hostingEnvironment)
        {
            _logger = logger;
            this._hostingEnvironment = _hostingEnvironment;
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

        [AllowAnonymous, RequestSizeLimit(1024_000_000)]
        public IActionResult Upload()
        {
            var files = Request.Form.Files;
            var file = files.FirstOrDefault();
            if (file == null)
            {
                return Json(ApiResultModel.FromError("未找到上传文件！"));
            }
            var path = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "UploadFiles", DateTime.Now.ToString("yyyy-MM"));
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            var timeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var ext = System.IO.Path.GetExtension(file.FileName).ToLower();
            var filename = timeString + ext;
            var fp = Path.Combine(path, filename);
            using (var stream = new FileStream(fp, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            var url = "/UploadFiles/" + DateTime.Now.ToString("yyyy-MM") + "/" + filename;
            var host = "http://" + Request.Host + url;
            return Json(new { location = host });
        }
    }
}
