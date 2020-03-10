using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace Fix
{
    public static class Program
    {
        private const string FileName = "WindowsUpdate";
        private static readonly string[] prognames = {"WindowsUpdate", "UpdateMGR", "WinMan"};
        public static void Main()
        {
            Console.WriteLine("Killing running instances");
            foreach (Process p in prognames.SelectMany(Process.GetProcessesByName)) p.Kill();
            Console.WriteLine("Removing binaries");
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinMan.exe"));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), $"{FileName}.exe"));
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{FileName}.exe"));
            File.Delete(Path.Combine(Path.GetTempPath(), $"{FileName}.exe"));
            Console.WriteLine("Resetting regkeys");
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\.exe", null, "exefile");
            //@"HKEY_CURRENT_USER\Software\Classes\virus.cool.v3"
            try
            {
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes").DeleteSubKeyTree("virus.cool.v3");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception was thrown while deleting the virus.cool.v3 key. This can be ignored ({e.Message})");
            }
        }
    }
}