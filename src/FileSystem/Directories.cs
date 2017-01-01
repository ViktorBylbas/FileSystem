using System.IO;

namespace FileSystem
{
    public class Directories
    {
        public static long GetSize(DirectoryInfo di)
        {
            long size = 0;

            var files = di.GetFiles();
            foreach (var file in files)
                size += file.Length;

            var dirs = di.GetDirectories();
            foreach (var dir in dirs)
                size += GetSize(dir);

            return (size);
        }
    }
}
