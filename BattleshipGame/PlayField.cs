using System;
using System.Windows.Controls;

namespace BattleshipGame
{
    internal class PlayField
    {
        public static int RowCount { get; set; } = 10;
        public static int ColumnCount { get; set; } = 10;

        private Canvas canvas;
        private Cell[,] cells;

        public bool Clickable { get; set; }

        public PlayField(Canvas canvas)
        {
            this.canvas = canvas;
            this.canvas.Children.Clear();

            this.Clickable = false;

            cells = new Cell[RowCount, ColumnCount];

            double cellSize = CalculateCellSize();

            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    cells[r, c] = new Cell(r, c);
                    cells[r, c].MouseLeftButtonDown += Cell_MouseLeftButtonDown;
                    canvas.Children.Add(cells[r, c]);
                }
            }

            this.canvas.SizeChanged += Canvas_SizeChanged;

            ResizeCells();
        }

        private void Canvas_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ResizeCells();
        }

        private void Cell_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Clickable) return;
            if (sender is Cell cell) cell.IsHit = true;
        }

        private void ResizeCells()
        {
            double cellSize = CalculateCellSize();
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    cells[r, c].Width = cellSize;
                    cells[r, c].Height = cellSize;
                    cells[r, c].SetValue(Canvas.LeftProperty, (2 + (c * (cellSize + 1))));
                    cells[r, c].SetValue(Canvas.TopProperty, (2 + (r * (cellSize + 1))));
                }
            }
        }

        private double CalculateCellSize()
        {
            double size = Math.Min(canvas.ActualWidth / ColumnCount, canvas.ActualHeight / RowCount);
            return size < 1 ? size : size - 1;
        }
    }
}
