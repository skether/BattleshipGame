using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BattleshipGame
{
    public enum FieldState
    {
        ReadOnly,
        ShipPlacing,
        Attacking
    }

    public class PlayField
    {
        public static int RowCount { get; set; } = 10;
        public static int ColumnCount { get; set; } = 10;

        private Canvas canvas;
        private Cell[,] cells;

        public FieldState State { get; set; }

        private int currentShipIndex;
        private Ship currentShip;
        private List<Ship> ships;
        private List<Cell> shipPreviewCells;
        public event EventHandler<PlacementFinishedEventArgs> PlacementFinished;

        public event EventHandler<CellHitEventArgs> CellHit;

        public PlayField(Canvas canvas)
        {
            //Setup the defaults
            State = FieldState.ReadOnly;

            //Setup the canvas
            this.canvas = canvas;
            this.canvas.Children.Clear();

            //Setup the cells
            cells = new Cell[RowCount, ColumnCount];
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    cells[r, c] = new Cell(this, r, c);
                    cells[r, c].InteractionEvent += Cell_InteractionEvent;
                    this.canvas.Children.Add(cells[r, c]);
                }
            }

            //Size the cells
            ResizeCells();
            this.canvas.SizeChanged += Canvas_SizeChanged;
        }

        #region Ship placement

        public void PlaceShips(List<Ship> shipsToPlace)
        {
            if (shipsToPlace.Count < 1) return;

            ships = shipsToPlace;
            currentShipIndex = 0;
            currentShip = ships[currentShipIndex];
            shipPreviewCells = new List<Cell>();
            State = FieldState.ShipPlacing;
        }

        private void PreviewShip_Start(Cell anchor)
        {
            //Clear previous cells if any
            if (shipPreviewCells.Count > 0) foreach (Cell cell in shipPreviewCells) cell.IsHighlighted = false;
            shipPreviewCells.Clear();

            //Mark new cells
            shipPreviewCells.AddRange(CalculateShipCells(anchor));
            foreach (Cell cell in shipPreviewCells) cell.IsHighlighted = true;
        }

        private void PreviewShip_End(Cell anchor)
        {
            foreach (Cell cell in shipPreviewCells) cell.IsHighlighted = false;
            shipPreviewCells.Clear();
        }

        private void PlaceCurrentShip(Cell anchor)
        {
            List<Cell> candidates = CalculateShipCells(anchor);
            if (candidates.Count == 0) return; //Couldn't place ship in this position!

            foreach (Cell cell in candidates) cell.IsShip = true;
            currentShip.cells = candidates;
            PreviewShip_End(anchor);

            //Go to the next ship if any
            if (currentShipIndex + 1 < ships.Count)
            {
                currentShipIndex++;
                currentShip = ships[currentShipIndex];
            }
            else
            {
                PlacementFinished?.Invoke(this, new PlacementFinishedEventArgs(ships));
                currentShip = null;
                currentShipIndex = -1;
                State = FieldState.ReadOnly;
            }
        }

        private List<Cell> CalculateShipCells(Cell anchor)
        {
            List<Cell> shipCells = new List<Cell>();

            if (currentShip.IsHorizontal)
            {
                //Calculate the starting index of this ship
                int startIndex = anchor.Column - ((currentShip.Size - 1) / 2);
                startIndex = startIndex < 0 ? 0 : startIndex;
                startIndex = startIndex + currentShip.Size >= ColumnCount ? ColumnCount - currentShip.Size : startIndex;
                int endIndex = startIndex + currentShip.Size - 1;

                //Check if there are any ships nearby
                for (int r = Math.Max(anchor.Row - 1, 0); r <= Math.Min(anchor.Row + 1, RowCount - 1); r++)
                {
                    for (int c = Math.Max(startIndex - 1, 0); c <= Math.Min(endIndex + 1, ColumnCount - 1); c++)
                    {
                        if (cells[r, c].IsShip) return shipCells;
                    }
                }

                //Mark all the cells in this ship
                for (int i = 0; i < currentShip.Size; i++)
                {
                    shipCells.Add(cells[anchor.Row, startIndex + i]);
                }
            }
            else
            {
                int startIndex = anchor.Row - ((currentShip.Size - 1) / 2);
                startIndex = startIndex < 0 ? 0 : startIndex;
                startIndex = startIndex + currentShip.Size >= RowCount ? ColumnCount - currentShip.Size : startIndex;
                int endIndex = startIndex + currentShip.Size - 1;

                //Check if there are any ships nearby
                for (int r = Math.Max(startIndex - 1, 0); r <= Math.Min(endIndex + 1, RowCount - 1); r++)
                {
                    for (int c = Math.Max(anchor.Column - 1, 0); c <= Math.Min(anchor.Column + 1, ColumnCount - 1); c++)
                    {
                        if (cells[r, c].IsShip) return shipCells;
                    }
                }

                //Mark all the cells in this ship
                for (int i = 0; i < currentShip.Size; i++)
                {
                    shipCells.Add(cells[startIndex + i, anchor.Column]);
                }
            }

            return shipCells;
        }

        #endregion

        #region Cell Interaction

        private void Cell_InteractionEvent(object sender, InteractionEventArgs e)
        {
            if (!(sender is Cell cell)) return;

            switch (e.Type)
            {
                //Interaction: Mouse Enter
                case InteractionType.Enter:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;

                        case FieldState.ShipPlacing:
                            PreviewShip_Start(cell);
                            break;

                        case FieldState.Attacking:
                            cell.IsHighlighted = true;
                            break;

                        default:
                            break;
                    }
                    break;

                //Interaction: Mouse Leave
                case InteractionType.Leave:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;

                        case FieldState.ShipPlacing:
                            PreviewShip_End(cell);
                            break;

                        case FieldState.Attacking:
                            cell.IsHighlighted = false;
                            break;

                        default:
                            break;
                    }
                    break;

                //Interaction: Mouse Left Click
                case InteractionType.LeftClick:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;

                        case FieldState.ShipPlacing:
                            PlaceCurrentShip(cell);
                            break;

                        case FieldState.Attacking:
                            cell.IsHit = true;
                            CellHit?.Invoke(this, new CellHitEventArgs(cell));
                            break;

                        default:
                            break;
                    }
                    break;

                //Interaction: Mouse Right Click
                case InteractionType.RightClick:
                    switch (State)
                    {
                        case FieldState.ReadOnly:
                            break;

                        case FieldState.ShipPlacing:
                            currentShip.IsHorizontal = !currentShip.IsHorizontal;
                            PreviewShip_Start(cell);
                            break;

                        case FieldState.Attacking:
                            break;

                        default:
                            break;
                    }
                    break;

                //Default
                default:
                    break;
            }
        }

        #endregion

        #region Cell Size Management

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

        #endregion
    }

    public class CellHitEventArgs : EventArgs
    {
        public Cell Target { get; }

        public CellHitEventArgs(Cell cell)
        {
            Target = cell;
        }
    }

    public class PlacementFinishedEventArgs : EventArgs
    {
        public List<Ship> Ships { get; }

        public PlacementFinishedEventArgs(List<Ship> ships)
        {
            Ships = ships;
        }
    }
}