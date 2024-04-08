using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private BoardManager _board;

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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
