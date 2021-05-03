
namespace xLiAd.ProjectFactory.Winform
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TbSolutionName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CbInSubFolder = new System.Windows.Forms.CheckBox();
            this.TbTargetFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnBorn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.BtnBrowser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TbSolutionName
            // 
            this.TbSolutionName.Location = new System.Drawing.Point(108, 25);
            this.TbSolutionName.Name = "TbSolutionName";
            this.TbSolutionName.Size = new System.Drawing.Size(238, 21);
            this.TbSolutionName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "解决方案名称：";
            // 
            // CbInSubFolder
            // 
            this.CbInSubFolder.AutoSize = true;
            this.CbInSubFolder.Checked = true;
            this.CbInSubFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbInSubFolder.Location = new System.Drawing.Point(238, 97);
            this.CbInSubFolder.Name = "CbInSubFolder";
            this.CbInSubFolder.Size = new System.Drawing.Size(108, 16);
            this.CbInSubFolder.TabIndex = 2;
            this.CbInSubFolder.Text = "生成在文件夹下";
            this.CbInSubFolder.UseVisualStyleBackColor = true;
            // 
            // TbTargetFolder
            // 
            this.TbTargetFolder.Location = new System.Drawing.Point(108, 53);
            this.TbTargetFolder.Name = "TbTargetFolder";
            this.TbTargetFolder.Size = new System.Drawing.Size(190, 21);
            this.TbTargetFolder.TabIndex = 3;
            this.TbTargetFolder.Text = "D:\\";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "代码放置位置：";
            // 
            // BtnBorn
            // 
            this.BtnBorn.Location = new System.Drawing.Point(384, 25);
            this.BtnBorn.Name = "BtnBorn";
            this.BtnBorn.Size = new System.Drawing.Size(105, 88);
            this.BtnBorn.TabIndex = 4;
            this.BtnBorn.Text = "生成";
            this.BtnBorn.UseVisualStyleBackColor = true;
            this.BtnBorn.Click += new System.EventHandler(this.BtnBorn_Click);
            // 
            // BtnBrowser
            // 
            this.BtnBrowser.Location = new System.Drawing.Point(305, 52);
            this.BtnBrowser.Name = "BtnBrowser";
            this.BtnBrowser.Size = new System.Drawing.Size(41, 23);
            this.BtnBrowser.TabIndex = 5;
            this.BtnBrowser.Text = "浏览";
            this.BtnBrowser.UseVisualStyleBackColor = true;
            this.BtnBrowser.Click += new System.EventHandler(this.BtnBrowser_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 147);
            this.Controls.Add(this.BtnBrowser);
            this.Controls.Add(this.BtnBorn);
            this.Controls.Add(this.TbTargetFolder);
            this.Controls.Add(this.CbInSubFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TbSolutionName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "张磊模板项目生成工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbSolutionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CbInSubFolder;
        private System.Windows.Forms.TextBox TbTargetFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnBorn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button BtnBrowser;
    }
}

