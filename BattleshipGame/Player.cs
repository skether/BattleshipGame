using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BattleshipGame
{
    public abstract class Player
    {
        public static Brush inactiveBackground = Brushes.SlateGray;
        public static Brush activeBackground = Brushes.White;
        public static Brush victoryBackground = Brushes.LimeGreen;
        public static Brush loseBackground = Brushes.Red;

        public static string textPleaseWait = "Kérlek várj!";
        public static string textPlaceYourShips = "Helyezd el a hajóidat!";
        public static string textTakeYourShot = "Te következel!";
        public static string textYouWin = "Nyertél!";
        public static string textYouLose = "Vesztettél!";

        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public List<Ship> Ships { get; protected set; }

        protected bool _active;
        public bool Active { get { return _active; } set { _active = value; ManageTurn(); } }
        public bool Ready { get; protected set; }
        public bool IsHidden { get { return window.Visibility == Visibility.Hidden; } }
        public bool AllowClose { get; set; }

        protected Brush BackgroundColor { get { return window.Background; } set { window.Background = value; window.OwnFieldCanvas.Background = value; window.EnemyFieldCanvas.Background = value; } }

        protected GameWindow window;
        protected PlayField ownField;
        protected PlayField enemyField;

        public event EventHandler<GameEventArgs> GameEventNotify;

        public Player(int id, string name)
        {
            ID = id;
            Name = name;

            Ready = false;
            _active = false;
            AllowClose = true;

            Ships = new List<Ship>() { new Ship(5), new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2) };

            window = new GameWindow(this);
            window.KeyUp += Window_KeyUp;
            window.Closed += Window_Closed;
            window.Loaded += Window_Loaded;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Tab)
            {
                GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.WindowShowKey, null));
            }
        }

        protected virtual void Window_Closed(object sender, EventArgs e)
        {
            Ready = false;
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.Exit, null));
        }

        protected virtual void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundColor = inactiveBackground;

            ownField = new PlayField(window.OwnFieldCanvas);
            enemyField = new PlayField(window.EnemyFieldCanvas);

            Ready = true;
        }

        public bool Hit(int row, int column)
        {
            Cell cell = ownField[row, column];
            cell.IsHit = true;
            return cell.IsShip;
        }

        public virtual void ShipSunk(Ship ship)
        {
            foreach (Cell cell in ship.cells)
            {
                enemyField[cell.Row, cell.Column].IsSunk = true;
            }
        }

        protected virtual void Shoot(Cell target)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.Hit, target));
        }

        protected void RaisePlacementFinished()
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.PlacementFinished, null));
        }

        public abstract void Start();

        protected virtual void ManageTurn()
        {
            if (Active)
            {
                BackgroundColor = activeBackground;
                window.StatusText.Text = textTakeYourShot;
            }
            else
            {
                BackgroundColor = inactiveBackground;
                window.StatusText.Text = textPleaseWait;
            }
            ActTurn();
        }

        protected abstract void ActTurn();

        public virtual void End(bool victory)
        {
            Active = false;
            BackgroundColor = victory ? victoryBackground : loseBackground;
            window.StatusText.Text = victory ? textYouWin : textYouLose;
        }

        public void Show() { window.Show(); }

        public void Hide() { window.Hide(); }

        public void ToggleVisibility()
        {
            switch (window.Visibility)
            {
                case Visibility.Visible:
                    window.Hide();
                    break;
                case Visibility.Hidden:
                    window.Show();
                    break;
                case Visibility.Collapsed:
                    break;
                default:
                    break;
            }
        }

        public void Close() { window.Close(); }

    }
}