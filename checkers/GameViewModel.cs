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


        public GameViewModel()
        {
            Board = new BoardManager();
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
