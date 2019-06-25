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

        public bool InProgress { get; private set; }

        public event EventHandler<EventArgs> GameFinished;

        public GameManager(string p1name, string p2name, bool ai)
        {
            p1 = new HumanPlayer(1, p1name);
            if (ai) p2 = new ArtificialPlayer(2, p2name);
            else p2 = new HumanPlayer(2, p2name);

            InProgress = false;

            p1.GameEventNotify += Player_GameEvent;
            p2.GameEventNotify += Player_GameEvent;
        }

        private void Player_GameEvent(object sender, GameEventArgs e)
        {
            if (!(sender is Player player)) return;

            switch (e.Type)
            {
                case GameEvent.PlacementFinished:
                    switch (player.ID)
                    {
                        case 1: p2.Start(); break;
                        case 2: p1.Active = true; break;
                        default:
                            break;
                    }
                    break;
                case GameEvent.Hit:
                    ProcessHit(player, e.Target);
                    break;
                case GameEvent.Exit:
                    if (!(p1.Ready || p2.Ready)) Finish();
                    if (!InProgress) break;
                    switch (player.ID)
                    {
                        case 1: End(p2, p1); break;
                        case 2: End(p1, p2); break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ProcessHit(Player currentPlayer, Cell cell)
        {
            Player otherPlayer = null;
            switch (currentPlayer.ID)
            {
                case 1: otherPlayer = p2; break;
                case 2: otherPlayer = p1; break;
                default: break;
            }
            if (otherPlayer == null) return;

            if (otherPlayer.Hit(cell.Row, cell.Column)) cell.IsShip = true;

            if(otherPlayer.Ships.Count(x => !x.IsSunk) == 0)
            {
                End(currentPlayer, otherPlayer);
                return;
            }

            currentPlayer.Active = false;
            otherPlayer.Active = true;
        }

        public void Start()
        {
            p1.Show();
            p2.Show();

            p1.Start();

            InProgress = true;
        }

        public void End(Player winner, Player looser)
        {
            InProgress = false;
            winner.End(true);
            looser.End(false);
        }

        private void Finish()
        {
            GameFinished?.Invoke(this, new EventArgs());
        }
    }

    public enum GameEvent
    {
        PlacementFinished,
        Hit,
        Exit
    }

    public class GameEventArgs : EventArgs
    {
        public GameEvent Type { get; }
        public Cell Target { get; }

        public GameEventArgs(GameEvent type, Cell cell)
        {
            Type = type;
            Target = cell;
        }
    }
}
