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
    public class MovePoint : INotifyPropertyChanged
    {
        public float _xPos { get; set; }
        public float _yPos { get; set; }
        public bool _canEliminate { get; set; }
        public float _eliminatedX { get; set; } 
        public float _eliminatedY { get; set; }
        private string _imagePath;
        private Thickness _margin;
        public event EventHandler MovePointClicked;
        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get { return _clickCommand; }
            set
            {
                _clickCommand = value;
                OnPropertyChanged(nameof(ClickCommand));
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

        public MovePoint(float x, float y)
        {
            ClickCommand = new RelayCommand(ExecuteClickCommand, CanExecuteClickCommand);

            _xPos = x;
            _yPos = y;
            _imagePath = "movePoint1.png";
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

        private void ExecuteClickCommand(object parameter)
        {
            MovePointClicked?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteClickCommand(object parameter)
        {
            return true;
        }

       

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
