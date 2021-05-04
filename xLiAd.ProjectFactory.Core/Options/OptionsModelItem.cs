using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ProjectFactory.Core.Options
{
    public class OptionsModelItem
    {
        public string OptionCode { get; set; }

        public string OptionDescription { get; set; }

        public OptionType Type { get; set; }

        public OptionChangeItem[] Options { get; set; }
    }
}
