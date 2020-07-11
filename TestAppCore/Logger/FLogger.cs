using System;
using System.IO;

namespace CVProjectCore.Logger
{
    public class FLogger
    {
        private string fileName;

        private FLogger(string fileName)
        {
            this.fileName = fileName;
        }


        public void Log(string txt)
        {
            txt = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + " :: INFO :: " + txt + Environment.NewLine;

            File.AppendAllText(fileName, txt, System.Text.Encoding.UTF8);

        }

        public void Error(string txt)
        {
            txt = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + " :: ERROR :: " + txt + Environment.NewLine;

            File.AppendAllText(fileName, txt, System.Text.Encoding.UTF8);

        }

        public static FLogger Get(string LoggerName)
        {
            string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(LogDirectory)) Directory.CreateDirectory(LogDirectory);
            string fName = Path.Combine(LogDirectory, LoggerName + ".log");
            return new FLogger(fName);
        }

    }
}

