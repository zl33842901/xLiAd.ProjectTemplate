using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xLiAd.ProjectFactory.Core.Options;

namespace xLiAd.ProjectFactory.WebApp.Models
{
    public class ConvertDto
    {
        public string SelectedId { get; set; }
        public OptionsSelectItem[] Items { get; set; }

        public string SolutionName { get; set; }
    }
}
