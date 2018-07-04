using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace CrlChecker
{
    class Xml
    {
        //Скачать xml-файл с указанного url и сохраняет по указанному пути:
        public XmlNodeList GetXmlFromUrlandSaveAs(string url, string xmlFolderPath)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(url);

            Logger.Write($"XML загружен с {url}");

            string version = doc.SelectSingleNode("/АккредитованныеУдостоверяющиеЦентры/Версия").InnerText;

            doc.Save($"{xmlFolderPath}\\testXml-{version}.xml");

            Logger.Write($"XML сохранен в {xmlFolderPath} под именем testXml-{version}.xml");

            XmlNodeList crlUrlList = doc.SelectNodes("/АккредитованныеУдостоверяющиеЦентры/УдостоверяющийЦентр/ПрограммноАппаратныеКомплексы/ПрограммноАппаратныйКомплекс/КлючиУполномоченныхЛиц/Ключ/АдресаСписковОтзыва/Адрес");

            string date = doc.GetElementsByTagName("Дата")[0].InnerText;

            Console.WriteLine($"Версия xml : {version}");

            Console.WriteLine($"Дата xml : {date}");

            return crlUrlList;
        }
    }
}
