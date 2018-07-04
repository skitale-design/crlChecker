using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using Org.BouncyCastle;
using System.IO;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Cryptography.X509Certificates;



namespace CrlChecker
{
    class Crl
    {
        public CrlInfo Crlinfo { get; private set; }

        public struct CrlInfo
        {
            public string issuer;
            public byte[] signature;
            public DateTime thisTime, updateTime;

            public CrlInfo(string _issuer, byte[] _signature, DateTime _thisTime, DateTime _updateTime)
            {
                this.issuer = _issuer;
                this.signature = _signature;
                this.thisTime = _thisTime;
                this.updateTime = _updateTime;
            }
        }

        public void GetNumberOfCrlFromUrlAndSaveToFolder(int numberOfCrlToByDownloaded, XmlNodeList crlUrlsList, string saveToFolder)
        {
            WebClient webcli = new WebClient();

            int i = 1;
            int len = crlUrlsList.Count;

            foreach (XmlNode node in crlUrlsList)
            {
                string crlUrl = node.InnerText;

                if (i == numberOfCrlToByDownloaded + 1)
                {
                    break;
                }

                try
                {
                    string crlSavePath = $"{saveToFolder}\\crl{i}.crl";
                    Console.Write($"Скачивание CRL №{i}/{len}..........");
                    Logger.Write($"Скачивание CRL №{i}/{len}..........");
                    webcli.DownloadFile(crlUrl, crlSavePath);
                    Console.WriteLine($"ОК");
                    Logger.Write($"ОК");

                    Logger.Write($"Скачивание CRL №{ i}/{len}.......... CRL с адреса {node.InnerText} скачан успешно.");

                    i++;
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.Message);

                    //Console.WriteLine(e.Status);

                    if (e.Status.ToString() == "ConnectionFailure")
                    {
                        Console.WriteLine(e.Status);
                        Logger.Write($"URL {node} не отвечает - скачивание не удалось. Ошибка: {e.Message}");
                        continue;
                    }

                }
            }
        }

        //public void GetCrlInfo2(string fileName, Org.BouncyCastle.Math.BigInteger serialNumber, Org.BouncyCastle.X509.X509Certificate cert)

        //Отдает данные из Crl-файла в виде массива строк:
        public string[] GetCrlInfoAsArray(string fileName)
        {
            try
            {
                byte[] buf = ReadFile(fileName);
                X509CrlParser clrParser = new X509CrlParser();
                X509Crl crl = clrParser.ReadCrl(buf);

                var issuer = crl.IssuerDN;
                var signature = crl.GetSignature();
                DateTime nextupdate = crl.NextUpdate.Value;
                DateTime thisUpdate = crl.ThisUpdate;

                Console.WriteLine("Issuerdata.tostring = {0}", issuer.ToString());
                Console.WriteLine("Signature.ToString = {0}", signature.ToString());
                Console.WriteLine("NextUpdate = {0}", nextupdate.ToString());
                Console.WriteLine("ThisUpdate = {0}", thisUpdate);

                Logger.Write($"Извлечение данных из crl-файла: {fileName}");
                Logger.Write($"issuer: {issuer}");
                Logger.Write($"signature: {signature}");
                Logger.Write($"nextupdate: {nextupdate}");
                Logger.Write($"thisupdate: {thisUpdate}");

                CrlInfo CrlInfo1 = new CrlInfo(issuer.ToString(),signature,nextupdate,thisUpdate);
                //CrlInfo CrlInfo1 = new CrlInfo(issuer.ToString(),BitConverter.ToInt32(signature,0),nextupdate,thisUpdate);

                string[] array = {issuer.ToString(),nextupdate.ToString(),thisUpdate.ToString() };

                return array;

            }
            catch (Exception ex)
            {
                string[] array = { "Операция не удалась" };
                Console.WriteLine(ex.Message);
                return array;
            }
        }

        //Отдает содержимое Crl-файла в виде структуры CrlInfo:
        public CrlInfo GetCrlInfoAsStructure(string fileName)
        {
            try
            {
                byte[] buf = ReadFile(fileName);
                X509CrlParser clrParser = new X509CrlParser();
                X509Crl crl = clrParser.ReadCrl(buf);

                var issuer = crl.IssuerDN;
                var signature = crl.GetSignature();
                DateTime nextupdate = crl.NextUpdate.Value;
                DateTime thisUpdate = crl.ThisUpdate;

                Console.WriteLine("Issuerdata.tostring = {0}", issuer.ToString());
                Console.WriteLine("Signature.ToString = {0}", signature.ToString());
                Console.WriteLine("NextUpdate = {0}", nextupdate.ToString());
                Console.WriteLine("ThisUpdate = {0}", thisUpdate);

                Logger.Write($"Извлечение данных из crl-файла: {fileName}");
                Logger.Write($"issuer: {issuer}");
                Logger.Write($"signature: {signature}");
                Logger.Write($"nextupdate: {nextupdate}");
                Logger.Write($"thisupdate: {thisUpdate}");

                CrlInfo CrlInfo = new CrlInfo(issuer.ToString(), signature, nextupdate, thisUpdate);

                return CrlInfo;
            }
            catch (Exception ex)
            {
                Crlinfo = new CrlInfo();
                Console.WriteLine(ex.Message);
                return Crlinfo;
            }
        }

        //Reads a file.
        internal static byte[] ReadFile(string fileName)
        {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        
        public static void generateCrl(string crlPath, string testCrlPath)
        {
            byte[] certbyte = File.ReadAllBytes(crlPath);
            Console.WriteLine("First byte: {0}", certbyte[0]);
            string pem = "-----BEGIN X509 CRL-----\r\n" + Convert.ToBase64String(certbyte, Base64FormattingOptions.InsertLineBreaks) + "-----END X509 CRL-----";
            using (StreamWriter outputFile = new StreamWriter($"{testCrlPath}\\test.crl"))
            {
                outputFile.Write(pem);
            }
        }
    }
}
