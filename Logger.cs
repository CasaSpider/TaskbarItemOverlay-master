using System;
using System.Configuration;
using System.IO;

namespace WpfApplication1
{
    public class Logger
    {
        public DateTime Timestamp { get; set; }
        public string Userid { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public static string LogDir = ConfigurationManager.AppSettings["LogDir"];

        public Logger(string userid, string oldValue, string newValue)
        {
            Timestamp = DateTime.Now;
            Userid = userid;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public void Write()
        {
            string f = $@"{LogDir}\TaskbarItemOverlay.log";
            string line = $"{Timestamp.ToString("yyyy.MM.dd_HH:mm:ss")},{Userid},{OldValue},{NewValue}\n";
            File.AppendAllText(f, line);
        }
    }
}
