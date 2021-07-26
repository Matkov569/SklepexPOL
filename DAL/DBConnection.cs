using System;
using System.Collections.Generic;
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
            stringBuilder.Server = Properties.Settings.Default.server;
            stringBuilder.Database = Properties.Settings.Default.database;
            stringBuilder.Port = Properties.Settings.Default.port;
            stringBuilder.Password = Properties.Settings.Default.passwd;
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
                    while(dataReader.Read())
                            nastanie.Add(new Country(dataReader["nazwa"].ToString(),(int)dataReader["ilosc"].ToString(),(int)dataReader["cena"],(int)dataReader["wysokosc"].ToString,(int)dataReader["marza"].ToString,(int)dataReader["magazyn"];

                }
                else
                {
                    Console.WriteLine("Brak wynikow zapytania");
                }
                connection.Close();

            }

            return nastanie;

        }
        
            
    }
}
