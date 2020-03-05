using MySql.Data.MySqlClient;

using System.Data;
using System.Data.SqlClient;

namespace CacheDecorator.Repository.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        public DatabaseHelper(string wLDOConnectionString, string mySQLConnectionString)
        {
            this.WLDOConnectionString = wLDOConnectionString;

            this.MySQLConnectionString = mySQLConnectionString;
        }

        /// <summary>
        ///WLDO連線字串
        /// </summary>
        public string WLDOConnectionString { get; }

        /// <summary>
        /// MySQL
        /// </summary>
        public string MySQLConnectionString { get; }

        public IDbConnection GetConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);

            return conn;
        }

        public IDbConnection GetMySQLConnection(string connectionString)
        {
            var conn = new MySqlConnection(connectionString);

            return conn;
        }
    }
}