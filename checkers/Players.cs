using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class Players : INotifyPropertyChanged
    {
        bool player1 = true;
        bool player2 = false;
        public string playerText = "Player 1";

        public void ChangePlayer()
        {
            if (player1)
            {
                player1 = false;
                player2 = true;
                PlayerText = "Player 2";
                playerText = PlayerText;
            }
            else
            {
                player1 = true;
                player2 = false;
                PlayerText = "Player 1";
                playerText = PlayerText;
            }
        }

        public bool IsPlayer1
        {
            get { return player1; }
            set
            {
                player1 = value;
                OnPropertyChanged(nameof(IsPlayer1));
            }
        }

        public string PlayerText
        {
            get { return playerText; }
            set
            {
                playerText = value;
                OnPropertyChanged(nameof(PlayerText));
            }
        }

        public Players()
        {
            player1 = true;
            player2 = false;
            playerText = "Player 1";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
