using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace reporting.data
{
    class mysql
    {
        private MySqlConnection connection;
        private string server = "93.119.178.245";
        private string database = "db";
        private string username = "admin";
        private string password = "admin";

        //Constructor
        public mysql()
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public int query(string sql)
        {
            int result = -1;
            //Open connection
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = connection;
                result = cmd.ExecuteNonQuery();
                CloseConnection();
            }
            return result;
        }

        //Select statement
        public IDataReader Select(string sql)
        {
            var ds = new DataTable();
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                return dt.CreateDataReader();
                
            }
            return null;
        }

        public object scalar(string sql)
        {
            object data = null;
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                data = cmd.ExecuteScalar();
                CloseConnection();

            }
            return data;

        }

    }
}
