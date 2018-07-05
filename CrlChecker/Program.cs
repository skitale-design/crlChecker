using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.Data.SQLite;
using System.Reflection;

namespace CrlChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectFolderPath = Path.GetFullPath(@"..\..\..\tmp");

            string xmlFolderPath = Path.GetFullPath(@"..\..\..\tmp");

            string crlFolderPath = Path.GetFullPath(@"..\..\..\tmp\crl");

            int numberOfCrlToByDownloaded = 10;

            string mask = "*.crl";

            string dbPath = Path.GetFullPath(@"..\..\..\tmp\CrlCheckTestDb.db");

            string logFilePath = Path.GetFullPath(@"..\..\..\tmp\log.txt");

            Logger.SetPath(logFilePath);

            Db db = new Db(dbPath);

            string query = "default";

            db.CreateTable(query);

            Logger.Write("Start ---------------------------------------------");

            Xml xml = new Xml();

            XmlNodeList crlUrlList = xml.GetXmlFromUrlandSaveAs("http://e-trust.gosuslugi.ru/CA/DownloadTSL?schemaVersion=0", xmlFolderPath);

            Crl crl = new Crl();

            crl.GetNumberOfCrlFromUrlAndSaveToFolder(numberOfCrlToByDownloaded, crlUrlList, crlFolderPath);

            string[] filePaths = Directory.GetFiles(crlFolderPath, mask);

            foreach (string filePath in filePaths)
            {
                Console.WriteLine($"Для загрузки файла {filePath} нажмите любую клавишу...");

                Console.ReadKey();

                string crlPath = filePath;

                if (!(new FileInfo(filePath).Length == 0))
                {
                    Crl.CrlInfo crlInfo = crl.GetCrlInfoAsStructure(crlPath);

                    db.WriteCrlToDbFromStructure(crlInfo, crlPath);
            }
                else
                {
                Console.WriteLine($"Файл {filePath} пуст!");

                continue;
            }
        }

            Console.WriteLine("Для завершения нажмите любую клавишу...");

            Console.ReadKey();
        }
    }
}
