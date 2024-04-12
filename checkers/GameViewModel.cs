using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace checkers
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private BoardManager _board;
        private Players _players;
        public ICommand ClickCommand { get; private set; }


        public BoardManager Board
        {
            get { return _board; }
            set
            {
                _board = value;
                OnPropertyChanged(nameof(Board));
            }
        }

        public Players Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }


        public GameViewModel()
        {
            Players = new Players();
            Board = new BoardManager(Players);
            ClickCommand = new RelayCommand(ExecuteClickCommand);
        }

        private void ExecuteClickCommand(object parameter)
        {
            // Logica asociată cu evenimentul de clic pe imagine
            MessageBox.Show("Ai făcut clic pe o piesă!");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
