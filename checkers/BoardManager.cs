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
using System.IO;

namespace checkers
{
    public class BoardManager : INotifyPropertyChanged
    {
        public List<Piece> _pieces;
        public List<MovePoint> _movePoints;
        private int _pickedPiece;
        private Players _players;
        private Piece _piece;
        public bool _canDoubleJump = true;
        private int _player1Wins = 0;
        private int _player2Wins = 0;
        private bool _playerJumped = false;
        private bool _playerCanEliminateAgain = false;

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

        public BoardManager(Players players, bool canDoubleJump)
        {
            InitializeTable();
            _canDoubleJump = canDoubleJump;
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
                _piece = piece;
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
                _playerCanEliminateAgain = CanPieceEliminateAgain(piece);

            }
        }

        //Remove errors from createmovepoints function
        private void CreateMovePoints(Piece piece)
        {
            double leftMargin = piece.Margin.Left;
            double topMargin = piece.Margin.Top;

            if (piece.IsKing)
            {
                // Verificare și adăugare pentru locul din dreapta sus
                if (piece.XPos + 1 <= 7 && piece.YPos + 1 <= 7)
                {
                    if (isPlaceFree(piece.XPos + 1, piece.YPos + 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos + 2, piece.YPos + 2) && !isPlaceFree(piece.XPos + 1, piece.YPos + 1) && piece.IsRedPiece != IsPieceRed(piece.XPos + 1, piece.YPos+1))
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
                    if (isPlaceFree(piece.XPos - 1, piece.YPos + 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos - 2, piece.YPos + 2) && !isPlaceFree(piece.XPos - 1, piece.YPos + 1) && piece.IsRedPiece != IsPieceRed(piece.XPos - 1, piece.YPos+1))
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

                // Verificare și adăugare pentru locul din dreapta jos
                if (piece.XPos + 1 <= 7 && piece.YPos - 1 >= 0)
                {
                    if (isPlaceFree(piece.XPos + 1, piece.YPos - 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos + 2, piece.YPos - 2) && !isPlaceFree(piece.XPos + 1, piece.YPos - 1) && piece.IsRedPiece != IsPieceRed(piece.XPos + 1, piece.YPos-1))
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
                    if (isPlaceFree(piece.XPos - 1, piece.YPos - 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos - 2, piece.YPos - 2) && !isPlaceFree(piece.XPos - 1, piece.YPos - 1) && piece.IsRedPiece != IsPieceRed(piece.XPos - 1, piece.YPos-1))
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
            else if (piece.IsRedPiece)
            {
                // Verificare și adăugare pentru locul din dreapta sus
                if (piece.XPos + 1 <= 7 && piece.YPos + 1 <= 7)
                {
                    if (isPlaceFree(piece.XPos + 1, piece.YPos + 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos + 2, piece.YPos + 2) && !isPlaceFree(piece.XPos + 1, piece.YPos + 1) && piece.IsRedPiece != IsPieceRed(piece.XPos + 1, piece.YPos+1))
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
                    if (isPlaceFree(piece.XPos - 1, piece.YPos + 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin + 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos + 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos + 2 <= 7 && isPlaceFree(piece.XPos - 2, piece.YPos + 2) && !isPlaceFree(piece.XPos - 1, piece.YPos + 1) && piece.IsRedPiece != IsPieceRed(piece.XPos - 1, piece.YPos+1))
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
                    if (isPlaceFree(piece.XPos + 1, piece.YPos - 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin + 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos + 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos + 2 <= 7 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos + 2, piece.YPos - 2) && !isPlaceFree(piece.XPos + 1, piece.YPos - 1) && piece.IsRedPiece != IsPieceRed(piece.XPos + 1, piece.YPos - 1))
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
                    if (isPlaceFree(piece.XPos - 1, piece.YPos - 1) && _playerJumped == false)
                    {
                        Thickness margin = new Thickness(leftMargin - 100, topMargin - 100, 0, 0);
                        MovePoint movePoint = new MovePoint(piece.XPos - 1, piece.YPos - 1) { Margin = margin };
                        movePoint.MovePointClicked += OnMovePointClicked;
                        _movePoints.Add(movePoint);
                    }
                    else if (piece.XPos - 2 >= 0 && piece.YPos - 2 >= 0 && isPlaceFree(piece.XPos - 2, piece.YPos - 2) && !isPlaceFree(piece.XPos - 1, piece.YPos - 1) && piece.IsRedPiece != IsPieceRed(piece.XPos - 1, piece.YPos-1))
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

        private bool IsPieceRed(float x, float y)
        {
            foreach (Piece piece in _pieces)
            {
                if (piece.XPos == x && piece.YPos == y)
                {
                    return piece.IsRedPiece;
                }
            }
            return false;
        }   

        private void SetPieceAsKing(Piece piece)
        {
            if (piece.IsRedPiece && piece.YPos == 7)
            {
                piece.IsKing = true;
            }
            else if (!piece.IsRedPiece && piece.YPos == 0)
            {
                piece.IsKing = true;
            }
        }

        private void CheckIfGameIsFinished()
        {
            int redPieces = 0;
            int whitePieces = 0;
            foreach (Piece piece in _pieces)
            {
                if (piece.IsRedPiece)
                {
                    redPieces++;
                }
                else
                {
                    whitePieces++;
                }
            }
            if (redPieces == 0)
            {
                MessageBox.Show("Jocul s-a terminat! Jucătorul 2 a câștigat!");
                _player2Wins++;
                // Salvam datele in fisier
                SaveWinsToFile();
            }
            else if (whitePieces == 0)
            {
                MessageBox.Show("Jocul s-a terminat! Jucătorul 1 a câștigat!");
                _player1Wins++;
                // Salvam datele in fisier
                SaveWinsToFile();
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
            CheckIfGameIsFinished();
        }
        private void SaveWinsToFile()
        {
            string filePath = "wins.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Player 1 wins: " + _player1Wins);
                writer.WriteLine("Player 2 wins: " + _player2Wins);
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

        private bool CanPieceEliminateAgain(Piece piece)
        {
            foreach (MovePoint movePoint in _movePoints)
            {
                if (movePoint._canEliminate)
                {
                    // Verificăm dacă piesa este regină
                    if (piece.IsKing)
                    {
                        // Verificare și adăugare pentru locul din dreapta sus
                        if (movePoint.XPos + 1 <= 7 && movePoint.YPos + 1 <= 7)
                        {
                            if (movePoint.XPos + 2 <= 7 && movePoint.YPos + 2 <= 7 && isPlaceFree(movePoint.XPos + 2, movePoint.YPos + 2) && !isPlaceFree(movePoint.XPos + 1, movePoint.YPos + 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos + 1, movePoint.YPos + 1))
                            {
                                return true;
                            }
                        }
                        // Verificare și adăugare pentru locul din stânga sus
                        if (movePoint.XPos - 1 >= 0 && movePoint.YPos + 1 <= 7)
                        {
                            if (movePoint.XPos - 2 >= 0 && movePoint.YPos + 2 <= 7 && isPlaceFree(movePoint.XPos - 2, movePoint.YPos + 2) && !isPlaceFree(movePoint.XPos - 1, movePoint.YPos + 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos - 1, movePoint.YPos + 1))
                            {
                                return true;
                            }
                        }
                        // Verificare și adăugare pentru locul din dreapta jos
                        if (movePoint.XPos + 1 <= 7 && movePoint.YPos - 1 >= 0)
                        {
                            if (movePoint.XPos + 2 <= 7 && movePoint.YPos - 2 >= 0 && isPlaceFree(movePoint.XPos + 2, movePoint.YPos - 2) && !isPlaceFree(movePoint.XPos + 1, movePoint.YPos - 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos + 1, movePoint.YPos - 1))
                            {
                                return true;
                            }
                        }
                        // Verificare și adăugare pentru locul din stânga jos
                        if (movePoint.XPos - 1 >= 0 && movePoint.YPos - 1 >= 0)
                        {
                            if (movePoint.XPos - 2 >= 0 && movePoint.YPos - 2 >= 0 && isPlaceFree(movePoint.XPos - 2, movePoint.YPos - 2) && !isPlaceFree(movePoint.XPos - 1, movePoint.YPos - 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos - 1, movePoint.YPos - 1))
                            {
                                return true;
                            }
                        }
                    }
                    else // Piesa nu este regină
                    {
                        // Verificăm direcțiile posibile de eliminare în funcție de culoarea piesei
                        if (piece.IsRedPiece)
                        {
                            // Verificare și adăugare pentru locul din dreapta sus
                            if (movePoint.XPos + 1 <= 7 && movePoint.YPos + 1 <= 7)
                            {
                                if (movePoint.XPos + 2 <= 7 && movePoint.YPos + 2 <= 7 && isPlaceFree(movePoint.XPos + 2, movePoint.YPos + 2) && !isPlaceFree(movePoint.XPos + 1, movePoint.YPos + 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos + 1, movePoint.YPos + 1))
                                {
                                    return true;
                                }
                            }
                            // Verificare și adăugare pentru locul din stânga sus
                            if (movePoint.XPos - 1 >= 0 && movePoint.YPos + 1 <= 7)
                            {
                                if (movePoint.XPos - 2 >= 0 && movePoint.YPos + 2 <= 7 && isPlaceFree(movePoint.XPos - 2, movePoint.YPos + 2) && !isPlaceFree(movePoint.XPos - 1, movePoint.YPos + 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos - 1, movePoint.YPos + 1))
                                {
                                    return true;
                                }
                            }
                        }
                        else // Piesa este albă
                        {
                            // Verificare și adăugare pentru locul din dreapta jos
                            if (movePoint.XPos + 1 <= 7 && movePoint.YPos - 1 >= 0)
                            {
                                if (movePoint.XPos + 2 <= 7 && movePoint.YPos - 2 >= 0 && isPlaceFree(movePoint.XPos + 2, movePoint.YPos - 2) && !isPlaceFree(movePoint.XPos + 1, movePoint.YPos - 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos + 1, movePoint.YPos - 1))
                                {
                                    return true;
                                }
                            }
                            // Verificare și adăugare pentru locul din stânga jos
                            if (movePoint.XPos - 1 >= 0 && movePoint.YPos - 1 >= 0)
                            {
                                if (movePoint.XPos - 2 >= 0 && movePoint.YPos - 2 >= 0 && isPlaceFree(movePoint.XPos - 2, movePoint.YPos - 2) && !isPlaceFree(movePoint.XPos - 1, movePoint.YPos - 1) && piece.IsRedPiece != IsPieceRed(movePoint.XPos - 1, movePoint.YPos - 1))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void OnMovePointClicked(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(MovePoint))
            {
                MovePoint movePoint = (MovePoint)sender;
                if (movePoint._canEliminate)
                {
                    RemoveAPiece(movePoint._eliminatedX, movePoint._eliminatedY);
                    if (!_canDoubleJump || _playerCanEliminateAgain == false)
                    {
                        Players.ChangePlayer();
                        _playerJumped = false;
                    }
                    else
                        _playerJumped = true;
                }
                else
                {
                    _playerJumped = false;
                    Players.ChangePlayer();
                }


                _piece.XPos = movePoint.XPos;
                _piece.YPos = movePoint.YPos;
                _piece.Margin = movePoint.Margin;
                if (_piece.IsRedPiece && _piece.YPos == 7)
                {
                    _piece.IsKing = true;
                }
                else if (!_piece.IsRedPiece && _piece.YPos == 0)
                {
                    _piece.IsKing = true;
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
