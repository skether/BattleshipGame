using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleshipGame
{
    public class Player
    {
        public int ID { get; }
        public string Name { get; }
        public List<Ship> Ships { get; private set; }

        private bool _active;
        public bool Active { get { return _active; } set { _active = value; ManageTurn(); } }

        GameWindow window;
        PlayField ownField;
        PlayField enemyField;

        public event EventHandler<GameEventArgs> GameEventNotify;

        public Player(int id, string name)
        {
            ID = id;
            Name = name;

            _active = false;

            Ships = new List<Ship>() { new Ship(5), new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2) };

            window = new GameWindow(this);
            window.Closed += Window_Closed;
            window.Loaded += Window_Loaded;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.Exit, null));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ownField = new PlayField(window.OwnFieldCanvas);
            enemyField = new PlayField(window.EnemyFieldCanvas);

            enemyField.CellHit += EnemyField_CellHit;

            ownField.PlacementFinished += OwnField_PlacementFinished;
        }

        private void EnemyField_CellHit(object sender, CellHitEventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.Hit, e.Target));
        }

        private void OwnField_PlacementFinished(object sender, PlacementFinishedEventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.PlacementFinished, null));
        }

        public bool Hit(int row, int column)
        {
            Cell cell = ownField[row, column];
            cell.IsHit = true;
            return cell.IsShip;
        }

        private void ManageTurn()
        {
            if(Active)
            {
                enemyField.State = FieldState.Attacking;
            }
            else
            {
                enemyField.State = FieldState.ReadOnly;
            }
        }

        public void Start() { ownField.PlaceShips(Ships); }

        public void Show() { window.Show(); }

        public void Hide() { window.Hide(); }

        public void Close() { window.Close(); }
    }
}
