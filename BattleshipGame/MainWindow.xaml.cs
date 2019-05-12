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
        public MainWindow()
        {
            InitializeComponent();
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
        }
    }
}
