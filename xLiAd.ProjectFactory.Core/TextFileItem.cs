using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ProjectFactory.Core
{
    public class TextFileItem : FileItem
    {
        public TextFileItem(string fileFullName, byte[] content) : base(fileFullName, content) { }

        public string GetText()
        {
            return System.Text.Encoding.UTF8.GetString(Content);
        }
    }
}
