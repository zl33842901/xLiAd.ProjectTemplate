using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xLiAd.ProjectFactory.Core;
using xLiAd.ProjectFactory.Core.Options;

namespace xLiAd.ProjectFactory.Winform
{
    public partial class Form1 : Form
    {
        private CodeLoader codeLoader;
        private IConvertService convertService;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBorn_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(TbSolutionName.Text))
            {
                MessageBox.Show("解决方案名称不能为空！");
                return;
            }
            if(string.IsNullOrWhiteSpace(TbTargetFolder.Text))
            {
                MessageBox.Show("代码放置位置不能为空！");
                return;
            }
            if (!convertService.IsProjectPreValid(TbSolutionName.Text))
            {
                MessageBox.Show("解决方案名称不合法，请更换！");
                return;
            }
            if (TbSolutionName.Text.StartsWith("xLiAd."))
            {
                MessageBox.Show("解决方案名称不能以 xLiAd. 开头，请更换！");
                return;
            }
            if (!System.IO.Directory.Exists(TbTargetFolder.Text))
            {
                MessageBox.Show("代码放置位置不存在，请更换！");
                return;
            }
            string ActureSaveFolder;
            if (CbInSubFolder.Checked)
                ActureSaveFolder = System.IO.Path.Combine(TbTargetFolder.Text, TbSolutionName.Text);
            else
                ActureSaveFolder = TbTargetFolder.Text;

            if (System.IO.Directory.Exists(ActureSaveFolder))
            {
                var dict = new System.IO.DirectoryInfo(ActureSaveFolder);
                if (dict.GetDirectories().Any() || dict.GetFiles().Any())
                {
                    MessageBox.Show("代码放置位置非空，请更换！");
                    return;
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(ActureSaveFolder);
            }
            OptionsSelect optionsSelect = new OptionsSelect()
            {
                Items = new OptionsSelectItem[]
                {
                    new OptionsSelectItem()
                    {
                        OptionCode = "SqlType",
                        SelectOrInput = RbSqlServer.Checked ? "SqlServer" : "MySql"
                    },
                    new OptionsSelectItem()
                    {
                        OptionCode = "SqlConn",
                        SelectOrInput = TbConn.Text
                    }
                }
            };
            var fileItems = convertService.Convert(TbSolutionName.Text, optionsSelect);
            foreach(var item in fileItems)
            {
                var fullName = System.IO.Path.Combine(ActureSaveFolder, item.FileFullName.TrimStart('/'));
                var fd = Path.GetDirectoryName(fullName);
                if (!Directory.Exists(fd))
                    Directory.CreateDirectory(fd);
                using (FileStream fs = new FileStream(fullName, FileMode.Create))
                    fs.Write(item.Content, 0, item.Content.Length);
            }
            MessageBox.Show("生成成功！");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var solutionPath = System.Configuration.ConfigurationManager.AppSettings["SolutionPath"];
                var projectPre = System.Configuration.ConfigurationManager.AppSettings["ProjectPre"];
                codeLoader = new CodeLoader(solutionPath, projectPre);
                convertService = new ConvertService(codeLoader);
            }
            catch (Exception ex)
            {

            }
        }

        private void BtnBrowser_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                TbTargetFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void RbSqlServer_CheckedChanged(object sender, EventArgs e)
        {
            if (RbSqlServer.Checked)
            {
                RbMySql.Checked = false;
                TbConn.Text = "server=127.0.0.1;user id=sa;password=zhanglei;database=OKR;Max Pool Size=300;";
            }
        }

        private void RbMySql_CheckedChanged(object sender, EventArgs e)
        {
            if (RbMySql.Checked)
            {
                RbSqlServer.Checked = false;
                TbConn.Text = "server=127.0.0.1;user id=root;password=zhanglei;database=okr;CharSet=utf8mb4;Convert Zero Datetime=true;Allow Zero Datetime=true";
            }
        }
    }
}
