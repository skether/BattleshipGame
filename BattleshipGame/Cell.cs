using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BattleshipGame
{
    internal class Cell : Shape
    {
        public static Brush borderColor = Brushes.Black;
        public static Brush waterColor = Brushes.LightBlue;
        public static Brush waterHitColor = Brushes.Blue;
        public static Brush shipColor = Brushes.LightGray;
        public static Brush shipHitColor = Brushes.Red;
        public static Brush highlightColor = Brushes.LimeGreen;

        public int Row { get; set; }
        public int Column { get; set; }

        private bool _isHighlighted;
        public bool IsHighlighted { get { return _isHighlighted; } set { _isHighlighted = value; UpdateFill(); } }

        private bool _isShip;
        public bool IsShip { get { return _isShip; } set { _isShip = value; UpdateFill(); } }

        private bool _isHit;
        public bool IsHit { get { return _isHit; } set { _isHit = value; UpdateFill(); } }

        private RectangleGeometry _geometry;
        protected override Geometry DefiningGeometry { get { return _geometry; } }

        public Cell(int row, int column)
        {
            this.Stroke = borderColor;
            this.StrokeThickness = 1;
            this.Fill = waterColor;

            this.Row = row;
            this.Column = column;

            this._geometry = new RectangleGeometry();
            _geometry.Rect = new System.Windows.Rect(0, 0, this.Width, this.Height);

            this.SizeChanged += Cell_SizeChanged;
            this.MouseEnter += Cell_MouseEnter;
            this.MouseLeave += Cell_MouseLeave;
        }

        private void Cell_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            UpdateSize();
        }

        private void Cell_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsHighlighted = true;
        }

        private void Cell_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsHighlighted = false;
        }

        public void UpdateSize()
        {
            _geometry.Rect = new System.Windows.Rect(0, 0, this.Width, this.Height);
        }

        private void UpdateFill()
        {
            if(IsHighlighted)
            {
                this.Fill = highlightColor;
            }
            else if(IsShip)
            {
                this.Fill = IsHit ? shipHitColor : shipColor;
            }
            else
            {
                this.Fill = IsHit ? waterHitColor : waterColor;
            }
        }
    }
}
