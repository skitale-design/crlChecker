using System;
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

            string q = $"INSERT INTO testTable(UC, signature, thisUpdate, nextUpdate, crlNumber, linkToCrl) VALUES ('{crl.issuer}',NULL, '{crl.thisTime}', '{crl.updateTime}', 'number', '{crlPath}' )";

            SQLiteCommand query = new SQLiteCommand(q, connection);

            int rezult = query.ExecuteNonQuery();

            if (rezult == 1)
            {
                Logger.Write($"Запись в БД прошла успешно.");
            }

            connection.Close();
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
