using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SklepexPOL.DAL
{
    using SklepexPOL.Model;
    class DBConnection
    {
        private MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();
        private MySqlConnection connection;

        MySqlConnectionStringBuilder connect = new MySqlConnectionStringBuilder();

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

            connect.UserID = Properties.Settings.Default.Rlogin;
            connect.Server = Properties.Settings.Default.server;
            connect.Port = Properties.Settings.Default.port;
            connect.Password = Properties.Settings.Default.Rpasswd;
            connect.AllowUserVariables = true;
            connect.AllowBatch = true;
            connect.Database = "sklepexPOL";
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
            MySqlConnectionStringBuilder connectL = new MySqlConnectionStringBuilder();
            connectL.UserID = Properties.Settings.Default.Rlogin;
            connectL.Server = Properties.Settings.Default.server;
            connectL.Port = Properties.Settings.Default.port;
            connectL.Password = Properties.Settings.Default.Rpasswd;
            connectL.AllowUserVariables = true;
            connectL.AllowBatch = true;
            try
            {
                connection = new MySqlConnection(connectL.ToString());
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
            MySqlConnectionStringBuilder connectL = new MySqlConnectionStringBuilder();
            connectL.UserID = Properties.Settings.Default.Rlogin;
            connectL.Server = Properties.Settings.Default.server;
            connectL.Port = Properties.Settings.Default.port;
            connectL.Password = Properties.Settings.Default.Rpasswd;
            connectL.AllowUserVariables = true;
            connectL.AllowBatch = true;
            if (!File.Exists(@"../../database/baza.sql"))
            {
                return false;
            }
            else
            {
                string query = File.ReadAllText(@"../../database/baza.sql");
                using (connection = new MySqlConnection(connectL.ToString()))
                {
                    MySqlScript script = new MySqlScript(connection, query);
                    connection.Open();
                    script.Execute();
                    connection.Close();
                }
            }
            return true;
        }

        //zmienna odwo³uj¹ca sie do struktur
        structs structs = new structs();

        //pobieranie listy dostawców i ich ofert
        public List<structs.dostawcy> Sellers()
        {
            Console.WriteLine("Sellers");
            List<structs.dostawcy> dostawcy = new List<structs.dostawcy>();
            string query = "select * from dostawcy";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var sellers = command.ExecuteReader();
                if (sellers.HasRows)
                {
                    while (sellers.Read())
                    {
                        //int _id, string _name, int _ddaysv, double _marginv, 
                        //string _country, string _taxname, double _taxvalue
                        dostawcy.Add(new structs.dostawcy(
                            int.Parse(sellers["ID"].ToString()),
                            sellers["Dostawca"].ToString(),
                            int.Parse(sellers["Czas_dostawy"].ToString()),
                            double.Parse(sellers["Marza"].ToString()),
                            sellers["Kraj"].ToString(),
                            sellers["Podatek"].ToString(),
                            double.Parse(sellers["Wysokosc"].ToString())));
                    }
                }
                connection.Close();
            }
            return dostawcy;
        }
        public List<List<structs.produkty>> SellersOffer(List<structs.dostawcy> dostawcy)
        {
            Console.WriteLine("SellersOffer");
            List<List<structs.produkty>> produkty = new List<List<structs.produkty>>();
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                foreach(structs.dostawcy item in dostawcy)
                {
                    List<structs.produkty> lista = new List<structs.produkty>();
                    Console.WriteLine(item.ID);
                    string query = "call oferta(" + item.ID + ")";
                    Console.WriteLine(query);
                    MySqlCommand command = new MySqlCommand(query, connection);
                    Console.WriteLine("uuu");
                    MySqlDataReader result = command.ExecuteReader();
                    Console.WriteLine("uuu");

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            //int _id, string _name, double _pricev, 
                            //string _taxname, double _taxvalue
                            lista.Add(new structs.produkty(
                                int.Parse(result["ID_prod"].ToString()),
                                result["Nazwa"].ToString(),
                                double.Parse(result["Cena"].ToString()),
                                result["Podatek"].ToString(),
                                double.Parse(result["Wysokosc"].ToString())));
                        }
                    }
                    produkty.Add(lista);
                    result.Close();
                }
                connection.Close();
            }
            return produkty;
        }

        //wczytanie listy zamowien
        public List<structs.zam> orders()
        {
            Console.WriteLine("orders");
            List<structs.zam> dict = new List<structs.zam>();

            string query = "select * from doDostarczenia Where ID_zam>0";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        Console.WriteLine("pêtla");
                        string subquery = "select sum(Ilosc) from pro_zam where Zamowienie = " + result["ID_zam"].ToString();
                        Console.WriteLine(subquery);
                        //MySqlCommand command2 = new MySqlCommand(subquery, connection);
                        Console.WriteLine("pêtla");
                        //var result2 = command2.ExecuteScalar();
                        int val = integer(subquery);
                        Console.WriteLine("pêtla");
                        //Console.WriteLine(result2);
                        dict.Add(new structs.zam(
                            int.Parse(result["ID_zam"].ToString()),
                            result["Nazwa"].ToString(),
                            DateTime.Parse(result["Data_zamowienia"].ToString()),
                            DateTime.Parse(result["Data_dostarczenia"].ToString()),
                            double.Parse(result["Koszt"].ToString()),
                            //int.Parse(result2.ToString())));
                            val));
                        
                    }
                }
                connection.Close();
            }

            return dict;
        }

        //wczytanie listy na stanie
        public Dictionary<string, double[]> onHouse(DateTime Today)
        {
            Console.WriteLine("onHouse");
            Dictionary<string, double[]> dict = new Dictionary<string, double[]>();
            string query = "select * from naStanie";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        Console.WriteLine("pêtla");
                        dict.Add(
                            result["Nazwa"].ToString() + " #" + result["Magazyn"].ToString(),
                            new double[]
                            {
                                double.Parse(result["Ilosc"].ToString()),
                                double.Parse(result["Cena"].ToString()),
                                double.Parse(result["Wysokosc"].ToString()),
                                double.Parse(result["Marza"].ToString()),
                                (DateTime.Parse(result["Termin_przydatnosci"].ToString()) - Today).TotalDays,
                                double.Parse(result["Magazyn"].ToString()),
                                double.Parse(result["ID_stan"].ToString())
                            });
                    }
                }
                connection.Close();
            }
            //p.Nazwa, s.Ilosc, p.Cena, pod.Wysokosc, m.Marza, 
            //Termin_przydatnosci, z.Magazyn

            //{ "produkt #id_zam":[ilosc, cena, wysokoœæ podatku, mar¿a dostawcy, 
            //termin wa¿noœci(ile dni zosta³o), id_zam, id_stan]}
            return dict;
        }

        //wczytanie danych o sklepie
        public void shopData()
        {

        }



        //mysql zwróæ int
        public int integer(string query)
        {
            Console.WriteLine("integer");
            int integer;
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                var result = command.ExecuteScalar();
                integer = int.Parse(result.ToString());
                connection.Close();
            }
            return integer;
        }

        //mysql zwróæ double
        public double duble(string query)
        {
            Console.WriteLine("duble");
            double dubel;
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                var result = command.ExecuteScalar();
                dubel = double.Parse(result.ToString());
                connection.Close();
            }
            return dubel;
        }

        //mysql zwróæ string
        public string str(string query)
        {
            Console.WriteLine("str");
            string str;
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                var result = command.ExecuteScalar();
                str = result.ToString();
                connection.Close();
            }
            return str;
        }

        //wywo³ywanie query
        public bool execute(string query)
        {
            Console.WriteLine("execute");
            bool bul;
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlScript command = new MySqlScript(connection, query);
                var result = command.Execute();
                bul = result != 0 ? true : false;
                connection.Close();
            }
            return bul;
        }

        //insert - zwraca id wstawionego
        public int insertID(string query)
        {
            Console.WriteLine("InsertID");
            int id;
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                var result = command.ExecuteScalar();
                id = int.Parse(result.ToString());
                connection.Close();
            }
            return id;
        }

        //podgl¹d zamowienia
        public string order(int id)
        {
            Console.WriteLine("order");
            string str = "";
            string query = "call podglad(" + id + ")";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        str += result["Nazwa"] + " : " + result["Ilosc"] + "\n";
                    }
                }
                connection.Close();
            }
            return str;
        }

        //pobieranie daty 
        public DateTime Today()
        {
            Console.WriteLine("Today");
            DateTime today;
            string query = "select Dzisiaj from info";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteScalar();
                today = DateTime.Parse(result.ToString());
                connection.Close();
            }
            return today;
        }

        //info o sklepie
        public Dictionary<string, double> appOpen()
        {
            Console.WriteLine("AppOpen");
            Dictionary<string, double> dict = new Dictionary<string, double>();
            string query = "select * from appOpen";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                var result = command.ExecuteReader();
                result.Read();
                dict.Add("Saldo", double.Parse(result["Saldo"].ToString()));
                dict.Add("Dochod_dzienny", double.Parse(result["Dochod_dzienny"].ToString()));
                dict.Add("Wydatki_dzienne", double.Parse(result["Wydatki_dzienne"].ToString()));
                dict.Add("Ruch", double.Parse(result["Ruch"].ToString()));
                dict.Add("Liczba_pracownikow", double.Parse(result["Liczba_pracownikow"].ToString()));
                dict.Add("Rodzaj", double.Parse(result["Rodzaj"].ToString()));
                dict.Add("Poziom", double.Parse(result["Poziom"].ToString()));
                dict.Add("Marza", double.Parse(result["Marza"].ToString()));
                dict.Add("oplaty_miesieczne", double.Parse(result["oplaty_miesieczne"].ToString()));
                dict.Add("wynagrodzenie", double.Parse(result["wynagrodzenie"].ToString()));
                dict.Add("pracownicy_min", double.Parse(result["pracownicy_min"].ToString()));
                dict.Add("pracownicy_opt", double.Parse(result["pracownicy_opt"].ToString()));
                dict.Add("pojemnosc_magazynu", double.Parse(result["pojemnosc_magazynu"].ToString()));
                dict.Add("klienci_max", double.Parse(result["klienci_max"].ToString()));
                connection.Close();
            }
            return dict;
        }
        public Dictionary<string, string> shopInfo()
        {
            Console.WriteLine("ShopInfo");
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string query = "select * from oSklepie";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                result.Read();
                dict.Add("Nazwa", result["Nazwa"].ToString());
                dict.Add("Data_zalozenia", result["Data_zalozenia"].ToString());
                connection.Close();
            }

            return dict;
        }
        public Dictionary<string, string> endInfo()
        {
            Console.WriteLine("EndInfo");
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string query = "select * from info";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                result.Read();
                dict.Add("dochod", result["Dochod_calkowity"].ToString());
                dict.Add("wydatki", result["Wydatki_calkowite"].ToString());
                dict.Add("liczba", result["Liczba_zamowien"].ToString());
                dict.Add("koszt", result["Koszt_zamowien"].ToString());
                dict.Add("lvl", result["Najwyzszy_poziom"].ToString());
                connection.Close();
            }

            return dict;
        }

        //liczenie iloœci produktów danej kategorii
        public Dictionary<int, int> catsCount()
        {
            Console.WriteLine("CatsCount");
            Dictionary<int, int> dict = new Dictionary<int, int>();

            string query = "select id_kat, count(distinct(id_prod)) as cnt from stan join produkty on Produkt = id_prod join kategorie on Kategoria = id_kat group by id_kat";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    dict.Add(int.Parse(result["id_kat"].ToString()), int.Parse(result["cnt"].ToString()));
                }
                connection.Close();
            }
            return dict;
        }

        //dodawanie produktów z zamówienia do stanu
        public void delivery()
        {

        }
    }
}
