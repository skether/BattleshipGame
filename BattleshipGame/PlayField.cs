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
                    cells[r, c] = new Cell(this, r, c);
                    cells[r, c].InteractionEvent += Cell_InteractionEvent;
                    canvas.Children.Add(cells[r, c]);
                }
            }

            this.canvas.SizeChanged += Canvas_SizeChanged;

            ResizeCells();
        }

        private void Cell_InteractionEvent(object sender, InteractionEventArgs e)
        {
            switch (e.Type)
            {
                case InteractionType.Enter:
                    break;
                case InteractionType.Leave:
                    break;
                case InteractionType.LeftClick:
                    if (!Clickable) return;
                    if (sender is Cell cell) cell.IsHit = true;
                    break;
                case InteractionType.RightClick:
                    break;
                default:
                    break;
            }
        }

        private void Canvas_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ResizeCells();
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
