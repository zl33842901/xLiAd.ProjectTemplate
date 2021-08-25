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
        private readonly IEnumerable<CodeLoader> codeLoader;
        private readonly IEnumerable<IConvertService> convertService;
        private readonly IConfigModel configModel;
        public HomeController(IEnumerable<CodeLoader> codeLoader, IEnumerable<IConvertService> convertService, IConfigModel configModel)
        {
            this.codeLoader = codeLoader;
            this.convertService = convertService;
            this.configModel = configModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IApiResultModel GetOptionsConfig(string id)
        {
            var cl = codeLoader.Where(x => x.Id == id).FirstOrDefault();
            if (cl == null)
                cl = codeLoader.FirstOrDefault();
            return ApiResultModel.FromData(cl?.Options);
        }

        public IApiResultModel DoConvert([FromBody] ConvertDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SolutionName))
            {
                return ApiResultModel.FromError("解决方案名称不能为空！");
            }
            var cs = convertService.Where(x => x.Id == dto.SelectedId).FirstOrDefault();
            if(cs == null)
            {
                return ApiResultModel.FromError("请选择有效的模板类型！");
            }
            if (!cs.IsProjectPreValid(dto.SolutionName))
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
            try
            {
                var fileItems = cs.Convert(dto.SolutionName, optionsSelect);
                using (MemoryStream zipToOpen = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
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
                    var fileName = $"{dto.SolutionName}.{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.zip";
                    var fullName = Path.Combine(configModel.ZipSavePath, fileName);
                    var bs = zipToOpen.ToArray();
                    using (var fs = new FileStream(fullName, FileMode.Create))
                        fs.Write(bs, 0, bs.Length);
                    return ApiResultModel.FromData(fileName);
                }
            }
            catch(Exception ex)
            {
                return ApiResultModel.FromError(ex.Message);
            }
        }

        public IActionResult GetFile(string fn)
        {
            var fullName = Path.Combine(configModel.ZipSavePath, fn);
            var fs = new FileStream(fullName, FileMode.Open);
            return File(fs, "application/x-zip-compressed", fn, true);
        }
    }
}
