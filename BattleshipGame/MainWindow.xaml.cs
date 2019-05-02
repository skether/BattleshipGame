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

        private Rectangle[,] ownField;
        private Rectangle[,] enemyField;

        public MainWindow()
        {
            InitializeComponent();
            SetupGameField();
        }

        private void SetupGameField()
        {
            ownField = new Rectangle[fieldRows, fieldCols];

            //Generate Rectangles for this player's field
            int rectSize = 20;
            for (int r = 0; r < fieldRows; r++)
            {
                for (int c = 0; c < fieldCols; c++)
                {
                    ownField[r, c] = new Rectangle()
                    {
                        Height = rectSize,
                        Width = rectSize,
                        Stroke = Brushes.Black,
                        Fill = Brushes.RoyalBlue,
                        Margin = new Thickness((ownFieldDistanceFromLeft + (c * rectSize)), (ownFieldDistanceFromTop + (r * rectSize)), 0, 0),
                    };
                    ownField[r, c].MouseEnter += OwnRectangle_MouseEnter;
                    ownField[r, c].MouseLeave += OwnRectangle_MouseLeave;
                    FieldCanvas.Children.Add(ownField[r, c]);
                }
            }


        }

        private void OwnRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            if (rect != null) rect.Fill = Brushes.Red;
        }

        private void OwnRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            if (rect != null) rect.Fill = Brushes.RoyalBlue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //rect.Fill = Brushes.Red;
        }
    }
}