using System.Speech.Synthesis;
using ëntärtäïnër.Properties;

namespace ëntärtäïnër
{
    public static class StartupPayload
    {
        public static void Run()
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer {Volume = 100, Rate = 0};
            string[] tmp = Resources.AlabamaMinne.Split('\n');
            int i = 0;
            synthesizer.SpeakCompleted += (sender, e) =>
            {
                i++;
                if (i < tmp.Length)
                    synthesizer.SpeakAsync(tmp[i]);
            };
            synthesizer.SpeakAsync(tmp[i]);
        }
    }
}
