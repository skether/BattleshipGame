using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BattleshipGame
{
    public class ArtificialPlayer : Player
    {
        private static readonly int MaximumShipPlacementTries = 100;

        private List<Cell> targets;
        private Cell lastTarget;
        private Cell currentTargetAnchor;
        private bool? currentTargetDirection;
        private Random random;

        public ArtificialPlayer(int id, string name) : base(id, name)
        {
            AllowClose = false;

            targets = new List<Cell>();
            lastTarget = null;
            currentTargetAnchor = null;
            currentTargetDirection = null;
            random = new Random();
        }

        protected override void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);
            
            ownField.PlacementFinished += OwnField_PlacementFinished;

            window.Closing += Window_Closing;
            window.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Hide();
            e.Cancel = !AllowClose;
        }

        private void OwnField_PlacementFinished(object sender, PlacementFinishedEventArgs e)
        {
            RaisePlacementFinished();
        }

        public override void Start()
        {
            PlaceShips(false);
        }

        private void PlaceShips(bool reset)
        {
            if (reset) foreach (Ship cShip in Ships) cShip.cells.Clear();

            ownField.PlaceShips(Ships);
            ownField.State = FieldState.ReadOnly;

            foreach (Ship cShip in Ships)
            {
                int tryCount = 0;
                cShip.IsHorizontal = random.Next(2) == 0;
                while (!ownField.PlaceCurrentShip(GetRandomCell(ownField)) && tryCount < MaximumShipPlacementTries) tryCount++;

                if (tryCount < MaximumShipPlacementTries) continue;

                cShip.IsHorizontal = !cShip.IsHorizontal;
                tryCount = 0;
                while (!ownField.PlaceCurrentShip(GetRandomCell(ownField)) && tryCount < MaximumShipPlacementTries) tryCount++;

                if (tryCount >= MaximumShipPlacementTries)
                {
                    PlaceShips(true);
                    break;
                }
            }
        }

        protected override void ActTurn()
        {
            if(Active)
            {
                if (currentTargetAnchor == null && lastTarget != null && lastTarget.IsShip) //Check if last target was a ship
                {
                    currentTargetAnchor = lastTarget;
                    if (lastTarget.Row > 0) AddTarget(enemyField[lastTarget.Row - 1, lastTarget.Column]);
                    if (lastTarget.Row + 1 < PlayField.RowCount) AddTarget(enemyField[lastTarget.Row + 1, lastTarget.Column]);
                    if (lastTarget.Column > 0) AddTarget(enemyField[lastTarget.Row, lastTarget.Column - 1]);
                    if (lastTarget.Column + 1 < PlayField.ColumnCount) AddTarget(enemyField[lastTarget.Row, lastTarget.Column + 1]);
                }

                if(currentTargetAnchor == null) //No known cell, Pick a random cell
                {
                    Cell target = null;
                    do
                    {
                        target = GetRandomCell(enemyField);
                    } while (target != null && target.IsHit);

                    Shoot(target);
                }
                else //Select target from available targets
                {
                    if(currentTargetAnchor == lastTarget) //Only one known cell, Pick a random cell from the targets
                    {
                        Shoot(GetRandomCell(targets));
                    }
                    else
                    {
                        if(currentTargetDirection == null && lastTarget.IsShip) //Calculate direction of the ship
                        {
                            if(lastTarget.Row == currentTargetAnchor.Row) //Horizontal, remove vertical targets
                            {
                                targets.RemoveAll(x => x.Row != currentTargetAnchor.Row);
                                currentTargetDirection = true;
                            }
                            else //Vertical, remove horizontal targets
                            {
                                targets.RemoveAll(x => x.Column != currentTargetAnchor.Column);
                                currentTargetDirection = false;
                            }
                        }

                        if(currentTargetDirection == true && lastTarget.IsShip) //Horizontal
                        {
                            if (lastTarget.Column < currentTargetAnchor.Column)
                            {
                                if (lastTarget.Column > 0) AddTarget(enemyField[currentTargetAnchor.Row, lastTarget.Column - 1]);
                            }
                            else if (lastTarget.Column > currentTargetAnchor.Column)
                            {
                                if (lastTarget.Column + 1 < PlayField.ColumnCount) AddTarget(enemyField[currentTargetAnchor.Row, lastTarget.Column + 1]);
                            }
                        }
                        else if(currentTargetDirection == false && lastTarget.IsShip) //Vertical
                        {
                            if (lastTarget.Row < currentTargetAnchor.Row)
                            {
                                if (lastTarget.Row > 0) AddTarget(enemyField[lastTarget.Row - 1, currentTargetAnchor.Column]);
                            }
                            else if (lastTarget.Row > currentTargetAnchor.Row)
                            {
                                if (lastTarget.Row + 1 < PlayField.RowCount) AddTarget(enemyField[lastTarget.Row + 1, currentTargetAnchor.Column]);
                            }
                        }

                        Shoot(GetRandomCell(targets));
                    }
                }
            }
        }

        protected override void Shoot(Cell target)
        {
            target.IsHit = true;
            lastTarget = target;
            targets.Remove(target);
            base.Shoot(target);
        }

        public override void ShipSunk(Ship ship)
        {
            base.ShipSunk(ship);

            targets.Clear();
            lastTarget = null;
            currentTargetAnchor = null;
            currentTargetDirection = null;
        }

        private Cell GetRandomCell(PlayField field)
        {
            return field[random.Next(PlayField.RowCount), random.Next(PlayField.ColumnCount)];
        }

        private Cell GetRandomCell(List<Cell> cells)
        {
            return cells[random.Next(cells.Count)];
        }

        private void AddTarget(Cell cell)
        {
            if (!cell.IsHit) targets.Add(cell);
        }

        public override void End(bool victory)
        {
            base.End(victory);
            AllowClose = true;
            window.Close();
        }
    }
}