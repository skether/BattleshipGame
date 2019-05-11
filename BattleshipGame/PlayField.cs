using System;
using System.Windows.Controls;

namespace BattleshipGame
{
    public enum FieldState
    {
        ReadOnly,
        ShipPlacing,
        Attacking
    }

    internal class PlayField
    {
        public static int RowCount { get; set; } = 10;
        public static int ColumnCount { get; set; } = 10;

        private Canvas canvas;
        private Cell[,] cells;

        public FieldState State { get; set; }

        public PlayField(Canvas canvas)
        {
            this.canvas = canvas;
            this.canvas.Children.Clear();

            this.State = FieldState.ReadOnly;

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
            if (!(sender is Cell cell)) return;
            switch (e.Type)
            {
                case InteractionType.Enter:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;
                        case FieldState.ShipPlacing:
                            break;
                        case FieldState.Attacking:
                            cell.IsHighlighted = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case InteractionType.Leave:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;
                        case FieldState.ShipPlacing:
                            break;
                        case FieldState.Attacking:
                            cell.IsHighlighted = false;
                            break;
                        default:
                            break;
                    }
                    break;
                case InteractionType.LeftClick:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;
                        case FieldState.ShipPlacing:
                            break;
                        case FieldState.Attacking:
                            cell.IsHit = true;
                            break;
                        default:
                            break;
                    }
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
            double size = Math.Floor(Math.Min((canvas.ActualWidth - (2 * ColumnCount)) / ColumnCount, (canvas.ActualHeight - (2 * RowCount)) / RowCount));
            return size < 0 ? 0 : size;
        }
    }
}