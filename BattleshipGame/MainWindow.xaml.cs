using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int fieldCols = 10;
        public const int fieldRows = 10;

        private int ownFieldDistanceFromLeft = 10;
        private int ownFieldDistanceFromTop = 10;

        private Cell[,] ownField;

        public MainWindow()
        {
            InitializeComponent();
            SetupGameField();
        }

        private void SetupGameField()
        {
            ownField = new Cell[fieldRows, fieldCols];

            //Generate Rectangles for this player's field
            int rectSize = 20;
            for (int r = 0; r < fieldRows; r++)
            {
                for (int c = 0; c < fieldCols; c++)
                {
                    ownField[r, c] = new Cell(r, c)
                    {
                        Height = rectSize,
                        Width = rectSize
                    };
                    ownField[r, c].SetValue(Canvas.LeftProperty, (double)(ownFieldDistanceFromLeft + (c * rectSize)));
                    ownField[r, c].SetValue(Canvas.TopProperty, (double)(ownFieldDistanceFromTop + (r * rectSize)));
                    ownField[r, c].MouseLeftButtonDown += Cell_MouseLeftButtonDown;
                    FieldCanvas.Children.Add(ownField[r, c]);
                }
            }
            
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Cell cell) cell.IsHit = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ownField[9, 9].Width = 100;
            ownField[9, 9].Height = 100;
        }
    }
}