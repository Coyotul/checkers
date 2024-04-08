using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace checkers
{
    public partial class Game : Window
    {
        public Game()
        {
            InitializeComponent();
            CreateTable();
        }


        private void CreateTable()
        {
            int rows = 8;
            int columns = 8;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    AddRectangleToGrid(row, col);
                }
            }
        }

        private void AddRectangleToGrid(int row, int column)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 100;
            rectangle.Height = 100;
            if ((row % 2 == 0 && column % 2 == 0) || (row % 2 == 1 && column % 2 == 1))
            {
                rectangle.Fill = Brushes.DarkOrange;
            }
            else
            {
                rectangle.Fill = Brushes.LightGoldenrodYellow;
            }

            rectangle.Margin = new Thickness(column * 100, row * 100, 800 - ((column + 1) * 100), 800 - ((row + 1) * 100) - rectangle.Height+100);

            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);

            Checkerboard.Children.Add(rectangle);
        }

    }
}
