using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystem.Controllers
{
    public class HomeController : Controller
    {
        Configuration.Configuration config = new Configuration.ConfigurationProvider(@"Configuration\configuration.json")
            .LoadConfig();

        public JsonResult GetCountFiles([FromBody] DirectoryParametrs dirParam)
        {
            if (!Directory.Exists(config.PathToRootDir))
                Directory.CreateDirectory(config.PathToRootDir);

            var path = Path.Combine(config.PathToRootDir, dirParam.Path);
            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            var files = dirInfo.GetFiles();

            int countLess = 0;
            int countMiddle = 0;
            int countMore = 0;

            foreach (var dir in dirs)
            {
                long size = Directories.GetSize(dir);
                if (size <= 10000000)
                    countLess++;
                if (size > 10000000 & size <= 50000000)
                    countMiddle++;
                if (size >= 100000000)
                    countMore++;
            }

            foreach (var file in files)
            {
                if (file.Length <= 10000000)
                    countLess++;
                if (file.Length > 10000000 & file.Length <= 50000000)
                    countMiddle++;
                if (file.Length >= 100000000)
                    countMore++;
            }

            return Json(new { less = countLess, middle = countMiddle, more = countMore });
        }

        public string GetCurrentPath([FromBody] DirectoryParametrs dirParam)
        {
            if (!Directory.Exists(config.PathToRootDir))
                Directory.CreateDirectory(config.PathToRootDir);

            var path = Path.Combine(config.PathToRootDir, dirParam.Path);
            return new DirectoryInfo(path).FullName.ToString();
        }

        public JsonResult GetFiles([FromBody] DirectoryParametrs dirParam)
        {
            if (!Directory.Exists(config.PathToRootDir))
                Directory.CreateDirectory(config.PathToRootDir);

            var path = config.PathToRootDir;
            var dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles().ToList()
                .Select(x => new { Name = x.Name, Path = Path.Combine(config.RootDirName, x.Name) }).ToList();

            if (dirParam.Path != null)
            {
                path = Path.Combine(path, dirParam.Path);
                dirInfo = new DirectoryInfo(path);

                files = dirInfo.GetFiles().ToList()
                    .Select(x => new { Name = x.Name, Path = Path.Combine(config.RootDirName, dirParam.Path, x.Name) })
                    .ToList();
            }

            List<DirectoryParametrs> dirs = new List<DirectoryParametrs>();
            if(dirParam.Path != null & dirParam.Path != "")
                dirs.Add(new DirectoryParametrs() { Name = "..", Path = new DirectoryInfo(config.PathToRootDir)
                    .FullName.ToString() });

            var allDirs = dirInfo.GetDirectories().ToList()
                .Select(x => new DirectoryParametrs() { Name = x.Name, Path = x.FullName }).ToList();
            foreach (var dir in allDirs)
                dirs.Add(dir);

            return Json(new { dirs = dirs, files = files });
        }
    }
}