using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace checkers
{
    public class Piece : INotifyPropertyChanged
    {
        private bool _isKing = false;
        private bool _isRedPiece = false;
        private float _xPos;
        private float _yPos;
        private string _imagePath;
        private Thickness _margin;



        public bool IsKing
        {
            get { return _isKing; }
            set
            {
                if (_isKing != value)
                {
                    _isKing = value;
                    OnPropertyChanged(nameof(IsKing));
                }
            }
        }

        public bool IsRedPiece
        {
            get { return _isRedPiece; }
            set
            {
                if (_isRedPiece != value)
                {
                    _isRedPiece = value;
                    OnPropertyChanged(nameof(IsRedPiece));
                }
            }
        }

        public float XPos
        {
            get { return _xPos; }
            set
            {
                if (_xPos != value)
                {
                    _xPos = value;
                    OnPropertyChanged(nameof(XPos));
                }
            }
        }

        public float YPos
        {
            get { return _yPos; }
            set
            {
                if (_yPos != value)
                {
                    _yPos = value;
                    OnPropertyChanged(nameof(YPos));
                }
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }


        public Piece(bool isKing, bool isRedPiece, float xPos, float yPos)
        {
            IsKing = isKing;
            IsRedPiece = isRedPiece;
            XPos = xPos;

            YPos = yPos;
            if (isRedPiece)
            {
                _imagePath = "red.png";
            }
            else
            {
                _imagePath = "white.png";
            }
        }

        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                if (_margin != value)
                {
                    _margin = value;
                    OnPropertyChanged(nameof(Margin));
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
