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
    using System.Windows.Data;
    using System.Collections;
    using System.Collections.ObjectModel;
    using DAL;

    class MainViewModel : BaseViewModel
    {
        //generator klientów i zakupów
        clientRandomizer randomizer = new clientRandomizer();
        //kalkulator
        moneyCounter moneyManager = new moneyCounter();
        //konwertor na stringi
        stringConventer strConv = new stringConventer();
        //messageboxy
        interactions intercom = new interactions();
        //struktury
        structs structs = new structs();
        //połączenie z bazą
        DBConnection mysql;

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
            //jeśli użytkownik nie podał dostępu do mysql
            if(R.Default.Rpasswd == "" && R.Default.Rlogin == "")
            {
                dialogShow();
            }
            //inicjacja mysql
            mysql = DBConnection.Instance;

            //mysql - sprawdź czy jest połączenie
            bool connection = mysql.IsConnection();
            
            //nie ma połączenia z sql
            if (!connection)
            {
                if (intercom.YesOrNo("Nie można nawiązać połączenia z bazą danych!\n" +
                    "Czy podane dane są poprawne?\n" +
                    "Login: " + R.Default.Rlogin +"\n" +
                    "Hasło: " + R.Default.Rpasswd, "Błąd połączenia z bazą danych!")) 
                {
                    intercom.message("Nie można nawiązać połączenia z bazą danych!\n" +
                        "Funkcja gry zostaje dezaktywowana.", "Brak połączenia z bazą danych!");
                    IsSQL = false;
                    IsSavedGame = false;
                }
                else
                {
                    dialogShow().Wait();
                }
                
            }
            //jest
            else
            {
                IsSQL = true;

                //mysql - sprawdź czy jest baza
                bool db = mysql.IsDBConnection();
                
                //jeśli nie ma
                if (!db) 
                {
                    IsSavedGame = false;
                    R.Default.IsGameSaved = false;
                    R.Default.SoldItemsString = "";
                    R.Default.Save();
                    //mysql - zaimportuj plik baza.sql
                    if (!mysql.DBImport())
                    {
                        intercom.message("Wykryto brak pewnych plików!\n" +
                            "Program przerywa działanie.", "Brakujące pliki!");
                        Environment.Exit(0);
                    }
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
        private async Task dialogShow()
        {
            var dialog = new dialog();
            if (dialog.ShowDialog() == true)
            {
                if (intercom.YesOrNo("Czy podane dane są poprawne?\n" +
                    "Login: " + dialog.ResponseLogin +
                    "\nHasło: " + dialog.ResponsePassword, "Potwierdzenie danych"))
                {
                    R.Default.Rpasswd = dialog.ResponsePassword;
                    R.Default.Rlogin = dialog.ResponseLogin;
                    R.Default.Save();
                }
                else
                {
                    dialogShow();
                }
            }
            else
            {
                Environment.Exit(0);
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

        #region funkcje przejścia do następnego dnia

        //funkcja przechodzenia do następnego dnia
        private async Task dayChangeAsync()
        {
            //zwiększenie daty o 1
            TodayDate = TodayDate.AddDays(1);
            //mysql => NextDay()
            mysql.execute("call NextDay()");
            //mysql Update info set Ruch = TodayClientsValue
            mysql.execute("Update info set Ruch = "+TodayClientsValue.ToString());

            //zmiany w pieniądzach
            MoneyBalance = MoneyBalance + MoneyIncome - MoneyExpense;

            //sprawdzenie czy jest dzisiaj dostawa
            //IsDeliveryDay

            //słowny zapis dnia
            TDN();

            //muzyczka
            //zmienić na to że jak będzie dostawa to ma grać
            MediaPlayer player = new MediaPlayer();
            if (IsDeliveryDay)
                player.Open(new Uri(@"../../sounds/delivery.mp3", UriKind.Relative));
            else if (MoneyBalance <= -5000)
                player.Open(new Uri(@"../../sounds/trombone.mp3", UriKind.Relative));
            else
            {
                string[] sounds = { "cash.mp3", "bell.mp3", "beep.mp3" };
                Random random = new Random();
                int index = random.Next(0, sounds.Length);
                player.Open(new Uri(@"../../sounds/" + sounds[index], UriKind.Relative));
            }
            player.Play();

            //przejście z widoku gry na widok daty
            gameDateSwitch();
            
            //mysql => pobranie ilości towarów na stanie

            //generowanie klientów i ich zakupów
            TodayClientsValue = randomizer.clientsCreator(TodayClientsValue, ShopMargin, ShopState, ShopLevel, TodayDate);
            SoldItems = randomizer.shopListGenerator(TodayClientsValue, OnHouseItems, ShopLevel, TodayDate);
            //mysql => odjęcie sprzedanych towarów z bazy
            R.Default.ClientsCountValue = TodayClientsValue;
            R.Default.SoldItemsString = TSI();
            R.Default.Save();

            //wczytanie z bazy liczby towarów na stanie
            //OnHouseItems = ...

            //generowanie listview'ów
            OnHouseToListView(OnHouseItems);
            DeliveriesToListView();

            //obliczenie dochodów ze sprzedaży - odblokować
            //MoneyIncome = moneyManager.stonksCalc(SoldItems, ShopMargin);
            //mysql => update info set Dochod_dzienny = MoneyIncome

            //obliczenie wydatków
            if (TodayDate.Day == 1)
            {
                IsPaymentDay = true;
                MoneyExpense = EmployeesSalary * ShopEmployees + RentValue;
            }
            else
            {
                IsPaymentDay = false;
                MoneyExpense = EmployeesSalary * ShopEmployees;
            }
            //mysql => update info set Wydatki_dzienne = MoneyExpense
            mysql.execute("update info set Wydatki_dzienne = "+ MoneyExpense);

            //sprawdzenie stanu, rodzaju i poziomu sklepu
            int oldType = ShopType;
            shopInfoUpdate();
            if(ShopType != oldType)
            {
                //mysql - zmień typ sklepu
                mysql.execute("update info set Rodzaj = " + ShopType);
            }

            //sprawdzenie czy sklep nie powinien mieć obniżonego poziomu
            if (R.Default.LvlDays == 7)
            {
                if (R.Default.warning)
                {
                    intercom.message("Twój sklep się stoczył.\n" +
                        "Tu kończy się historia twojego sklepu.\n" +
                        "R. I. P. " + ShopName + "\n" + 
                        OpenDate.ToString("dd/MM/yyyy") + " - " + TodayDate.ToString("dd/MM/yyyy"), 
                        "R. I. P.");
                    GameOver();
                }
                else
                {
                    intercom.message("Twój sklep przez 7 dni nie spełniał wymogów swojego poziomu.\n" +
                        "Jego poziom zostaje obniżony.\n\n" +
                        "Jeśli ta sytuacja powtórzy się, twój sklep zostanie zamknięty",
                        "Regresja sklepu");
                    ShopLevel -= 1;
                    R.Default.LvlDays = 0;
                    R.Default.warning = true;

                }

                //mysql zmniejsz wartość lvl
                mysql.execute("update info set Poziom = " + ShopLevel);
            }

            //info o funduszach
            if (MoneyBalance < -1000)
            {
                IsAlert = true;
                AlertText = "Twój sklep przynosi straty. Jeśli jego sytuacja nie poprawi się, zostanie on zamknięty.";
            }
            else
            {
                IsAlert = false;
            }
            //info czy można levelować
            if (IsLvlUp && MoneyBalance > 0)
            {
                IsAlert = true;
                AlertText = "Istnieje możliwość zwiększenia poziomu sklepu.";
            }
            else
            {
                IsAlert = false;
            }

            //chwila odpoczynku
            await Task.Delay(3000);

            //przejście z daty do gry albo i nie

            if (MoneyBalance <= -5000)
            {
                intercom.message("Komornik przesyła pozdrowienia.\nTu kończy się historia twojego sklepu.\n" +
                    "R. I. P. " + ShopName + "\n" + OpenDate.ToString("dd/MM/yyyy") + " - " + TodayDate.ToString("dd/MM/yyyy"), "R. I. P.");
                GameOver();
            }
            else
            {
                dateGameSwitch();
            }
        }

        //przejście do gry z przycisku kontynuuj/po utworzeniu nowej gry
        //zrobić na tej samej zasadzie co tą wyżej (żeby wczytywała to samo)
        private async Task gameWindowAsync()
        {
            //pobranie daty
            TodayDate = mysql.Today();

            //pobranie infa o sklepie z bazy
            appOpen();

            //inicjacja spisu produktów na stanie z bazy
            OnHouseItems = mysql.onHouse(TodayDate);

            int storage = 0;
            foreach(KeyValuePair<string, double[]> item in OnHouseItems)
            {
                storage += (int)item.Value[0];
            }

            StorageValue = storage;

            CategoriesCounter = mysql.catsCount();

            //Wywołanie wyświetlacza słownego dnia tygodnia
            TDN();

            //wczytanie z zapisu gry, co ostatnio (dzisiaj) sprzedano
            TodaySoldItems = R.Default.SoldItemsString;

            //zmiana widoczności okien
            MenuVis = Visibility.Collapsed;
            DateVis = Visibility.Visible;

            //wywołanie inicjacji listview'ów
            OnHouseToListView(OnHouseItems);
            DeliveriesToListView();

            //wczytanie listy sprzedawców i ich produktów

            List<structs.dostawcy> dostawcy = mysql.Sellers();
            foreach (structs.dostawcy item in dostawcy)
                ListOfSellers.Add(item);

            List<List<structs.produkty>> produkty = mysql.SellersOffer(dostawcy);
            foreach (List<structs.produkty> item in produkty)
            {
                structs.produktyHandler listOfProducts = new structs.produktyHandler();
                foreach(structs.produkty produkt in item)
                {
                    listOfProducts.Add(produkt);
                }
                ListOfListOfProducts.Add(listOfProducts);
            }

            shopInfoUpdate();

            //chwila odpoczynku
            await Task.Delay(3000);

            //zmiana widoczności okien
            dateGameSwitch();
        }

        #endregion

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
        private Visibility gOVis = Visibility.Collapsed;
        public Visibility GOVis
        {
            get { return gOVis; }
            set
            {
                gOVis = value;
                onPropertyChanged(nameof(GOVis));
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

        //tekst alertu w raporcie
        private string alertText;
        public string AlertText
        {
            get { return alertText; }
            set
            {
                alertText = value;
                onPropertyChanged(nameof(AlertText));
            }
        }
        #endregion

        #region panel data

        //zmienna daty - zaktualizować z bazą
        private DateTime todayDate;
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

        //zmiana tabsów
        private ICommand changeTab;
        public ICommand ChangeTab
        {
            get
            {
                return changeTab ?? new RelayCommand(p => switchTab(p), null);
            }
        }
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

        //lista dostawców
        private structs.dostawcyHandler ListOfSellers = new structs.dostawcyHandler();
        public ObservableCollection<structs.dostawcy> Sellers
        {
            get { return ListOfSellers.Items; }
        }
        //index wybranego dostawcy (domyślnie żaden)
        private int lOSIndex = -1;
        public int LOSIndex
        {
            get { return lOSIndex; }
            set
            {
                lOSIndex = value;
                onPropertyChanged(nameof(LOSIndex));
                //wczytywanie listy produktów danego sprzedawcy
                if (value >= 0)
                {
                    Console.WriteLine(value);
                    ListOfProducts = null;
                    ListOfProducts = ListOfListOfProducts.Item(value);
                    SelectedProductIndex = -1;
                    IsProductsEnabled = true;
                }
                else
                {
                    ListOfProducts = null;
                    IsProductsEnabled = false;
                }
                onPropertyChanged(nameof(ListOfProducts));
            }
        }
        //wybrany dostawca
        private structs.dostawcy selectedSeller;
        public structs.dostawcy SelectedSeller
        {
            get { return selectedSeller; }
            set
            {
                Console.WriteLine(value);
                selectedSeller = value;
                onPropertyChanged(nameof(SelectedSeller));
                onPropertyChanged(nameof(LOSIndex));
                Console.WriteLine("AAAA");
            }
        }
        
        //lista produktów
        private structs.produktyHandler ListOfProducts;
        public ObservableCollection<structs.produkty> Products
        {
            get { return ListOfProducts.Items; }
        }
        //lista list produktów
        private structs.produktyHandlerList ListOfListOfProducts = new structs.produktyHandlerList();
        //wybrany produkt
        private structs.produkty selectedProduct;
        public structs.produkty SelectedProduct
        {
            get { return selectedProduct; }
            set{
                selectedProduct = value;
                onPropertyChanged(nameof(SelectedProduct));
            }
        }

        //czy combobox jest odblokowany
        private bool isProductsEnabled;
        public bool IsProductsEnabled
        {
            get { return isProductsEnabled; }
            set
            {
                isProductsEnabled = value;
                onPropertyChanged(nameof(IsProductsEnabled));
            }
        }

        //indeks wybranego produktu
        private int selectedProductIndex = -1;
        public int SelectedProductIndex
        {
            get { return selectedProductIndex; }
            set
            {
                selectedProductIndex = value;
                onPropertyChanged(nameof(SelectedProductIndex));
                if (value >= 0)
                {
                    SelectedProduct = ListOfProducts.Item(value);
                }
                else
                {
                    SelectedProduct = default(structs.produkty);
                }
                onPropertyChanged(nameof(SelectedProduct));
            }
        }

        //czy można zmienić dostawce - zależy od tego czy w liście jest jakaś pozycja czy nie
        public bool DeliveryEnabled
        {
            get 
            {
                if (cartList.Count() == 0) return true;
                else return false;
            }

        }

        //matematyka
        //ilość produktu
        private string productCount;
        public int ProductCount
        {
            get {
                if (productCount != null && productCount != "") return int.Parse(productCount);
                else return 0;
            }
        }
        public string ProductCountS
        {
            get { return productCount; }
            set
            {
                productCount = value;
                onPropertyChanged(nameof(ProductCount));
                onPropertyChanged(nameof(ProductCountS));
                if (productCount != null)
                {
                    onPropertyChanged(nameof(ProductCost));
                    onPropertyChanged(nameof(ProductTaxes));
                    onPropertyChanged(nameof(ProductSum));
                }
            }
        }

        //cena produktów
        public string ProductCost
        {
            //netto = ilosć x cena 
            get { return strConv.money(ProductCostV); }
        } 
        public double ProductCostV
        {
            //netto = ilosć x cena 
            get { return Math.Round(ProductCount * SelectedProduct.PriceV, 2); }
        } 
        
        //kwota podatków
        public string ProductTaxes
        {
            //tara = ilosć x cena x ((1+marża ( 1+ cło + pod) - 1))
            get { return strConv.money(ProductTaxesV); }
        }
        public double ProductTaxesV
        {
            //tara = ilosć x cena x ((1+marża ( 1+ cło + pod) - 1))
            get { return Math.Round(ProductCount * SelectedProduct.PriceV * ((1 + SelectedSeller.MarginV) * (1 + (SelectedSeller.TaxValue) + (SelectedProduct.TaxValue)) - 1), 2); }
        }

        //suma
        public string ProductSum
        {
            //brutto = ilosć x cena x (1+marża ( 1+ 1+cło + 1+pod))
            get { return strConv.money(ProductSumV); }
        }
        public double ProductSumV
        {
            //brutto = ilosć x cena x (1+marża ( 1+ 1+cło + 1+pod))
            get { return Math.Round(ProductCount * SelectedProduct.PriceV * ((1 + SelectedSeller.MarginV) * (1 + (SelectedSeller.TaxValue) + (SelectedProduct.TaxValue))), 2); }
        }

        //koszt zamówienia
        public string OrderCost
        {
            get { return strConv.money(OrderCostV); }
        }
        public double OrderCostV
        {
            get { return Math.Round(cartList.CostSum(), 2); }
        }

        //pozycje zamówienia
        private structs.noweHandler cartList = new structs.noweHandler();
        public ObservableCollection<structs.nowe> ShoppingCart
        {
            get { return cartList.Items; }
        }

        //dodawanie pozycji zamówienia
        private ICommand orderAddPosition;
        public ICommand OrderAddPosition
        {
            get
            {
                return orderAddPosition ?? new RelayCommand(p => 
                {   
                    var item = SelectedProduct;
                    double price = Math.Round(ProductCount * item.PriceV * ((1 + SelectedSeller.MarginV) * (1 + (SelectedSeller.TaxValue) + (item.TaxValue))), 2);
                    cartList.Add(new structs.nowe(item.ID, item.Name, ProductCount, price, OrderRemovePosition));
                    onPropertyChanged(nameof(cartList));
                    onPropertyChanged(nameof(ShoppingCart));
                    onPropertyChanged(nameof(DeliveryEnabled));
                    ProductCountS = "";
                }, arg => 
                //wpisano liczbe większą niż 0 w ilości
                ProductCountS != null &&
                ProductCountS != "" &&
                ProductCount > 0 &&
                //wybrano dostawce
                LOSIndex >= 0 &&
                //wybrano produkt
                SelectedProductIndex >= 0 &&
                //produkt nie jest już dodany do listy
                cartList.ItemExist(SelectedProduct.ID) == false
                );
            }
        }

        //usuwanie pozycji zamówienia
        private ICommand orderRemovePosition;
        public ICommand OrderRemovePosition
        {
            get
            {
                return orderRemovePosition ?? new RelayCommand(p => 
                {
                    cartList.Remove((int)p);
                    onPropertyChanged(nameof(cartList));
                    onPropertyChanged(nameof(ShoppingCart));
                    onPropertyChanged(nameof(DeliveryEnabled));
                },null);
            }
        }

        //usuwanie wszystkich pozycji w zamówieniu
        private ICommand orderClear;
        public ICommand OrderClear
        {
            get
            {
                return orderClear ?? new RelayCommand(p =>
                {
                    cartList.Clear();
                    onPropertyChanged(nameof(cartList));
                    onPropertyChanged(nameof(cartList));
                    onPropertyChanged(nameof(ShoppingCart));
                    onPropertyChanged(nameof(DeliveryEnabled));

                }, arg => cartList.Items.Count > 0);
            }
        }

        //składanie zamówienia
        private ICommand orderSubmit;
        public ICommand OrderSubmit
        {
            get
            {
                return orderSubmit ?? new RelayCommand(p => checkOrder(), arg => cartList.Items.Count > 0);
            }
        }

        private void checkOrder()
        {
            if (OrderCostV > MoneyBalance)
            {
                if (intercom.YesOrNo("Masz zbyt mało pieniędzy na koncie, żeby złożyć to zamówienie.\n" +
                    "Czy chcesz je złożyć mimo to?\n" +
                    "(Zbyt wielki ujemny stan konta (dług) może doprowadzić to zamknięcia twojego sklepu!)",
                    "Za mało pieniędzy na koncie!"))
                {
                    SubmitOrder();
                }
            }
            else if (cartList.SpaceSum() > (StorageSize - (ReservedStorageSpace + StorageValue)))
            {
                if (intercom.YesOrNo("Masz zbyt mało miejsca w magazynie, żeby złożyć to zamówienie.\n" +
                    "Czy chcesz je złożyć mimo to?\n" +
                    "(Do czasu przyjazdu zamówienia może zwolnić się nieco miejsca, jednak w przypadku gdy tego miejsca zabraknie - towar zostanie zniszczony!)",
                    "Za mało miejsca w magazynie!"))
                {
                    SubmitOrder();
                }
            }
            else
                SubmitOrder();
        }

        private void SubmitOrder()
        {
            ReservedStorageSpace += cartList.SpaceSum();
            //mysql => insert into zamowienia ... Data dostarczenia = dzisiaj + czas dostawy + 1 (jutro wyślą dopiero) 
            string query = "Insert into zamowienia(Data_zamowienia, Data_dostarczenia, Magazyn, Koszt) values('" +
               TodayDate.ToString("yyyy-MM-dd") + "', '" +
               DateTime.Now.AddDays(SelectedSeller.DDaysV + 1).ToString("yyyy-MM-dd") + "', " +
               SelectedSeller.ID + ", " +
               cartList.CostSum().ToString().Replace(',', '.') + ")";
            Console.WriteLine(query);
            int id = mysql.insertID(query);
            MoneyExpense += cartList.CostSum();
            mysql.execute("Update info set Wydatki_calkowite = Wydatki_calkowite + " + cartList.CostSum().ToString().Replace(',','.'));
            //najlepiej foreach (var item in cartList){query = insert into pro_zam values (...)}
            query = "";
            foreach(var item in ShoppingCart)
            {
                query = "Insert into pro_zam values ("+item.ID+", "+id+", "+item.Count+")";
                mysql.execute(query);
            }

            ordersList.Add(new structs.zamowienia(
                id.ToString(),
                SelectedSeller.Name,
                TodayDate.ToString("dd/MM/yyyy"),
                DateTime.Now.AddDays(SelectedSeller.DDaysV+1).ToString("dd/MM/yyyy"),
                strConv.money(cartList.CostSum()),
                DeliveryLook));
            OrderClear.Execute(null);
        }
        #endregion

        #region co jest na stanie

        private structs.stanHandler itemsList;
        public ObservableCollection<structs.stan> ItemsList
        {
            get { return itemsList.Items; }
        }
        public void OnHouseToListView(Dictionary<string, double[]> dict)
        {
            itemsList = new structs.stanHandler();
            foreach (KeyValuePair<string, double[]> product in dict)
            {
                int count = (int)product.Value[0];
                itemsList.Add(new structs.stan() { 
                    ID=product.Value[6].ToString(), 
                    Name= product.Key, 
                    Days=product.Value[4].ToString(), 
                    Count=count.ToString() });
            }
        }

        //co jest na stanie
        //{"produkt #id_zam":[ilosc, cena, wysokość podatku, marża dostawcy, termin ważności(ile dni zostało), id_zam, id_stan]}
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

        #endregion

        #region co jest zamówione

        private structs.zamowieniaHandler ordersList;
        public ObservableCollection<structs.zamowienia> Deliveries
        {
            get { return ordersList.Items; }
        }
        public void DeliveriesToListView()
        {
            ordersList = new structs.zamowieniaHandler();
            //wczytaj z mysql zamowienia (widok dodostarczenia)
            List<structs.zam> list = mysql.orders();
            int storage = 0;
            foreach (structs.zam item in list)
            {
                ordersList.Add(new structs.zamowienia() { 
                    ID = item.ID.ToString(), 
                    Name = item.Name, 
                    ODate = item.ODate.ToString("dd/MM/yyyy"),
                    DDate = item.DDate.ToString("dd/MM/yyyy"),
                    Cost = strConv.money(item.Cost), 
                    Action =  DeliveryLook});
                storage += item.Count;
            }
            ReservedStorageSpace = storage;
        }

        private ICommand deliveryLook;
        public ICommand DeliveryLook
        {
            get
            {
                return deliveryLook ?? new RelayCommand(p => 
                {
                    Console.WriteLine(p);
                    //mysql pobierz dane o zamówieniu z id = id
                    //zmień te dane w stringa z przejściami \n i wywołaj 
                    intercom.message(mysql.order(int.Parse(p.ToString())),"Zamówienie #"+p.ToString());
                }, null);
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
                onPropertyChanged(nameof(OpenDateString));
            }
        }
        //data otwarcia jako string
        public string OpenDateString
        {
            get { return ODS(); }
        }
        private string ODS()
        {
            return OpenDate.ToString("dd/MM/yyyy") +" ("+(TodayDate - OpenDate).TotalDays.ToString()+" dni)";
        }

        

        //informacja o ilościach kategorii i produktów
        // {id_kat : count()}
        private Dictionary<int, int> categoriesCounter;
        public Dictionary<int, int> CategoriesCounter
        {
            get { return categoriesCounter; }
            set
            {
                categoriesCounter = value;
                onPropertyChanged(nameof(CategoriesCounter));
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
                onPropertyChanged(nameof(MoneyBalanceString));
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
                onPropertyChanged(nameof(MoneyIncomeString));
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
                onPropertyChanged(nameof(MoneyExpenseString));
                onPropertyChanged(nameof(MoneyProfit));
                onPropertyChanged(nameof(MoneySummary));
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

        //ilość klientów 
        private int todayClientsValue;
        public int TodayClientsValue
        {
            get { return todayClientsValue; }
            set
            {
                todayClientsValue = value;
                onPropertyChanged(nameof(TodayClientsValue));
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
        private int shopLevel;
        public int ShopLevel
        {
            get { return shopLevel; }
            set
            {
                shopLevel = value;
                onPropertyChanged(nameof(ShopLevel));
                onPropertyChanged(nameof(ShopLevelStr));
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
        private int shopState;
        public int ShopState
        {
            get { return shopState; }
            set
            {
                shopState = value;
                onPropertyChanged(nameof(ShopState));
                onPropertyChanged(nameof(ShopStateStr));
            }
        }
        //stan sklepu jako string
        public string ShopStateStr
        {
            get { return SSS(); }
        }
        private string SSS()
        {
            if (ShopState == 0) return "Zły";
            else if (ShopState == 1) return "Średni";
            else if (ShopState == 2) return "Dobry";
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
                onPropertyChanged(nameof(EmployeeString));
                onPropertyChanged(nameof(SalaryString));
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
        private double shopMargin;
        public double ShopMargin
        {
            get { return shopMargin; }
            set
            {
                shopMargin = value;
                onPropertyChanged(nameof(ShopMargin));
                onPropertyChanged(nameof(MarginString));
            }
        }
        //marża jako string
        public string MarginString
        {
            get { return strConv.percentage(ShopMargin); }
        }

        //rodzaj sklepu
        private int shopType;
        public int ShopType
        {
            get { return shopType; }
            set
            {
                shopType = value;
                onPropertyChanged(nameof(ShopType));
                onPropertyChanged(nameof(ShopTypeString));
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
                onPropertyChanged(nameof(StorageSpace));
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
                onPropertyChanged(nameof(StorageSpace));
            }
        }
        //string zapełnienia magazynu
        public string StorageSpace
        {
            get { return StorageValue.ToString() + "/" + StorageSize.ToString(); }
        }
        //zarezerwowana pojemność magazynu
        private int reservedStorageSpace;
        public int ReservedStorageSpace
        {
            get { return reservedStorageSpace; }
            set
            {
                reservedStorageSpace = value;
                onPropertyChanged(nameof(ReservedStorageSpace));
            }
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
                onPropertyChanged(nameof(RentValueString));
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

        //czy dzisiaj jest dostawa
        private bool isDeliveryDay;
        public bool IsDeliveryDay
        {
            get { return isDeliveryDay; }
            set
            {
                isDeliveryDay = value;
                onPropertyChanged(nameof(IsDeliveryDay));
                onPropertyChanged(nameof(IDD));
            }
        }
        public Visibility IDD
        {
            get
            {
                if (IsDeliveryDay) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        //czy dzisiaj jest pierwszy
        private bool isPaymentDay;
        public bool IsPaymentDay
        {
            get { return isPaymentDay; }
            set
            {
                isPaymentDay = value;
                onPropertyChanged(nameof(IsPaymentDay));
                onPropertyChanged(nameof(IPD));
            }
        }
        public Visibility IPD
        {
            get
            {
                if (IsPaymentDay) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        //czy ma wyświetlać panel informacji
        private bool isAlert;
        public bool IsAlert
        {
            get { return isAlert; }
            set
            {
                isAlert = value;
                onPropertyChanged(nameof(IsAlert));
                onPropertyChanged(nameof(IA));
            }
        }
        public Visibility IA
        {
            get
            {
                if (IsAlert) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }


        //info o sklepie - wczytywanie
        private void appOpen()
        {
            Dictionary<string, double> info = mysql.appOpen();
            MoneyBalance = info["Saldo"];
            MoneyIncome = info["Dochod_dzienny"];
            MoneyExpense = info["Wydatki_dzienne"];
            Console.WriteLine("int");
            TodayClientsValue = (int)info["Ruch"];
            Console.WriteLine("int");
            ShopEmployees = (int)info["Liczba_pracownikow"];
            Console.WriteLine("int");
            ShopType = (int)info["Rodzaj"];
            Console.WriteLine("int");
            ShopLevel = (int)info["Poziom"];
            Console.WriteLine("int");
            ShopMargin = info["Marza"];
            RentValue = info["oplaty_miesieczne"];
            EmployeesSalary = info["wynagrodzenie"];
            StorageSize = (int)info["pojemnosc_magazynu"];
            Console.WriteLine("int");

            Dictionary<string, string> infoS = mysql.shopInfo();
            ShopName = infoS["Nazwa"];
            OpenDate = DateTime.Parse(infoS["Data_zalozenia"]);
        }


        //komendy

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

        //zmiana marży - pokazanie slidera
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
                    mysql.execute("update info set liczba_pracownikow = liczba_pracownikow + 1");
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
                    mysql.execute("update info set liczba_pracownikow = liczba_pracownikow - 1");
                    shopInfoUpdate();
                }, arg => ShopEmployees > 0);
            }
        }

        //Aktualizacja informacji o sklepie (stan, poziom, rodzaj)
        public void shopInfoUpdate()
        {
            //określenie rodzaju, stanu i poziomu
            //poziom
            int counter = 0;
            int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;
            int counter5 = 0;
            foreach (KeyValuePair<int, int> item in CategoriesCounter)
            {
                counter += item.Value;
                if (item.Value >= 20)
                {
                    counter2++;
                    counter3++;
                    counter4++;
                    counter5++;
                }
                else if (item.Value >= 15)
                {
                    counter2++;
                    counter3++;
                    counter4++;
                }
                else if (item.Value >= 10)
                {
                    counter2++;
                    counter3++;
                }
                else if (item.Value >= 5)
                    counter2++;

                }
            switch (ShopLevel)
            {
                case 1:
                    //czy spełnia warunki obecnego poziomu
                    if (ShopEmployees < 0 && OnHouseItems.Count < 1)
                        R.Default.LvlDays += 1;
                    //czy spełnia warunki podniesienia poziomu
                    else if (ShopEmployees >= 1 && counter2 >= 2)
                        IsLvlUp = true;
                    break;
                case 2:
                    if (ShopEmployees < 1 && counter2<2)
                        R.Default.LvlDays += 1;
                    else if (ShopEmployees >= 10 && counter3 >= 5)
                        IsLvlUp = true;
                    break;
                case 3:
                    if (ShopEmployees < 10 && counter3 < 5)
                        R.Default.LvlDays += 1;
                    else if (ShopEmployees >= 18 && counter4 >= 10)
                        IsLvlUp = true;
                    break;
                case 4:
                    if (ShopEmployees < 18 && counter4 < 10)
                        R.Default.LvlDays += 1;
                    else if (ShopEmployees >= 30 && counter5 >= 15)
                        IsLvlUp = true;
                    break;
                case 5:
                    //czy spełnia warunki obecnego poziomu
                    if (ShopEmployees < 30 &&  counter5 < 15)
                        R.Default.LvlDays += 1;
                    break;
                default:
                    //czy spełnia warunki podniesienia poziomu
                    if (ShopEmployees >= 0 && CategoriesCounter.Count >= 1 )
                        IsLvlUp = true;
                    break;
            }

            //stan
            switch (ShopLevel)
            {
                case 1:
                    if (ShopEmployees > 2)
                        ShopState = 3;
                    else if (ShopEmployees > 0)
                        ShopState = 2;
                    else
                        ShopState = 1;
                    break;
                case 2:
                    if (ShopEmployees > 12)
                        ShopState = 3;
                    else if (ShopEmployees > 8)
                        ShopState = 2;
                    else if (ShopEmployees >= 1)
                        ShopState = 1;
                    else
                        ShopState = 0;
                    break;
                case 3:
                    if (ShopEmployees > 20)
                        ShopState = 3;
                    else if (ShopEmployees > 16)
                        ShopState = 2;
                    else if (ShopEmployees >= 10)
                        ShopState = 1;
                    else
                        ShopState = 0;
                    break;
                case 4:
                    if (ShopEmployees > 32)
                        ShopState = 3;
                    else if (ShopEmployees > 28)
                        ShopState = 2;
                    else if (ShopEmployees >= 18)
                        ShopState = 1;
                    else
                        ShopState = 0;
                    break;
                case 5:
                    if (ShopEmployees > 52)
                        ShopState = 3;
                    else if (ShopEmployees > 48)
                        ShopState = 2;
                    else if (ShopEmployees >= 30)
                        ShopState = 1;
                    else
                        ShopState = 0;
                    break;
                default:
                    ShopState = 1;
                    break;
            }
            Console.WriteLine(ShopState);

            //rodzaj
            
            if(CategoriesCounter.Count > 1)
            {
                double[] percentages = new double[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach(KeyValuePair<int, int> items in CategoriesCounter)
                {
                    percentages[items.Key - 1] = items.Value / counter;
                }
                if (percentages[5] == 0 && percentages[6] == 0 && percentages[7] == 0 && percentages[8] == 0 && percentages[9] == 0 && percentages[10] == 0) ShopType = 1;
                else if (percentages[0] + percentages[1] == 1) ShopType = 2;
                else if (percentages[6] >= 0.65 ) ShopType = 3;
                else if (percentages[5] >= 0.9) ShopType = 5;
                else if (percentages[8] >= 0.95) ShopType = 6;
                else if (percentages[10] >= 0.8) ShopType = 9;
                else if (percentages[14] >= 0.85) ShopType = 11;
                else ShopType = 10;
            }
            else
            {
                int type = CategoriesCounter.Keys.Single();
                if (type == 1) ShopType = 2;
                else if (type == 2) ShopType = 2;
                else if (type == 3) ShopType = 4;
                else if (type == 4) ShopType = 1;
                else if (type == 5) ShopType = 8;
                else if (type == 6) ShopType = 5;
                else if (type == 7) ShopType = 3;
                else if (type == 8) ShopType = 7;
                else if (type == 9) ShopType = 6;
                else if (type == 10) ShopType = 10;
                else if (type == 11) ShopType = 9;
                else if (type == 12) ShopType = 1;
                else if (type == 13) ShopType = 1;
                else if (type == 14) ShopType = 1;
                else ShopType = 11;
            }
            
        }

        //czy można levelować
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

        //levelowanie
        private ICommand levelUp;
        public ICommand LevelUp 
        {
            get
            {
                return levelUp ?? new RelayCommand(p => 
                {
                    //lewelowanie
                    //sql zwiększ level
                    mysql.execute("update info set Poziom = Poziom + 1");
                    mysql.execute("update info set Najwyzszy_poziom = Poziom");
                    //wczytaj dane dla levelu
                    appOpen();
                    ShopLevel += 1;
                }, null);
            }
        }

        //wyświetlenie infoboxa z informacjami
        private ICommand infoBox;
        public ICommand InfoBox
        {
            get
            {
                return infoBox ?? new RelayCommand(p => intercom.infoBox((string)p), null);
            }
        }

        #endregion

        #region nowa gra

        //pokazanie ekranu tworzenia nowej gry
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

        //powrót do menu
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

        //tworzenie nowej gry
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
                mysql.execute("call TheBlip()");
                //mysql => SET SESSION sql_mode = 'NO_AUTO_VALUE_ON_ZERO';
                mysql.execute("SET SESSION sql_mode = 'NO_AUTO_VALUE_ON_ZERO'");
                //mysql => Insert Into zamowienia Values(0,CURDATE(),CURDATE(),0,0);
                mysql.execute("Insert Into zamowienia Values(0,CURDATE(),CURDATE(),0,0)");

                //typ sklepu
                /*
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
                */
                mysql.execute("Insert into stan (Ilosc,Produkt,Zamowienie) values " +
                    "(500,3,0), (500,10,0), (500,24,0), (500,26,0), (10,29,0), (100,44,0)");
                mysql.execute("Insert Into pro_zam Values " +
                    "(3, 0, 500), (10, 0, 500), (24, 0, 500), (26, 0, 500), (29, 0, 10), (44, 0, 100)");

                string query = "Insert Into info Values ('"+ 
                    ShopNameInpt.Replace('"', '\"').Replace("'", "\'") +
                    "',CURDATE(),2000,CURDATE(),0.4,0,0,0,0,0,0,0,10,"+(SelectedShopType+1)+",1,1);";
                //mysql => query
                mysql.execute(query);
                //mysql => Update stan Set Liczba_zamowien = 0;
                mysql.execute("Update info Set Liczba_zamowien = 0");

                R.Default.ClientsCountValue = 10;
                R.Default.TodayDate = DateTime.Now;
                R.Default.SoldItemsString = "";
                R.Default.IsGameSaved = true;
                R.Default.Save();

                NGVis = Visibility.Collapsed;
                DateVis = Visibility.Visible;
                ShopNameInpt = "";
                gameWindowAsync();
            }
        }

        //nowa nazwa sklepu
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
        
        //indeks (rodzaj) sklepu
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

        //przycisk zamknij sklep
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
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(@"../../sounds/funeral.mp3", UriKind.Relative));
                player.Play();
                ByeScreen();
            }
        }

        private void GameOver()
        {
            ByeScreen();
        }

        private void ByeScreen()
        {
            //mysql pobierz dane końcowe
            //ustaw zmienne danych końcowych
            Dictionary<string, string> info = mysql.endInfo();
            TotalIncome = info["dochod"];
            TotalExpense = info["wydatki"];
            TotalOrdersValue = info["liczba"];
            TotalOrdersCost = info["koszt"];
            HighestLevel = info["lvl"];

            GOVis = Visibility.Visible;
            DateVis = Visibility.Collapsed;
            GameVis = Visibility.Collapsed;

            //czyszczenie bazy danych
            //mysql => the blip
            mysql.execute("call TheBlip()");

            IsSavedGame = false;
            R.Default.IsGameSaved = false;
            R.Default.SoldItemsString = "";
            R.Default.ClientsCountValue = 0;
            R.Default.TodayDate = DateTime.Now;
            R.Default.Save();

        }

        //powrót do menu
        private ICommand gOmenu;
        public ICommand GOmenu
        {
            get
            {
                return gOmenu ?? new RelayCommand(p =>
                {
                    GOVis = Visibility.Collapsed;
                    MenuVis = Visibility.Visible;
                }, null);
            }
        }

        //zmienne do podsumowania game over
        private double totalIncome;
        public string TotalIncome
        {
            get { return strConv.money(totalIncome); }
            set
            {
                totalIncome = double.Parse(value);
                onPropertyChanged(nameof(TotalIncome));
            }
        }
        private double totalExpense;
        public string TotalExpense
        {
            get { return strConv.money(totalExpense); }
            set
            {
                totalExpense = double.Parse(value);
                onPropertyChanged(nameof(TotalExpense));
            }
        }
        private double totalOrdersValue;
        public string TotalOrdersValue
        {
            get { return strConv.money(totalOrdersValue); }
            set
            {
                totalOrdersValue = double.Parse(value);
                onPropertyChanged(nameof(TotalOrdersValue));
            }
        }
        private double totalOrdersCost;
        public string TotalOrdersCost
        {
            get { return strConv.money(totalOrdersCost); }
            set
            {
                totalOrdersCost = double.Parse(value);
                onPropertyChanged(nameof(TotalOrdersCost));
            }
        }
        private int highestLevel;
        public string HighestLevel
        {
            get {
                if (highestLevel == 1) return "(1) - Sklepik";
                else if (highestLevel == 2) return "(2) - Sklep";
                else if (highestLevel == 3) return "(3) - Dyskont";
                else if (highestLevel == 4) return "(4) - Supermarket";
                else if (highestLevel == 5) return "(5) - Hipermarket";
                else return "(0) - Pustostan";
            }
            set
            {
                highestLevel = int.Parse(value);
                onPropertyChanged(nameof(HighestLevel));
            }
        }
        
        #endregion
  
    }
}
