using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace reporting.data
{
    class mysql
    {
        private MySqlConnection connection;
        private string server = "localhost";
        private string database = "reporting";
        private string username = "root";
        private string password = "";

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
        public DataSet Select(string sql)
        {
            var ds = new DataSet();
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                var adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);
                CloseConnection();
            }
            return ds;
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
