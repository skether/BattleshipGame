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
    public partial class MainWindow : Window
    {

        PlayField ownField;
        PlayField enemyField;

        public MainWindow()
        {
            InitializeComponent();
            SetupGameField();
        }

        private void SetupGameField()
        {
            ownField = new PlayField(OwnFieldCanvas);
            enemyField = new PlayField(EnemyFieldCanvas);
            enemyField.State = FieldState.Attacking;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}