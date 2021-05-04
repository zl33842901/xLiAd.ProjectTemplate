using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ProjectFactory.Core.Options
{
    public class OptionChangeItem
    {
        /// <summary>
        /// input类型时，当默认字符串
        /// </summary>
        public string ItemName { get; set; }
        public bool IsDefault { get; set; }
        public OptionChangeDetail[] ChangeDetails { get; set; }
    }
}
