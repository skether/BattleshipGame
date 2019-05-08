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

        PlayField field;

        public MainWindow()
        {
            InitializeComponent();
            SetupGameField();
        }

        private void SetupGameField()
        {
            field = new PlayField(FieldCanvas);
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Cell cell) cell.IsHit = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}