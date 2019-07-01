using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BattleshipGame
{
    public class HumanPlayer : Player
    {

        public HumanPlayer(int id, string name) : base(id, name) { }

        protected override void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);

            enemyField.CellHit += EnemyField_CellHit;
            ownField.PlacementFinished += OwnField_PlacementFinished;
        }

        private void EnemyField_CellHit(object sender, CellHitEventArgs e)
        {
            Shoot(e.Target);
        }

        private void OwnField_PlacementFinished(object sender, PlacementFinishedEventArgs e)
        {
            BackgroundColor = inactiveBackground;
            window.StatusText.Text = textPleaseWait;

            RaisePlacementFinished();
        }

        public override void Start()
        {
            BackgroundColor = activeBackground;
            window.StatusText.Text = textPlaceYourShips;

            ownField.PlaceShips(Ships);
        }

        protected override void ActTurn()
        {
            switch (Active)
            {
                case true: enemyField.State = FieldState.Attacking; break;
                case false: enemyField.State = FieldState.ReadOnly; break;
                default: break;
            }
        }
    }
}