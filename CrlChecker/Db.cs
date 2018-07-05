using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CrlChecker
{
    class Db
    {
        private string dbPath;
        private string connectionString;

        public Db(string dbPath)
        {
            this.dbPath = dbPath;
            this.connectionString = $"Data Source={dbPath};";
        }

        public void CreateDb()
        {
            SQLiteConnection.CreateFile(dbPath);
            Console.WriteLine(File.Exists(dbPath) ? $"База данных по пути {dbPath} успешно создана!" : "Возникли ошибки при создании :(");
        }

        //TODO try-catch в методах

        //Писать в БД данные, которые получаем в виде массива строк:
        internal void WriteCrlToDbFromArray(string[] array)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string q = $"INSERT INTO testTable(UC, thisUpdate, nextUpdate) VALUES ('{array[2]}', '{array[1]}')";


            SQLiteCommand query = new SQLiteCommand(q,connection);

            query.ExecuteNonQuery();

            connection.Close();
        }

        //Писать в БД данные, которые получаем в виде стрктуры данных CrlInfo:
        internal void WriteCrlToDbFromStructure(Crl.CrlInfo crl, string crlPath)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string q = $"INSERT INTO test(UC, signature, thisUpdate, nextUpdate, crlNumber, linkToCrl) VALUES ('{crl.issuer}',NULL, '{crl.thisTime}', '{crl.updateTime}', 'number', '{crlPath}' )";

            SQLiteCommand query = new SQLiteCommand(q, connection);

            int rezult = query.ExecuteNonQuery();

            if (rezult == 1)
            {
                Logger.Write($"Запись в БД прошла успешно.");
            }

            connection.Close();
        }

        public void CreateTable(string query)
        {
            try
            {

                string q = "CREATE TABLE test (id INTEGER PRIMARY KEY AUTOINCREMENT,UC STRING,signature BLOB,thisUpdate STRING,nextUpdate STRING,crlNumber INTEGER, linkToCrl STRING);";

                if (query == "default")
                {
                    query = q;
                }

                SQLiteConnection connection = new SQLiteConnection(connectionString);

                connection.Open();

                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                connection.Close();

                Logger.Write($"Таблица в базе данных {dbPath} создана. Выполнен запрос: {q}");
            }
            catch (Exception e)
            {
                Logger.Write($"Таблица уже существует (текст ошибки: {e.Message})");
            }
        }

        //Послать любой запрос в БД
        internal void SendQueryToDb(string query)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            SQLiteCommand Query = new SQLiteCommand(query ,connection);

            Query.ExecuteNonQuery();

            connection.Close();
        }
    }
}
