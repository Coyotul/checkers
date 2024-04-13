using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Net.NetworkInformation;

namespace checkers
{
    public class BoardManager : INotifyPropertyChanged
    {
        public List<Piece> _pieces;
        public List<MovePoint> _movePoints;
        private int _pickedPiece;
        private Players _players;

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

        public Players Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public BoardManager(Players players)
        {
            InitializeTable();
            _players = players;
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
            if (sender.GetType() == typeof(Piece))
            {
                Piece piece = (Piece)sender;
                _pickedPiece = piece.index;
                bool isPlayer1Turn = _players.IsPlayer1;
                _movePoints.Clear();
                MovePoints = _movePoints.ToList();

                if ((!isPlayer1Turn && piece.IsRedPiece))
                {
                    CreateMovePoints(piece);
                }
                else if ((isPlayer1Turn && !piece.IsRedPiece))
                {
                    CreateMovePoints(piece);
                }
                MovePoints = _movePoints.ToList();
            }
        }

        //Remove errors from createmovepoints function
        private void CreateMovePoints(Piece piece)
        {
            double leftMargin = piece.Margin.Left;
            double topMargin = piece.Margin.Top;

            if (piece.IsRedPiece)
            {
                // Verificare și adăugare pentru locul din dreapta sus
                if (piece.XPos + 1 <= 7 && piece.YPos + 1 <= 7)
                {
                    if (isPlaceFree(piece.XPos + 1, piece.YPos + 1))
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos + 2, piece.YPos + 2) && !isPlaceFree(piece.XPos + 1, piece.YPos + 1))
                    {
                        Thickness margin = new Thickness(leftMargin + 200, topMargin + 200, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 2, piece.YPos + 2) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        movePoint._canEliminate = true;
                        movePoint._eliminatedX = piece.XPos + 1;
                        movePoint._eliminatedY = piece.YPos + 1;
                        _movePoints.Add(movePoint);
                    }
                }
                // Verificare și adăugare pentru locul din stânga sus
                if (piece.XPos - 1 >= 0 && piece.YPos + 1 <= 7)
                {
                    if (isPlaceFree(piece.XPos - 1, piece.YPos + 1))
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos - 2, piece.YPos + 2) && !isPlaceFree(piece.XPos - 1, piece.YPos + 1))
                    {
                        Thickness margin = new Thickness(leftMargin - 200, topMargin + 200, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 2, piece.YPos + 2) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        movePoint._canEliminate = true;
                        movePoint._eliminatedX = piece.XPos - 1;
                        movePoint._eliminatedY = piece.YPos + 1;
                        _movePoints.Add(movePoint);
                    }
                }
            }
            else // Pentru piesele albe
            {
                // Verificare și adăugare pentru locul din dreapta jos
                if (piece.XPos + 1 <= 7 && piece.YPos - 1 >= 0)
                {
                    if (isPlaceFree(piece.XPos + 1, piece.YPos - 1))
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos + 2, piece.YPos - 2) && !isPlaceFree(piece.XPos + 1, piece.YPos - 1))
                    {
                        Thickness margin = new Thickness(leftMargin + 200, topMargin - 200, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 2, piece.YPos - 2) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        movePoint._canEliminate = true;
                        movePoint._eliminatedX = piece.XPos + 1;
                        movePoint._eliminatedY = piece.YPos - 1;
                        _movePoints.Add(movePoint);
                    }
                }
                // Verificare și adăugare pentru locul din stânga jos
                if (piece.XPos - 1 >= 0 && piece.YPos - 1 >= 0)
                {
                    if (isPlaceFree(piece.XPos - 1, piece.YPos - 1))
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos - 2, piece.YPos - 2) && !isPlaceFree(piece.XPos - 1, piece.YPos - 1))
                    {
                        Thickness margin = new Thickness(leftMargin - 200, topMargin - 200, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 2, piece.YPos - 2) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        movePoint._canEliminate = true;
                        movePoint._eliminatedX = piece.XPos - 1;
                        movePoint._eliminatedY = piece.YPos - 1;
                        _movePoints.Add(movePoint);
                    }
                }
            }

        }

        private void RemoveAPiece(float x, float y)
        {
            foreach (Piece piece in _pieces)
            {
                if (piece.XPos == x && piece.YPos == y)
                {
                    _pieces.Remove(piece);
                    break;
                }
            }
        }   

        private bool isPlaceFree(float x, float y)
        {
            foreach (Piece piece in _pieces)
            {
                if (piece.XPos == x && piece.YPos == y)
                {

                    return false;
                }
            }
            return true;
        }

        private void OnMovePointClicked(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(MovePoint))
            {
                MovePoint movePoint = (MovePoint)sender;
                if(movePoint._canEliminate)
                {
                    RemoveAPiece(movePoint._eliminatedX, movePoint._eliminatedY);
                }   
                else
                {
                    Players.ChangePlayer();
                }
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
