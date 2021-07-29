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
        private MySqlConnection connection;

        MySqlConnectionStringBuilder connect = new MySqlConnectionStringBuilder();

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

        private DBConnection()
        {
            connect.UserID = Properties.Settings.Default.Rlogin;
            connect.Server = Properties.Settings.Default.server;
            connect.Port = Properties.Settings.Default.port;
            connect.Password = Properties.Settings.Default.Rpasswd;
            connect.AllowUserVariables = true;
            connect.AllowBatch = true;
            connect.Database = "sklepexPOL";
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
                MySqlCommand command = new MySqlCommand("Select * from info", connection);
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
                    string query = "call oferta(" + item.ID + ")";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader result = command.ExecuteReader();

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
                        string subquery = "select sum(Ilosc) from pro_zam where Zamowienie = " + result["ID_zam"].ToString();
                        int val = integer(subquery);
                        dict.Add(new structs.zam(
                            int.Parse(result["ID_zam"].ToString()),
                            result["Nazwa"].ToString(),
                            DateTime.Parse(result["Data_zamowienia"].ToString()),
                            DateTime.Parse(result["Data_dostarczenia"].ToString()),
                            double.Parse(result["Koszt"].ToString()),
                            val));
                        
                    }
                }
                connection.Close();
            }
            Console.WriteLine("end - orders");
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
        public string delivery(DateTime Today)
        {
            string message = "";
            Console.WriteLine("delivery");
            List<string> usedIDS = new List<string>();
            string query = "Select ID_zam, Produkt, Ilosc, m.Nazwa from zamowienia " +
                "join pro_zam on zamowienia.ID_zam = pro_zam.Zamowienie JOIN magazyny m ON zamowienia.Magazyn = m.ID_mag " +
                "Where ID_zam > 0 AND Data_dostarczenia = '" + Today.ToString("yyyy-MM-dd") + "'";
            using (connection = new MySqlConnection(connect.ToString()))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    if(!usedIDS.Contains(result["ID_zam"].ToString()))
                        message += "Zamówienie #" + result["ID_zam"].ToString() + " - " + result["Nazwa"].ToString() + "\n";
                    usedIDS.Add(result["ID_zam"].ToString());
                    Console.WriteLine("del - foreach");
                    string subquery = "Insert into stan Values (NULL, " +
                        result["Ilosc"].ToString() + ", " +
                        result["Produkt"].ToString() + ", " +
                        result["ID_zam"].ToString() + ")";
                    execute(subquery);
                }
                connection.Close();
            }
            return message;
        }
    }
}
