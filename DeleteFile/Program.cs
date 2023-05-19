using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace DeleteFile
{
    internal class Program
    {
        public static string Path = ConfigurationManager.AppSettings.Get("Path");
        public static int Days = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Days"));
        public static int seconds = Convert.ToInt32(ConfigurationManager.AppSettings.Get("seconds"));
        static void Main(string[] args)
        {
            DateTime now = DateTime.Now; // 今天
            DirectoryInfo di = new DirectoryInfo(string.Format(@"{0}",Path)); // 取得資料夾資訊
            delete(Path);
            deleteFolder(Path);
            Console.WriteLine(string.Format("刪除大於{0}日的檔案及空資料夾完畢，{1}秒後自動關閉程式",Days,seconds));
            Thread.Sleep(seconds*1000);
            Environment.Exit(0);
        }
        private static void delete(String dir)
        {
            DateTime dateTime = DateTime.Now;
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] files = di.GetFiles();
            DirectoryInfo[] directs = di.GetDirectories();
            foreach (FileInfo file in files)
            {
                FileInfo fileInfo = new FileInfo(dir+"\\"+file.Name);
                DateTime dates = Convert.ToDateTime(fileInfo.LastWriteTime);
                if ((dateTime - dates).Days >= Days)
                {
                    Console.WriteLine("已刪除 : "+file.Name);
                    fileInfo.Delete();
                }
            }
            foreach(DirectoryInfo dd in directs)
            {
                string dirs = dir + "\\" + dd.Name;
                delete(dirs);
            }
        }
        public static void deleteFolder(String storagepath)
        {
            DirectoryInfo dir = new DirectoryInfo(storagepath);
            DirectoryInfo[] subdirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo subdir in subdirs.Reverse())
            {
                FileSystemInfo[] subFiles = subdir.GetFileSystemInfos();
                if (subFiles.Count() == 0)
                {
                    Console.WriteLine("已刪除 : " + subdir + "空資料夾");
                    subdir.Delete();
                }
            }
        }
    }
}
