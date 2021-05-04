using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xLiAd.ProjectFactory.Core
{
    public class TextFileItem : FileItem
    {
        private readonly Encoding encoding;
        public TextFileItem(string fileFullName, byte[] content) : base(fileFullName, content)
        {
            MemoryStream ms = new MemoryStream(content);
            ms.Seek(0, SeekOrigin.Begin);
            encoding = TextEncoder.GetEncoding(ms);
        }

        public string GetText()
        {
            return encoding.GetString(Content);
        }
    }
}
