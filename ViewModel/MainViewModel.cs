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

    class MainViewModel : BaseViewModel
    {
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
        //zwiększenie daty o 1
        private ICommand dateUp;
        public ICommand DateUp
        {
            get
            {
                return dateUp ?? new RelayCommand(p => dayChangeAsync(), null);
            }
        }
        private async Task dayChangeAsync()
        {
            TodayDate = TodayDate.AddDays(1);
            
            MediaPlayer player = new MediaPlayer();
            
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
            await Task.Delay(3000);
            dateGameSwitch();
        }
        //przejście z ekranu daty do ekranu gry
        public void dateGameSwitch()
        {
            DateVis = Visibility.Collapsed;
            GameVis = Visibility.Visible;
        }
        //przejście z ekranu daty do ekranu gry
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
    }
}
