using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xLiAd.ProjectFactory.WebApp.Models
{
    public class ConfigModel : IConfigModel
    {
        public string SolutionPath { get; set; }
        public string ProjectPre { get; set; }
        public string ZipSavePath { get; set; }
    }

    public interface IConfigModel
    {
        string SolutionPath { get; }
        string ProjectPre { get; }
        string ZipSavePath { get; }
    }
}
