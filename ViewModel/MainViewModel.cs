using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SklepexPOL.ViewModel
{
    using SklepexPOL;
    using BaseClass;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Model;
    using View;
    using R = Properties.Settings;
    class MainViewModel : BaseViewModel
    {
        //generator klientów i zakupów
        clientRandomizer randomizer = new clientRandomizer();
        moneyCounter moneyManager = new moneyCounter();
        stringConventer strConv = new stringConventer();
        interactions intercom = new interactions();

        //uruchamianie sie aplikacji
        private ICommand hello;
        public ICommand Hello
        {
            get
            {
                return hello ?? new RelayCommand(p => initialize(), null);
            }
        }
        private void initialize()
        {
            bool connection = false;
            //mysql - sprawdź czy jest połączenie
            //nie ma połączenia z sql
            if (!connection)
            {
                intercom.message("Nie można nawiązać połączenia z bazą danych!\nFunkcja gry zostaje dezaktywowana.", "Brak połączenia z bazą danych!");
                IsSQL = false;
                IsSavedGame = false;
            }
            //jest
            else
            {
                IsSQL = true;
                bool db = false;
                //mysql - sprawdź czy jest baza
                //jeśli nie ma
                if (!db) 
                {
                    IsSavedGame = false;
                    R.Default.IsGameSaved = false;
                    R.Default.SoldItemsString = "";
                    R.Default.Save();
                    //mysql - zaimportuj plik baza.sql
                }
                //jest
                else
                {
                    IsSavedGame = R.Default.IsGameSaved;
                    //jeśli jest zapis gry
                    if (R.Default.IsGameSaved) 
                    { 
                        //wczytaj dane z bazy i appOpen, oSklepie, dostawcy, naStanie, doDostarczenia                
                    }
                }
                
            }
            
        }

        //zamykanie sie aplikacji
        private ICommand goodBye;
        public ICommand GoodBye
        {
            get
            {
                return goodBye ?? new RelayCommand(p => saveProperties(),null);
            }
        }
        private void saveProperties()
        {
            if (R.Default.IsGameSaved)
            {
                R.Default.ClientsCountValue = TodayClientsValue;
                R.Default.TodayDate = TodayDate;
                R.Default.SoldItemsString = TSI();
                R.Default.Save();
            }
        }


        #region panel menu i gry
        //czy jest połączenie z sql
        private bool isSQL;
        public bool IsSQL
        {
            get { return isSQL; }
            set
            {
                isSQL = value;
                onPropertyChanged(nameof(IsSQL));
            }
        }
        //czy jest zapisana gra
        private bool isSavedGame;
        public bool IsSavedGame
        {
            get { return isSavedGame; }
            set
            {
                isSavedGame = value;
                onPropertyChanged(nameof(IsSavedGame));
            }
        }

        //Widoczność menu/gry
        private Visibility menuVis = Visibility.Visible;
        public Visibility MenuVis
        {
            get { return menuVis; }
            set
            {
                menuVis = value;
                onPropertyChanged(nameof(MenuVis));
            }
        }
        private Visibility gameVis = Visibility.Collapsed;
        public Visibility GameVis
        {
            get { return gameVis; }
            set
            {
                gameVis = value;
                onPropertyChanged(nameof(GameVis));
            }
        }
        private Visibility dateVis = Visibility.Collapsed;
        public Visibility DateVis
        {
            get { return dateVis; }
            set
            {
                dateVis = value;
                onPropertyChanged(nameof(DateVis));
            }
        }
        private Visibility nGVis = Visibility.Collapsed;
        public Visibility NGVis
        {
            get { return nGVis; }
            set
            {
                nGVis = value;
                onPropertyChanged(nameof(NGVis));
            }
        }
        //komendy zmiany widoczności
        //przejście do menu
        private ICommand windowMenu;
        public ICommand WindowMenu
        {
            get
            {
                return windowMenu ?? new RelayCommand(prop => menuWindow(),null);
            }
        }
        private void menuWindow()
        {
            MenuVis = Visibility.Visible;
            GameVis = Visibility.Collapsed;
        }
        //przejście do widoku gry
        private ICommand windowGame;
        public ICommand WindowGame
        {
            get
            {
                return windowGame ?? new RelayCommand(prop => gameWindowAsync(), null);
            }
        }
        
        private async Task gameWindowAsync()
        {
            //inicjacja spisu produktów na stanie itd. - z bazy
            double[] key = new double[] { 110, 5 };
            Dictionary<string, double[]> slownik = new Dictionary<string, double[]>();
            slownik.Add("pomidor", key);
            slownik.Add("ogórek", key);
            slownik.Add("Brokuł", key);
            OnHouseItems = slownik;
            //Wywołanie wyświetlacza słownego dnia tygodnia
            TDN();
            //wczytanie z zapisu gry, co ostatnio (dzisiaj) sprzedano
            TodaySoldItems = R.Default.SoldItemsString;
            //zmiana widoczności okien
            MenuVis = Visibility.Collapsed;
            DateVis = Visibility.Visible;
            //chwila odpoczynku
            await Task.Delay(3000);
            //zmiana widoczności okien
            dateGameSwitch();
        }

        //komendy otworzenia pliku z instrukcją
        private ICommand infoPdf;
        public ICommand InfoPdf
        {
            get
            {
                return infoPdf ?? new RelayCommand(prop => showInfo(), null);
            }
        }

        private void showInfo()
        {
            //utworzyć instrukcje i podpiąc jej plik
            System.Diagnostics.Process.Start("test.pdf");            
        }
        
        //zamykanie programu
        private ICommand exitGame;
        public ICommand ExitGame
        {
            get
            {
                return exitGame ?? new RelayCommand(prop => byeBye(), null);
            }
        }

        private void byeBye()
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region panel data
        //zmienna daty - zaktualizować z bazą
        private DateTime todayDate = DateTime.Today;
        public DateTime TodayDate
        {
            get { return todayDate; }
            set
            {
                todayDate = value;
                onPropertyChanged(nameof(todayDate));
            }
        }
        private string todayDateName;
        public string TodayDateName
        {
            get{ return todayDateName; }
            set
            {
                todayDateName = value;
                onPropertyChanged(nameof(TodayDateName));
            }
        }
        //słowna reprezentacja dnia tygodnia
        public void TDN()
        {
            switch ((int)TodayDate.DayOfWeek)
            {
                case 0:
                    TodayDateName = "Niedziela";
                    break;
                case 1:
                    TodayDateName = "Poniedziałek";
                    break;
                case 2:
                    TodayDateName = "Wtorek";
                    break;
                case 3:
                    TodayDateName = "Środa";
                    break;
                case 4:
                    TodayDateName = "Czwartek";
                    break;
                case 5:
                    TodayDateName = "Piątek";
                    break;
                case 6:
                    TodayDateName = "Sobota";
                    break;
            }
        }
        //zwiększenie daty o 1
        private ICommand dateUp;
        public ICommand DateUp
        {
            get
            {
                return dateUp ?? new RelayCommand(p => dayChangeAsync(), null);
            }
        }
        //funkcja przechodzenia do następnego dnia
        private async Task dayChangeAsync()
        {
            //zwiększenie daty o 1
            TodayDate = TodayDate.AddDays(1);
            //mysql => NextDay()
            //mysql Update info set Ruch = TodayClientsValue
            //słowny zapis dnia
            TDN();
            MediaPlayer player = new MediaPlayer();
            //muzyczka
            //zmienić na to że jak będzie dostawa to ma grać
            if (TodayDate.Day == 13)
                player.Open(new Uri(@"../../sounds/delivery.mp3", UriKind.Relative));
            else
            {
                string[] sounds = { "cash.mp3","bell.mp3","beep.mp3"};
                Random random = new Random();
                int index = random.Next(0, sounds.Length);
                player.Open(new Uri(@"../../sounds/"+sounds[index], UriKind.Relative));
            }
            player.Play();
            //przejście z widoku gry na widok daty
            gameDateSwitch();
            //generowanie klientów i ich zakupów
            TodayClientsValue = randomizer.clientsCreator(TodayClientsValue, ShopMargin, ShopState, ShopLevel, TodayDate);
            SoldItems = randomizer.shopListGenerator(TodayClientsValue, OnHouseItems, ShopLevel, TodayDate);
            R.Default.SoldItemsString = TSI();
            R.Default.Save();
            //chwila odpoczynku
            await Task.Delay(3000);
            //przejście z daty do gry
            dateGameSwitch();
        }
        //przejście z ekranu daty do ekranu gry
        public void dateGameSwitch()
        {
            DateVis = Visibility.Collapsed;
            GameVis = Visibility.Visible;
        }
        //przejście z ekranu gry do ekranu daty
        public void gameDateSwitch()
        {
            DateVis = Visibility.Visible;
            GameVis = Visibility.Collapsed;
        }
        #endregion

        #region tabsy - zakładki w grze
        //zmienna aktualnie otwartej zakładki (domyślnie raport)
        private UserControl actualTab = new View.raport();
        public UserControl ActualTab
        {
            get { return actualTab; }
            set
            {
                actualTab = value;
                onPropertyChanged(nameof(ActualTab));
            }
        }

        private ICommand changeTab;
        public ICommand ChangeTab
        {
            get
            {
                return changeTab ?? new RelayCommand(p => switchTab(p), null);
            }
        }
        //zmiana tabsów
        private void switchTab(object p)
        {
            int param = int.Parse(p.ToString());
            switch (param)
            {
                case 0:
                    ActualTab = new View.raport();
                    break;
                case 1:
                    ActualTab = new View.stan();
                    break;
                case 2:
                    ActualTab = new View.zamowienia();
                    break;
                case 3:
                    ActualTab = new View.nowe();
                    break;
                case 4:
                    ActualTab = new View.sklep();
                    break;
            }

        }
        #endregion

        #region nowe zamówienie
        //podatek produktu (wczytać z bazy)
        private double proPod = 0.23;
        public double ProPod
        {
            get { return proPod; }
            set
            {
                proPod = value;
                onPropertyChanged(nameof(ProPod));
            }
        }
        #endregion
        
        #region zamiana numerycznych procentów na stringi
        //procent podatku produktu
        private string proPodPer;
        public string ProPodPer
        {
            get
            {
                proPodPer = (ProPod * 100).ToString() + " %";
                return proPodPer;
            }
        }
        #endregion

        #region informacje o sklepie

        //nazwa sklepu
        private string shopName;
        public string ShopName
        {
            get { return shopName; }
            set
            {
                shopName = value;
                onPropertyChanged(nameof(ShopName));
            }
        }
        //data otwarcia
        private DateTime openDate;
        public DateTime OpenDate
        {
            get { return openDate; }
            set
            {
                openDate = value;
                onPropertyChanged(nameof(OpenDate));
            }
        }
        //data otwarcia jako string
        public string OpenDateString
        {
            get { return ODS(); }
        }
        private string ODS()
        {
            return OpenDate.ToString("dd/MM/yyyy") +" ("+(TodayDate - OpenDate).TotalDays.ToString()+"dni)";
        }
        //co jest na stanie - wczytać z bazy
        //{"produkt #id_zam":[ilosc, cena, wysokość podatku, marża dostawcy, termin ważności(ile dni zostało), id_zam]}
        private Dictionary<string, double[]> onHouseItems;
        public Dictionary<string, double[]> OnHouseItems
        {
            get { return onHouseItems; }
            set
            {
                onHouseItems = value;
                onPropertyChanged(nameof(OnHouseItems));
            }
        }

        //saldo
        private double moneyBalance;
        public double MoneyBalance
        {
            get { return moneyBalance; }
            set
            {
                moneyBalance = Math.Round(value, 2);
                onPropertyChanged(nameof(MoneyBalance));
            }
        }
        public string MoneyBalanceString
        {
            get { return strConv.money(MoneyBalance); }
        }
        //dochód
        private double moneyIncome;
        public double MoneyIncome
        {
            get { return moneyIncome; }
            set
            {
                moneyIncome = Math.Round(value, 2);
                onPropertyChanged(nameof(MoneyIncome));
            }
        }
        public string MoneyIncomeString
        {
            get { return strConv.money(MoneyIncome); }
        }
        //wydatki
        private double moneyExpense;
        public double MoneyExpense
        {
            get { return moneyExpense; }
            set
            {
                moneyExpense = Math.Round(value, 2);
                onPropertyChanged(nameof(MoneyExpense));
            }
        }
        public string MoneyExpenseString
        {
            get { return strConv.money(MoneyExpense); }
        }
        //zysk
        public string MoneyProfit
        {
            get { return strConv.money(MoneyIncome - MoneyExpense); }
        }
        //podsumowanie
        public string MoneySummary
        {
            get { return strConv.money(MoneyBalance + MoneyIncome - MoneyExpense); }
        }

        //ilość klientów - wczytać z bazy
        private int todayClientsValue = 10;
        public int TodayClientsValue
        {
            get { return todayClientsValue; }
            set
            {
                todayClientsValue = value;
                onPropertyChanged(nameof(TodayClientsValue));
            }
        }
        //wartość sprzedanych przedmiotów
        private double todaySellValue;
        public double TodaySellValue
        {
            get { return todaySellValue; }
            set
            {
                todaySellValue = value;
                onPropertyChanged(nameof(TodaySellValue));
            }
        }
        //lista sprzedanych przedmiotów
        private Dictionary<string, double[]> soldItems;
        public Dictionary<string, double[]> SoldItems
        {
            get { return soldItems; }
            set
            {
                soldItems = value;
                onPropertyChanged(nameof(SoldItems));
            }
        }
        //string sprzedanych przedmiotów
        private string todaySoldItems;
        public string TodaySoldItems
        {
            get { return todaySoldItems; }
            set
            {
                todaySoldItems = value;
                onPropertyChanged(nameof(TodaySoldItems));
            }
        }
        //tworzenie listy sprzedanych przedmiotów
        private string TSI()
        {
            string t = "";
            if (SoldItems != null)
            {
                foreach (KeyValuePair<string, double[]> key in SoldItems)
                    t += key.Key + " : " + key.Value[0] + "\n";
            }
            TodaySoldItems = t; 
            return t;          
        }
        //poziom sklepu
        private int shopLevel = 1;
        public int ShopLevel
        {
            get { return shopLevel; }
            set
            {
                shopLevel = value;
                onPropertyChanged(nameof(ShopLevel));
            }
        }
        //poziom sklepu jako string
        public string ShopLevelStr
        {
            get { return SLS(); }
        }
        private string SLS()
        {
            if (ShopLevel == 1) return "(1) - Sklepik";
            else if (ShopLevel == 2) return "(2) - Sklep";
            else if (ShopLevel == 3) return "(3) - Dyskont";
            else if (ShopLevel == 4) return "(4) - Supermarket";
            else if (ShopLevel == 5) return "(5) - Hipermarket";
            else return "(0) - Pustostan";
        }

        //stan sklepu
        private int shopState = 2;
        public int ShopState
        {
            get { return shopState; }
            set
            {
                shopState = value;
                onPropertyChanged(nameof(ShopState));
            }
        }
        //stan sklepu jako string
        public string ShopStateStr
        {
            get { return SSS(); }
        }
        private string SSS()
        {
            if (ShopLevel == 0) return "Zły";
            else if (ShopLevel == 1) return "Średni";
            else if (ShopLevel == 2) return "Dobry";
            else return "Świetny";
        }

        //liczba pracowników
        private int shopEmployees;
        public int ShopEmployees
        {
            get { return shopEmployees; }
            set
            {
                shopEmployees = value;
                onPropertyChanged(nameof(ShopEmployees));
            }
        }
        //wynagrodzenie pracowników
        private double employeesSalary;
        public double EmployeesSalary
        {
            get { return employeesSalary; }
            set
            {
                employeesSalary = value;
                onPropertyChanged(nameof(EmployeesSalary));
            }
        }
        public string EmployeeString
        {
            get { return strConv.money(EmployeesSalary); } 
        }
        public string SalaryString
        {
            get { return strConv.money(EmployeesSalary * ShopEmployees); }
        }
        //marża sklepu
        private double shopMargin = 0.65;
        public double ShopMargin
        {
            get { return shopMargin; }
            set
            {
                shopMargin = value;
                onPropertyChanged(nameof(ShopMargin));
            }
        }
        //marża jako string
        public string MarginString
        {
            get { return strConv.percentage(ShopMargin); }
        }
        //rodzaj sklepu
        private int shopType = 10;
        public int ShopType
        {
            get { return shopType; }
            set
            {
                shopType = value;
                onPropertyChanged(nameof(ShopType));
            }
        }
        //rodzaj sklepu jako string
        public string ShopTypeString
        {
            get { return STS(); }
        }
        private string STS()
        {
            if (ShopType == 1) return "Spożywczy";
            else if (ShopType == 2) return "Warzywniak";
            else if (ShopType == 3) return "RTV AGD";
            else if (ShopType == 4) return "Mięsny";
            else if (ShopType == 5) return "Drogeria";
            else if (ShopType == 6) return "Butik";
            else if (ShopType == 7) return "Papierniczy";
            else if (ShopType == 8) return "Piekarnia";
            else if (ShopType == 9) return "Ogrodniczy";
            else if (ShopType == 11) return "Monopolowy";
            else return "Market";
        }
        //pojemność magazynu
        private int storageSize;
        public int StorageSize
        {
            get { return storageSize; }
            set
            {
                storageSize = value;
                onPropertyChanged(nameof(StorageSize));
            }
        }
        //ilość produktów w magazynie
        private int storageValue;
        public int StorageValue
        {
            get { return storageValue; }
            set
            {
                storageValue = value;
                onPropertyChanged(nameof(StorageValue));
            }
        }
        //string zapełnienia magazynu
        public string StorageSpace
        {
            get { return StorageValue.ToString() + "/" + StorageSize.ToString(); }
        }
        //wysokość opłat miesięcznych
        private double rentValue;
        public double RentValue
        {
            get { return rentValue; }
            set
            {
                rentValue = value;
                onPropertyChanged(nameof(RentValue));
            }
        }
        public string RentValueString
        {
            get { return strConv.money(RentValue); }
        }

        //widoczność slidera zmiany marży
        private Visibility isMarginSliderVisible = Visibility.Collapsed;
        public Visibility IsMarginSliderVisible
        {
            get { return isMarginSliderVisible; }
            set
            {
                isMarginSliderVisible = value;
                onPropertyChanged(nameof(IsMarginSliderVisible));
            }
        }

        //zmiana marży
        private ICommand marginChange;
        public ICommand MarginChange
        {
            get
            {
                return marginChange ?? new RelayCommand(p => { 
                    ShopMargin = (double)p;
                    onPropertyChanged(nameof(MarginString));
                    IsMarginSliderVisible = Visibility.Collapsed;
                }, null);
            }
        }
        private ICommand marginEnabler;
        public ICommand MarginEnabler
        {
            get
            {
                return marginEnabler ?? new RelayCommand(p => IsMarginSliderVisible = Visibility.Visible, null);
            }
        }

        //zatrudnienie i zwolnienie pracownika
        private ICommand hire;
        public ICommand Hire
        {
            get
            {
                return hire ?? new RelayCommand(p => { 
                    ShopEmployees += 1;
                    //sql update info set liczba_pracownikow = liczba_pracownikow + 1;
                    shopInfoUpdate();
                }, null);
            }
        }
        private ICommand fire;
        public ICommand Fire
        {
            get
            {
                return fire ?? new RelayCommand(p => {
                    ShopEmployees -= 1;
                    //sql update info set liczba_pracownikow = liczba_pracownikow - 1;
                    shopInfoUpdate();
                }, arg => ShopEmployees > 0);
            }
        }

        public void shopInfoUpdate()
        {
            //określenie rodzaju, stanu i poziomu
        }
        private ICommand infoBox;
        public ICommand InfoBox
        {
            get
            {
                return infoBox ?? new RelayCommand(p => intercom.infoBox((string)p), null);
            }
        }
        private bool isLvlUp = false;
        public bool IsLvlUp
        {
            get { return isLvlUp; }
            set
            {
                isLvlUp = value;
                onPropertyChanged(nameof(IsLvlUp));
            }
        }
        private ICommand levelUp;
        public ICommand LevelUp 
        {
            get
            {
                return levelUp ?? new RelayCommand(p => 
                {
                    //lewelowanie
                    //sql zwiększ level
                    //wczytaj dane dla levelu
                    ShopLevel += 1;
                }, null);
            }
        }
        #endregion

        #region nowa gra
        private ICommand newGameView;
        public ICommand NewGameView
        {
            get
            {
                return newGameView ?? new RelayCommand(p => gameScreen(), null);
            }
        }
        private void gameScreen()
        { 
            NGVis = Visibility.Visible;
            MenuVis = Visibility.Collapsed;    
        }

        private ICommand cancelNG;
        public ICommand CancelNG
        {
            get
            {
                return cancelNG ?? new RelayCommand(p=>NGCancel(),null);
            }
        }
        private void NGCancel()
        {
           MenuVis = Visibility.Visible;
           NGVis = Visibility.Collapsed;
        }

        private ICommand nG;
        public ICommand NG
        {
            get
            {
                return nG ?? new RelayCommand(p => NowaGra(), arg => ShopNameInpt != null && ShopNameInpt != "");
            }
        }
        private void NowaGra()
        {
            bool confirm = false;
            //jeśli jest zapis
            if (R.Default.IsGameSaved)
                if (intercom.YesOrNo("Wykryto zapisaną grę.\nCzy na pewno chcesz utworzyć nowy zapis gry?\nPoprzedni zapis zostanie usunięty bezpowrotnie!", "Czy chcesz utworzyć nową grę?"))
                    confirm = true;

            //jeśli nie ma zapisu lub jest i jest zgoda na nadpisanie
            if (!R.Default.IsGameSaved || confirm)
            {
                //sql theblip
                //mysql => select @@sql_mode; -> do zmiennej
                //mysql => SET SESSION sql_mode = 'NO_AUTO_VALUE_ON_ZERO';
                //mysql => Insert Into zamowienia Values(0,CURDATE(),CURDATE(),0,0);
                //mysql => SET SESSION sql_mode <zmienna> 
                
                //Console.WriteLine(ShopNameInpt);
                //Console.WriteLine(SelectedShopType);

                //typ sklepu
                switch (SelectedShopType)
                {
                    //spożywczy
                    case 0:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //warzywniak
                    case 1:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //RTV AGD
                    case 2:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //mięsny
                    case 3:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //drogeria
                    case 4:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //butik
                    case 5:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //papierniczy
                    case 6:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //piekarnia
                    case 7:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //ogrodniczy
                    case 8:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //market
                    case 9:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                    //monopolowy
                    case 10:
                        //sql insert into stan (Ilosc,Produkt,Zamowienie) values (500,,0), (500,,0), (500,,0), (500,,0), (500,,0);
                        break;
                }
                string query = "Insert Into info Values ('"+ 
                    ShopNameInpt.Replace('"', '\"').Replace("'", "\'") +
                    "',CURDATE(),2000,CURDATE(),0.4,0,0,0,0,0,0,0,10,"+(SelectedShopType+1)+",1,1);";
                //mysql => query
                //mysql => Update stan Set Liczba_zamowien = 0;

                R.Default.ClientsCountValue = 10;
                R.Default.TodayDate = DateTime.Now;
                R.Default.SoldItemsString = "";
                R.Default.IsGameSaved = true;
                R.Default.Save();


            }
        }
        private string shopNameInpt;
        public string ShopNameInpt
        {
            get
            {
                return shopNameInpt;
            }
            set
            {
                shopNameInpt = value;
                onPropertyChanged(nameof(ShopNameInpt));
            }
        }

        private int selectedShopType = 0;
        public int SelectedShopType
        {
            get { return selectedShopType; }
            set
            {
                selectedShopType = value;
                onPropertyChanged(nameof(SelectedShopType));
            }
        }
        #endregion

        #region zamknięcie sklepu/przegrana

        private ICommand sepuku;
        public ICommand Sepuku
        {
            get
            {
                return sepuku ?? new RelayCommand(p=>shopClose(),null);
            }
        }
        private void shopClose()
        {
            if(intercom.YesOrNo("Czy na pewno chcesz zamknąć swój sklep?\nTego działania nie można odwrócić!","Czy chcesz zamknąć sklep?"))
            {
                Console.WriteLine("zamykamy");
                //sql ;
            }
        }

        #endregion
    }
}
