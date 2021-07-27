using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SklepexPOL.DAL
{
    class DBConnection
    {
        private MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();
        private MySqlConnection connection;

        private static string ALL_NASTANIE_QUERY = "SELECT * FROM nastanie";

        private static DBConnection instance = null;
        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBConnection();
                return instance;
            }
        }

        public MySqlConnection Connection => new MySqlConnection(stringBuilder.ToString());


        private DBConnection()
        {
            stringBuilder.UserID = Properties.Settings.Default.userID;
            //stringBuilder.UserID = Properties.Settings.Default.Rlogin;
            stringBuilder.Server = Properties.Settings.Default.server;
            stringBuilder.Database = Properties.Settings.Default.database;
            stringBuilder.Port = Properties.Settings.Default.port;
            stringBuilder.Password = Properties.Settings.Default.passwd;
            //stringBuilder.Password = Properties.Settings.Default.Rpasswd;
        }

        public List<NaStanie> GetNaStanies()
        {
            List<NaStanie> nastanie = new List<NaStanie>();
            using(connection = new MySqlConnection(stringBuilder.ToString()))
            {
                MySqlCommand command = new MySqlCommand(ALL_NASTANIE_QUERY,connection);
                connection.Open();
                var dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    //while(dataReader.Read())
                    //        nastanie.Add(new Country(dataReader["nazwa"].ToString(),(int)dataReader["ilosc"].ToString(),(int)dataReader["cena"],(int)dataReader["wysokosc"].ToString,(int)dataReader["marza"].ToString,(int)dataReader["magazyn"];

                }
                else
                {
                    Console.WriteLine("Brak wynikow zapytania");
                }
                connection.Close();

            }

            return nastanie;

        }
        
        //czy jest po³¹czenie z baz¹
        public bool IsConnection()
        {
            MySqlConnectionStringBuilder connect = new MySqlConnectionStringBuilder();
            connect.UserID = Properties.Settings.Default.Rlogin;
            connect.Server = Properties.Settings.Default.server;
            connect.Port = Properties.Settings.Default.port;
            connect.Password = Properties.Settings.Default.Rpasswd;
            try
            {
                connection = new MySqlConnection(connect.ToString());
                connection.Open();
                connection.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //czy jest po³¹czenie z baz¹ danych
        public bool IsDBConnection()
        {
            MySqlConnectionStringBuilder connect = new MySqlConnectionStringBuilder();
            connect.UserID = Properties.Settings.Default.Rlogin;
            connect.Server = Properties.Settings.Default.server;
            connect.Database = "sklepexPOL";
            connect.Port = Properties.Settings.Default.port;
            connect.Password = Properties.Settings.Default.Rpasswd;
            try
            {
                connection = new MySqlConnection(connect.ToString());
                connection.Open();
                MySqlCommand command = new MySqlCommand("Select * from kategorie", connection);
                if (command.ExecuteReader().HasRows)
                {
                    connection.Close();
                    return true;
                }
                else 
                { 
                    connection.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //importowanie bazy danych
        public bool DBImport()
        {
            MySqlConnectionStringBuilder connect = new MySqlConnectionStringBuilder();
            connect.UserID = Properties.Settings.Default.Rlogin;
            connect.Server = Properties.Settings.Default.server;
            connect.Port = Properties.Settings.Default.port;
            connect.Password = Properties.Settings.Default.Rpasswd;
            connect.AllowUserVariables = true;
            connect.AllowBatch = true;
            if (!File.Exists(@"../../database/baza.sql"))
            {
                return false;
            }
            else
            {
                string query = File.ReadAllText(@"../../database/baza.sql");
                using (connection = new MySqlConnection(connect.ToString()))
                {
                    MySqlScript script = new MySqlScript(connection, query);
                    connection.Open();
                    script.Execute();
                    connection.Close();
                }
            }
            return true;
        }

    }
}
