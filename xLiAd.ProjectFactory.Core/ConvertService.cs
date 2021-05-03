using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.ProjectFactory.Core
{
    public class ConvertService : IConvertService
    {
        private readonly CodeLoader codeLoader;
        public ConvertService(CodeLoader codeLoader)
        {
            this.codeLoader = codeLoader;
        }

        public bool IsProjectPreValid(string projectPre)
        {
            var pattern = "([\\w]+)(\\.([\\w]+)(\\.([\\w]+))?)?";
            return System.Text.RegularExpressions.Regex.IsMatch(projectPre, pattern);
        }

        public List<FileItem> Convert(string projectPre)
        {
            if (!IsProjectPreValid(projectPre))
                throw new Exception("名称不合法！");
            List<FileItem> result = new List<FileItem>();
            foreach(var item in codeLoader.FileItems)
            {
                var name = item.FileFullName.Replace(codeLoader.ProjectPre, projectPre);
                if (item is TextFileItem itemt)
                    result.Add(new TextFileItem(name, System.Text.Encoding.UTF8.GetBytes(itemt.GetText().Replace(codeLoader.ProjectPre, projectPre))));
                else
                    result.Add(new FileItem(name, item.Content));
            }
            return result;
        }
    }

    public interface IConvertService
    {
        bool IsProjectPreValid(string projectPre);
        List<FileItem> Convert(string projectPre);
    }
}
