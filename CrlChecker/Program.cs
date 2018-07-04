using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.Data.SQLite;

namespace CrlChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlFolderPath = @"C:\Users\avilin\Documents\S\tmp";
            string crlFolderPath = @"C:\Users\avilin\Documents\S\tmp\crl";
            int numberOfCrlToByDownloaded = 10;
            string mask = "*.crl";
            string dbPath = @"C:\Users\avilin\Documents\S\tmp\test.db";
            string logFilePath = @"C:\Users\avilin\Documents\S\tmp\log.txt";

            Logger.SetPath(logFilePath);

            Logger.Write("Start ---------------------------------------------");

            //Xml xml = new Xml();

            //XmlNodeList crlUrlList = xml.GetXmlFromUrlandSaveAs("http://e-trust.gosuslugi.ru/CA/DownloadTSL?schemaVersion=0", xmlFolderPath);

            Crl crl = new Crl();

            //crl.GetNumberOfCrlFromUrlAndSaveToFolder(numberOfCrlToByDownloaded, crlUrlList, crlFolderPath);

            string[] filePaths = Directory.GetFiles(crlFolderPath, mask);

            foreach (string filePath in filePaths)
            {
                Console.WriteLine($"Для загрузки файла {filePath} нажмите любую клавишу...");

                Console.ReadKey();

                string crlPath = filePath;

                Crl.CrlInfo crlInfo = crl.GetCrlInfoAsStructure(crlPath);

                Db db = new Db(dbPath);

                db.WriteCrlToDbFromStructure(crlInfo, crlPath);

            }

            Console.WriteLine("Для завершения нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
