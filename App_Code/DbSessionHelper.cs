using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace NhanVienWebApp.App_Code
{
    public static class DbSessionHelper
    {
        private const string KEY_SERVER = "db_server";
        private const string KEY_PORT = "db_port";
        private const string KEY_USER = "db_user";
        private const string KEY_PASS = "db_pass";
        private const string KEY_DB = "db_name";
        private const string KEY_TABLE = "db_table";

        public static void SaveCredentials(string server, int port, string user, string password)
        {
            HttpContext.Current.Session[KEY_SERVER] = server;
            HttpContext.Current.Session[KEY_PORT] = port;
            HttpContext.Current.Session[KEY_USER] = user;
            HttpContext.Current.Session[KEY_PASS] = password; // chỉ ở Session (server-side memory), KHÔNG ghi Cookie/DB
        }
        public static void SaveDatabaseAndTable(string dbName, string tableName)
        {
            HttpContext.Current.Session[KEY_DB] = dbName;
            HttpContext.Current.Session[KEY_TABLE] = tableName;
        }
        public static bool HasCredentials()
        {
            return HttpContext.Current?.Session?[KEY_USER] != null;
        }
        public static bool HasDatabaseSelected()
        {
            return HttpContext.Current?.Session?[KEY_DB] != null;
        }
        public static string CurrentDatabase
        {
            get { return HttpContext.Current.Session[KEY_DB] as string; }
        }
        public static string CurrentTable
        {
            get { return HttpContext.Current.Session[KEY_TABLE] as string; }
        }
        public static MySqlConnection CreateServerConnection()
        {
            var s = HttpContext.Current.Session;
            var csb = new MySqlConnectionStringBuilder
            {
                Server = (string)s[KEY_SERVER],
                Port = (uint)(int)s[KEY_PORT],
                UserID = (string)s[KEY_USER],
                Password = (string)s[KEY_PASS],
                ConnectionTimeout = 8
            };
            return new MySqlConnection(csb.ConnectionString);
        }
        public static MySqlConnection CreateDbConnection()
        {
            var conn = CreateServerConnection();
            var s = HttpContext.Current.Session;
            string dbName = (string)s[KEY_DB];

            if (string.IsNullOrEmpty(dbName))
            {
                throw new InvalidOperationException("Không tìm thấy thông tin Database trong Session. Vui lòng chọn Database trước!");
            }
            conn.ChangeDatabase(dbName);
            return conn;
        }
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}