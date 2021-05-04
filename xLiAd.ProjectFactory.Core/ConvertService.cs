using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Options.OptionChangeItem> GetChangeItems(Options.OptionsSelect select)
        {
            List<Options.OptionChangeItem> listOptions = new List<Options.OptionChangeItem>();
            if ((codeLoader.Options?.Items.Any() ?? false) && (select?.Items.Any() ?? false))
            {
                foreach (var sel in select.Items)
                {
                    var sl = codeLoader.Options.Items.Where(x => x.OptionCode == sel.OptionCode).FirstOrDefault();
                    if (sl == null)
                        continue;
                    if (sl.Type == Options.OptionType.Radio)
                    {
                        var ss = sl.Options.Where(x => x.ItemName == sel.SelectOrInput).FirstOrDefault();
                        if (ss != null && !ss.IsDefault)
                            listOptions.Add(ss);
                    }
                    else
                    {
                        var ss = new Options.OptionChangeItem()
                        {
                            ItemName = sel.SelectOrInput,
                            ChangeDetails = sl.Options.Where(x => !x.IsDefault).FirstOrDefault().ChangeDetails.Select(x =>
                             new Options.OptionChangeDetail()
                             {
                                 FileName = x.FileName,
                                 OldeValue = x.OldeValue,
                                 NewValue = x.NewValue == "$" ? sel.SelectOrInput : x.NewValue
                             }).ToArray()
                        };
                        listOptions.Add(ss);
                    }
                }
            }
            return listOptions;
        }

        public List<FileItem> Convert(string projectPre, Options.OptionsSelect select = null)
        {
            if (!IsProjectPreValid(projectPre))
                throw new Exception("名称不合法！");

            var cd = GetChangeItems(select).SelectMany(x => x.ChangeDetails).ToArray();


            List<FileItem> result = new List<FileItem>();
            foreach(var item in codeLoader.FileItems)
            {
                var name = item.FileFullName.Replace(codeLoader.ProjectPre, projectPre);
                if (item is TextFileItem itemt)
                {
                    var changes = cd.Where(x => item.FileFullName.EndsWith(x.FileName, StringComparison.OrdinalIgnoreCase)).ToArray();
                    var cnt = itemt.GetText().Replace(codeLoader.ProjectPre, projectPre);
                    foreach (var cg in changes)
                        cnt = cnt.Replace(cg.OldeValue, cg.NewValue);
                    result.Add(new TextFileItem(name, System.Text.Encoding.UTF8.GetBytes(cnt)));
                }
                else
                    result.Add(new FileItem(name, item.Content));
            }
            return result;
        }
    }

    public interface IConvertService
    {
        bool IsProjectPreValid(string projectPre);
        List<FileItem> Convert(string projectPre, Options.OptionsSelect select = null);
    }
}
