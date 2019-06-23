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

        GameWindow window;
        PlayField ownField;
        PlayField enemyField;

        public event EventHandler<GameEventArgs> GameEventNotify;

        public Player(int id, string name)
        {
            ID = id;
            Name = name;
            window = new GameWindow(this);
            window.Closed += Window_Closed;
            window.Loaded += Window_Loaded;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.Exit, -1, -1));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ownField = new PlayField(window.OwnFieldCanvas);
            enemyField = new PlayField(window.EnemyFieldCanvas);

            ownField.PlacementFinished += OwnField_PlacementFinished;
            ownField.PlaceShips(new List<Ship>() { new Ship(5), new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2) });
        }

        private void OwnField_PlacementFinished(object sender, PlacementFinishedEventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.PlacementFinished, -1, -1));
        }

        public void Show() { window.Show(); }

        public void Hide() { window.Hide(); }

        public void Close() { window.Close(); }
    }
}
