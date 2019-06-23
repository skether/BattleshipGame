using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    public class GameManager
    {
        Player p1;
        Player p2;
        bool p2ai;

        public event EventHandler<EventArgs> GameFinished;

        public GameManager(string p1name, string p2name, bool ai)
        {
            p1 = new Player(1, p1name);
            p2 = new Player(2, p2name);
            p2ai = ai;

            p1.GameEventNotify += Player_GameEvent;
            p2.GameEventNotify += Player_GameEvent;
        }

        private void Player_GameEvent(object sender, GameEventArgs e)
        {
            if (!(sender is Player player)) return;

            switch (e.Type)
            {
                case GameEvent.PlacementFinished:
                    break;
                case GameEvent.Shot:
                    break;
                case GameEvent.Exit:
                    switch (player.ID)
                    {
                        case 1: p2.Close(); Finish(); break;
                        case 2: p1.Close(); Finish(); break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Start()
        {
            p1.Show();
            if (!p2ai) p2.Show();
        }

        void Finish()
        {
            GameFinished?.Invoke(this, new EventArgs());
        }
    }

    public enum GameEvent
    {
        PlacementFinished,
        Shot,
        Exit
    }

    public class GameEventArgs : EventArgs
    {
        public GameEvent Type { get; }
        public int Row { get; }
        public int Column { get; }

        public GameEventArgs(GameEvent type, int row, int column)
        {
            Type = type;
            Row = row;
            Column = column;
        }
    }
}
