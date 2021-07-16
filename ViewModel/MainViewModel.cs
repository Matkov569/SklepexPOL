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
        //DateTime timeStamp = R.Default.TodayDate;
        //int RCCV = R.Default.ClientsCountValue;
        
        //generator klientów i zakupów
        clientRandomizer randomizer = new clientRandomizer();
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
            R.Default.Save();
        }


        #region panel menu i gry
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
                dateVis = value;
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
            double[] key = new double[] { 110, 5 };
            Dictionary<string, double[]> slownik = new Dictionary<string, double[]>();
            slownik.Add("pomidor", key);
            slownik.Add("ogórek", key);
            slownik.Add("Brokuł", key);
            OnHouseItems = slownik;
            TDN();
            TodaySoldItems = R.Default.SoldItemsString;
            MenuVis = Visibility.Collapsed;
            DateVis = Visibility.Visible;
            await Task.Delay(3000);
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
            TodayDate = TodayDate.AddDays(1);
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
            gameDateSwitch();
            TodayClientsValue = randomizer.clientsCreator(TodayClientsValue, ShopMargin, ShopState, ShopLevel, TodayDate);
            SoldItems = randomizer.shopListGenerator(TodayClientsValue, OnHouseItems, ShopLevel, TodayDate);
            //TSI();
            R.Default.SoldItemsString = TSI();
            R.Default.Save();
            await Task.Delay(3000);
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
        //co jest na stanie - wczytać z bazy
        //{"produkt":[ilosc, cena, wysokość podatku, marża dostawcy, termin ważności(ile dni zostało)]}
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
        //liczba pracowników
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
        #endregion

        #region nowa gra
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
            Console.WriteLine(ShopNameInpt);
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
        #endregion
    }
}
