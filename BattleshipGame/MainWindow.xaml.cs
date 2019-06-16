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
using System.Windows.Shapes;

namespace BattleshipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameWindow p1Window;
        GameWindow p2Window;

        public MainWindow()
        {
            InitializeComponent();
            p1Window = null;
            p2Window = null;
        }

        private void PlayerTwoAICheckbox_Checked(object sender, RoutedEventArgs e)
        {
            playerTwoName.IsEnabled = false;
            playerTwoName.Text = "AI";
        }

        private void PlayerTwoAICheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            playerTwoName.Text = "Játékos 2";
            playerTwoName.IsEnabled = true;
        }

        private void CheckNames()
        {
            if (playerOneName.Text.Length == 0) playerOneName.Text = "Játékos 1";
            if (playerTwoName.Text.Length == 0) playerTwoName.Text = "Játékos 2";
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            CheckNames();

            //Setup Player1
            p1Window = new GameWindow() { PlayerName = playerOneName.Text };
            p1Window.Closed += GameWindow_Closed;

            //Setup Player2
            p2Window = new GameWindow() { PlayerName = playerTwoName.Text };
            p2Window.Closed += GameWindow_Closed;
            if (playerTwoAICheckbox.IsChecked == true) p2Window.Hide();

            //Show the Game windows and hide this one!
            p1Window.Show();
            if (playerTwoAICheckbox.IsChecked == false) p2Window.Show();
            this.Hide();
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            if (!(sender is GameWindow window)) return;
            if (window == p1Window) p2Window.Close();
            else p1Window.Close();
            this.Show();

            ///TODO: Handle forfeit
        }
    }
}
