using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xLiAd.ProjectFactory.Core
{
    public class FileItem
    {
        public FileItem(string fileFullName, byte[] content)
        {
            if (string.IsNullOrWhiteSpace(fileFullName))
                throw new ArgumentNullException(nameof(fileFullName));
            var sa = fileFullName.Replace('\\', '/').Split('/');
            FileName = sa.Last();
            FilePath = string.Join("/", sa.Take(sa.Length - 1));
            Content = content;
        }
        public string FileName { get; }
        public string FilePath { get; }
        public byte[] Content { get; }

        public string FileFullName => FilePath + "/" + FileName;

        public override string ToString()
        {
            return FileFullName;
        }
    }
}
