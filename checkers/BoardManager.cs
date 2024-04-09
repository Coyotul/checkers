using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace checkers
{
    public class BoardManager
    {
        public List<Piece> _pieces;

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
        public async Task InitializeTable()
        {
            Pieces = new List<Piece>();
            //await Task.Delay(1);
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Verifică dacă celula curentă este neagră
                    if ((row + col) % 2 != 0)
                    {
                        double leftMargin = col * 100;  // Calculul valorii Margin.Left
                        double topMargin = row * 100;  // Calculul valorii Margin.Top
                        Thickness margin = new Thickness(leftMargin, topMargin, 0, 0);

                        // Adaugă piese cu proprietatea Margin setată corespunzător
                        if (row < 3)
                        {
                            Pieces.Add(new Piece(false, true, col, row) { Margin = margin });
                        }
                        else if (row > 4)
                        {
                            Pieces.Add(new Piece(false, false, col, row) { Margin = margin });
                        }
                    }
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
