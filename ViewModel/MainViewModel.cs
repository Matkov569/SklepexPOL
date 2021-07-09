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
        //komendy zmiany widoczności
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

    }
}
