using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log_management_system
{
    internal class DatabaseHelper
    {
        private static string connectionString = "Data Source=MyLogs.db;Version=3;";

        public static void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Logs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Content TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT (datetime('now', 'localtime')),
                        UpdatedAt DATETIME DEFAULT (datetime('now', 'localtime')))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
