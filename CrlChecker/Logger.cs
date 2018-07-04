using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlChecker
{
    public class Logger
    {
        static private string logPath;

        //Комментарий для проверки работы коммита
        //коммит из дома

        static public void Write(string text)
        {
            using (StreamWriter sw = new StreamWriter(logPath, append: true))
            {
                sw.WriteLine(String.Format($"{DateTime.Now.ToString()} : {text}"));
            }
        }

        internal static void SetPath(string logFilePath)
        {
            logPath = logFilePath;
        }
    }
}
