﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace xLiAd.ProjectFactory.Core
{
    public class CodeLoader
    {
        private readonly string[] entityProjectShouldName = new string[] { "Entities", "Entity", "Models", "Model" };
        private readonly string[] repositoryShouldName = new string[] { "Infrastructure", "Infrastructures", "Repositories", "Infrastructure", "DAL", "Dal", "DALS", "Dals" };
        private readonly string[] serviceShouldName = new string[] { "Services", "Service", "Applications", "Application", "BLL", "Bll", "BLLS", "Blls" };
        private readonly string[] webappShouldName = new string[] { "WebApp", "WebApi", "UserInterface", "Web" };
        private readonly string[] TextFileExtension = new string[] { ".cs", ".csproj", ".cshtml", ".json" };
        public string ProjectPre { get; }
        private readonly List<FileItem> fileItems;
        public CodeLoader(string codeSolutionPath, string projectPre)
        {
            ProjectPre = projectPre;
            var dict = new System.IO.DirectoryInfo(codeSolutionPath);
            var subFolders = dict.GetDirectories();
            var entityFolder = ConfirmProjectFolder(subFolders, entityProjectShouldName);
            var reposiFolder = ConfirmProjectFolder(subFolders, repositoryShouldName);
            var servicFolder = ConfirmProjectFolder(subFolders, serviceShouldName);
            var webappFolder = ConfirmProjectFolder(subFolders, webappShouldName);
            var solutionFile = dict.GetFiles($"{projectPre}.sln").FirstOrDefault();
            if (solutionFile == null)
                throw new Exception("未找到解决方案！");
            fileItems = new List<FileItem>();
            fileItems.Add(new TextFileItem(projectPre + ".sln", ReadFile(solutionFile.FullName)));
            fileItems.AddRange(ProcessFolder(entityFolder, $"/{ProjectPre}.Entities"));
            fileItems.AddRange(ProcessFolder(reposiFolder, $"/{ProjectPre}.Infrastructure"));
            fileItems.AddRange(ProcessFolder(servicFolder, $"/{ProjectPre}.Services"));
            fileItems.AddRange(ProcessFolder(webappFolder, $"/{ProjectPre}.WebApp"));
        }

        public IEnumerable<FileItem> FileItems => fileItems;

        private IEnumerable<FileItem> ProcessFolder(DirectoryInfo directoryInfo, string inPath)
        {
            foreach(var folder in directoryInfo.GetDirectories())
            {
                if (folder.Name.Equals("bin", StringComparison.OrdinalIgnoreCase) || folder.Name.Equals("obj", StringComparison.OrdinalIgnoreCase))
                    continue;
                foreach (var item in ProcessFolder(folder, inPath + "/" + folder.Name))
                    yield return item;
            }
            var rp = inPath;
            foreach (var file in directoryInfo.GetFiles())
            {
                if (TextFileExtension.Contains(file.Extension.ToLower()))
                    yield return new TextFileItem(rp + "/" + file.Name, ReadFile(file.FullName));
                else
                    yield return new FileItem(rp + "/" + file.Name, ReadFile(file.FullName));
            }
        }

        private byte[] ReadFile(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"文件不存在：{path}");
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                return bs;
            }
        }

        private DirectoryInfo ConfirmProjectFolder(System.IO.DirectoryInfo[] folers, string[] shouldNames)
        {
            foreach(var name in shouldNames)
            {
                var f = folers.Where(x => x.Name.Equals(ProjectPre + "." + name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (f != null)
                    return f;
            }
            throw new Exception("未找到需要的项目（文件夹）");
        }
    }
}