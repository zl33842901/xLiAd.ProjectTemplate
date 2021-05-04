using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using xLiAd.ProjectFactory.Core;
using xLiAd.ProjectFactory.Core.Options;
using xLiAd.ProjectFactory.WebApp.Models;

namespace xLiAd.ProjectFactory.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CodeLoader codeLoader;
        private readonly IConvertService convertService;
        private readonly IConfigModel configModel;
        public HomeController(CodeLoader codeLoader, IConvertService convertService, IConfigModel configModel)
        {
            this.codeLoader = codeLoader;
            this.convertService = convertService;
            this.configModel = configModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IApiResultModel GetOptionsConfig()
        {
            return ApiResultModel.FromData(codeLoader.Options);
        }

        public IApiResultModel DoConvert([FromBody] ConvertDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SolutionName))
            {
                return ApiResultModel.FromError("解决方案名称不能为空！");
            }
            if (!convertService.IsProjectPreValid(dto.SolutionName))
            {
                return ApiResultModel.FromError("解决方案名称不合法，请更换！");
            }
            if (dto.SolutionName.StartsWith("xLiAd."))
            {
                return ApiResultModel.FromError("解决方案名称不能以 xLiAd. 开头，请更换！");
            }
            OptionsSelect optionsSelect = new OptionsSelect()
            {
                Items = dto.Items
            };
            var fileItems = convertService.Convert(dto.SolutionName, optionsSelect);
            using (MemoryStream zipToOpen = new MemoryStream())
            {
                using(ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    foreach (var item in fileItems)
                    {
                        var en = archive.CreateEntry(item.FileFullName.TrimStart('/'));
                        using (var stream = en.Open())
                            stream.Write(item.Content, 0, item.Content.Length);
                    }
                }
                if (!Directory.Exists(configModel.ZipSavePath))
                    Directory.CreateDirectory(configModel.ZipSavePath);
                var fileName = $"{dto.SolutionName}.{DateTime.Now:yyy-MM-dd-HH-mm-ss}.zip";
                var fullName = Path.Combine(configModel.ZipSavePath, fileName);
                var bs = zipToOpen.ToArray();
                using (var fs = new FileStream(fullName, FileMode.Create))
                    fs.Write(bs, 0, bs.Length);
                return ApiResultModel.FromData(fileName);
            }
        }
    }
}
