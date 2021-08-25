using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xLiAd.ProjectFactory.WebApp.Models
{
    public class ConfigModel : IConfigModel
    {
        public TemplateModel[] Templates { get; set; }
        public string ZipSavePath { get; set; }
    }

    public class TemplateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SolutionPath { get; set; }
        public string ProjectPre { get; set; }
    }

    public interface IConfigModel
    {
        TemplateModel[] Templates { get; }
        string ZipSavePath { get; }
    }
}
