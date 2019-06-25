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

        private Random random;

        public ArtificialPlayer(int id, string name) : base(id, name)
        {
            random = new Random();
        }

        protected override void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);
            
            ownField.PlacementFinished += OwnField_PlacementFinished;
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
        }

        private Cell GetRandomCell(PlayField field)
        {
            return field[random.Next(PlayField.RowCount), random.Next(PlayField.ColumnCount)];
        }
    }
}