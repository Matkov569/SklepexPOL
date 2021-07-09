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

    class MainViewModel : BaseViewModel
    {
        //składowa interfejsu 
        //zdarzenie wywoływane w chwili zmiany własności o której chcemy powiadomić
        //żeby zaktualizowany został widok

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
        private object content = new View.menu();
        public object Content
        {
            get { return content; }
            set 
            {
                content = value;
                onPropertyChanged(nameof(Content));
            }
        }
        
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

        private ICommand windowGame;
        public ICommand WindowGame
        {
            get
            {
                return windowGame ?? new RelayCommand(prop => gameWindow(), null);
            }
        }

        private void gameWindow()
        {
            MenuVis = Visibility.Collapsed;
            GameVis = Visibility.Visible;
        }
        /*private Model.Lottery lottery=new Model.Lottery();


        private int[] randomNumbers;
        public int[] RandomNumbers
        {
            get { return randomNumbers; }
            private set
            {
                randomNumbers = value;
               
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RandomNumbers)));
            }
        }


        //polecenie 
        private ICommand randomization;

        public ICommand Randomization
        {
            get {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return randomization ?? (randomization=new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p)=> { RandomNumbers=lottery.LotteryRandom();}
                    , 
                    //warunek kiedy może je wykonać
                    p=>true )
                    );
            }
        }
        */

    }
}
