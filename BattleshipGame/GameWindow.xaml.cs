using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public string PlayerName { get; set; }

        PlayField ownField;
        PlayField enemyField;

        public event EventHandler<GameEventArgs> GameEventNotify;

        public GameWindow()
        {
            InitializeComponent();
            DataContext = this;

            SetupGameField();
        }

        private void SetupGameField()
        {
            ownField = new PlayField(OwnFieldCanvas);
            enemyField = new PlayField(EnemyFieldCanvas);

            ownField.PlacementFinished += OwnField_PlacementFinished;
            ownField.PlaceShips(new List<Ship>() { new Ship(5), new Ship(4), new Ship(3), new Ship(3), new Ship(2), new Ship(2) });
        }

        private void OwnField_PlacementFinished(object sender, PlacementFinishedEventArgs e)
        {
            GameEventNotify?.Invoke(this, new GameEventArgs(GameEvent.PlacementFinished, -1, -1));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public enum GameEvent
    {
        PlacementFinished,
        Shot
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