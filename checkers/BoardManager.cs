using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class BoardManager
    {
        private List<Piece> _pieces;

        public List<Piece> Pieces
        {
            get { return _pieces; }
            set
            {
                _pieces = value;
                OnPropertyChanged(nameof(Pieces));
            }
        }

        public BoardManager()
        {
            InitializeTable();
        }

        private void InitializeTable()
        {
            Pieces = new List<Piece>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
