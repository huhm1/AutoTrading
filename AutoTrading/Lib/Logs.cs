using System;
using System.IO;

namespace AutoTrading.Lib
{
    public class Logs
    {
        string LogFileName;
        //private File LogFile;
        private StreamWriter streamWriter;
        public Logs(string fileName, bool append)
        {
            LogFileName = Client.FilePath + fileName;
            streamWriter = new StreamWriter(LogFileName, append)
            {
                AutoFlush = true
            };
        }
        public Logs(string path, string fileName, bool append)
        {
            LogFileName = path + fileName;
            streamWriter = new StreamWriter(LogFileName, append)
            {
                AutoFlush = true
            };
        }

        public void Close()
        {
            streamWriter.Close();
        }
        public void WriteT(string message)
        {
            string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", " + message;
            streamWriter.WriteLine(line);
        }
        public void Write(string message)
        {
            string line = message;
            streamWriter.WriteLine(line);
        }

    }
}
