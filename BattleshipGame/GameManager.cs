﻿using System;
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
            if ((new Random()).Next(2) == 0)
            {
                p1 = new HumanPlayer(1, p1name);
                if (ai) p2 = new ArtificialPlayer(2, p2name);
                else p2 = new HumanPlayer(2, p2name);
            }
            else
            {
                if (ai) p1 = new ArtificialPlayer(1, p2name);
                else p1 = new HumanPlayer(1, p2name);
                p2 = new HumanPlayer(2, p1name);
            }

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
                    if (!(p1.Ready || p2.Ready)) Finish(); //Finish when all windows are closed

                    if (InProgress) //Handle forfeit
                    {
                        switch (player.ID)
                        {
                            case 1: End(p2, p1); break;
                            case 2: End(p1, p2); break;
                            default:
                                break;
                        }
                    }

                    if (!InProgress && player is HumanPlayer) //Close the AI window if any
                    {
                        switch (player.ID)
                        {
                            case 1: if (p2 is ArtificialPlayer && p2.IsHidden) p2.Close(); break;
                            case 2: if (p1 is ArtificialPlayer && p1.IsHidden) p1.Close(); break;
                            default:
                                break;
                        }
                    }
                    break;

                case GameEvent.WindowShowKey:
                    if (player is ArtificialPlayer) break;
                    if (p1 is ArtificialPlayer) p1.ToggleVisibility();
                    else if (p2 is ArtificialPlayer) p2.ToggleVisibility();
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

            if (otherPlayer.Hit(cell.Row, cell.Column))
            {
                cell.IsShip = true;
                currentPlayer.HitCount++;
                Ship ship = Ship.WhichShip(otherPlayer.Ships, cell.Row, cell.Column);
                if (ship.IsSunk)
                {
                    ship.MarkSunk();
                    currentPlayer.ShipSunk(ship);
                }
            }

            if (otherPlayer.Ships.Count(x => !x.IsSunk) == 0)
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

        private void End(Player winner, Player looser)
        {
            Database.AddResult(new MatchResult(winner.Name, looser.Name, DateTime.Now));
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
        Exit,
        WindowShowKey
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