using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace FS
{
    public class FileSystem
    {
        public FileSystem()
        {
            //...
        }

        public static bool RunCmd(string cmd, string arg)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C " + cmd + " " + arg;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                if(process.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            

            return true;
        }

        public static bool CreateFolder(string path)
        {
            bool ret;
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                ret = true;
            }
            catch(Exception e)
            {
                ret = false;
            }            
            return ret;
        }

        public static bool RemoveFolder(string path)
        {
            bool ret;
            try
            {
                Directory.Delete(path, true);
                ret = true;
            }
            catch (Exception e)
            {
                ret = false;
            }
            return ret;
        }

        public static bool RemoveFile(string path)
        {
            bool ret;
            try
            {
                File.Delete(path);
                ret = true;
            }
            catch (Exception e)
            {
                ret = false;
            }
            return ret;
        }

        public static bool CreateFile(string path)
        {
            bool ret;
            try
            {
                File.Create(path);
                ret = true;
            }
            catch (Exception e)
            {
                ret = false;
            }
            return ret;
        }

        public static bool IsFolder(string path)
        {
            return Directory.Exists(path);
        }

        public static bool IsFile(string path)
        {
            
            return File.Exists(path);
        }

        public static string[] GetFileList(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                IEnumerable<FileInfo> ie = di.EnumerateFiles();
                int count = ie.Count();
                string[] ret = new string[count];
                int i = 0;
                foreach (FileInfo f in ie)
                {
                    ret[i] = f.Name;
                    ++i;
                }
                return ret;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string[] GetFolderList(string path)
        {
            
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                IEnumerable<DirectoryInfo> ie = di.EnumerateDirectories();
                int count = ie.Count();
                string[] ret = new string[count];
                int i = 0;
                foreach(DirectoryInfo d in ie)
                {
                    ret[i] = d.Name;
                    ++i;
                }
                return ret;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool ReadFile(string path, out byte[] arr)
        {
            arr = new byte[10];
            return true;
        }

        public static bool CopyFile(string src, string dest)
        {
            try
            {
                File.Copy(src, dest);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool CopyFolder(string src, string dest)
        {
            if(Directory.Exists(src))
            {

            }
            else
            {
                return false;
            }

            if(RunCmd("xcopy", "\"" + src + "\" " + "\"" + dest + "\" /e /i /q"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
