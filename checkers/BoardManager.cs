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
    public class BoardManager : INotifyPropertyChanged
    {
        public List<Piece> _pieces;
        public List<MovePoint> _movePoints;
        private int _pickedPiece;

        public List<Piece> Pieces
        {
            get { return _pieces; }
            set
            {
                _pieces = value;
                OnPropertyChanged(nameof(Pieces));
            }
        }
        public List<MovePoint> MovePoints
        {
            get { return _movePoints; }
            set
            {
                _movePoints = value;
                OnPropertyChanged(nameof(MovePoints));
            }
        }

        public BoardManager()
        {
            InitializeTable();
        }
        public async Task InitializeTable()
        {
            Pieces = new List<Piece>();
            MovePoints = new List<MovePoint>();
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
                            Piece piece = new Piece(false, true, col, row) { Margin = margin };
                            piece.index = Pieces.Count;
                            Pieces.Add(piece);
                            piece.PieceClicked += OnPieceClicked; // Abonare la eveniment
                        }
                        else if (row > 4)
                        {
                            Piece piece = new Piece(false, false, col, row) { Margin = margin };
                            piece.index = Pieces.Count;
                            Pieces.Add(piece);
                            piece.PieceClicked += OnPieceClicked; // Abonare la eveniment                        
                        }
                    }
                }
            }
        }

        private void OnPieceClicked(object sender, EventArgs e)
        {
            if(sender.GetType() == typeof(Piece))
            {
                Piece piece = (Piece)sender;
                if (piece.IsRedPiece)
                {

                    double leftMargin = piece.Margin.Left;
                    double topMargin = piece.Margin.Top;
                    Thickness margin = new Thickness(leftMargin+100, topMargin+100, 0, 0);
                    MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                    movePoint.MovePointClicked += OnMovePointClicked;
                    _movePoints.Add(movePoint);
                    margin = new Thickness(leftMargin-100, topMargin + 100, 0, 0);
                    MovePoint movePoint2 = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                    movePoint2.MovePointClicked += OnMovePointClicked;
                    _movePoints.Add(movePoint2);
                    _pickedPiece = piece.index;
                }
                else
                {
                    double leftMargin = piece.Margin.Left;
                    double topMargin = piece.Margin.Top;
                    Thickness margin = new Thickness(leftMargin + 100, topMargin - 100, 0, 0);
                    MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                    movePoint.MovePointClicked += OnMovePointClicked;
                    _movePoints.Add(movePoint);
                    margin = new Thickness(leftMargin - 100, topMargin - 100, 0, 0);
                    MovePoint movePoint2 = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                    movePoint2.MovePointClicked += OnMovePointClicked;
                    _movePoints.Add(movePoint2);
                    _pickedPiece = piece.index;
                }
                MovePoints = _movePoints.ToList();

            }
        }

        private void OnMovePointClicked(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(MovePoint))
            {
                MovePoint movePoint = (MovePoint)sender;

                // Verificați dacă _pickedPiece este un index valid pentru lista _pieces
                if (_pickedPiece >= 0 && _pickedPiece < _pieces.Count)
                {
                    // Actualizați proprietățile XPos și YPos ale obiectului Piece asociat cu _pickedPiece
                    _pieces[_pickedPiece].XPos = movePoint.XPos;
                    _pieces[_pickedPiece].YPos = movePoint.YPos;
                    _pieces[_pickedPiece].Margin = movePoint.Margin;
                }
                else
                {
                    // Mesaj de avertizare sau tratament pentru cazul în care _pickedPiece este un index invalid
                    MessageBox.Show("Indexul _pickedPiece este invalid!");
                }

                // Curățați și actualizați lista _movePoints
                _movePoints.Clear();
                MovePoints = _movePoints.ToList();

                // Actualizați lista Pieces
                Pieces = _pieces.ToList();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
