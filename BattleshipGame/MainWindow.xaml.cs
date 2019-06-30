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
        GameManager gameManager = null;

        public MainWindow()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            historyBoard.ItemsSource = Database.GetHistory();
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

        private bool CheckNames()
        {
            if (playerOneName.Text.Length == 0) playerOneName.Text = "Játékos 1";
            if (playerTwoName.Text.Length == 0) playerTwoName.Text = "Játékos 2";

            if (playerOneName.Text == playerTwoName.Text)
            {
                MessageBox.Show("Nem játszhatsz önmagad ellen! :)", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            if (!CheckNames()) return;

            gameManager = new GameManager(playerOneName.Text, playerTwoName.Text, playerTwoAICheckbox.IsChecked ?? false);
            gameManager.GameFinished += GameManager_GameFinished;

            gameManager.Start();
            this.Hide();
        }

        private void GameManager_GameFinished(object sender, EventArgs e)
        {
            this.Show();
            gameManager = null;
            LoadData();
        }
    }
}
