using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    public class Ship
    {
        public int Size { get; private set; }
        public bool IsHorizontal { get; set; }

        public List<Cell> cells;

        public bool IsSunk { get { return cells.Count(x => !x.IsHit) == 0; } }

        public Ship(int size)
        {
            if (size < 1 || size > Math.Min(PlayField.ColumnCount, PlayField.RowCount)) throw new ArgumentOutOfRangeException("size", size, "Size is not allowed to be greater than the smallest dimension of the playfield, or to be smaller than 1.");
            Size = size;
            IsHorizontal = true;
            cells = new List<Cell>();
        }

        public void MarkSunk()
        {
            foreach (Cell cell in cells)
            {
                cell.IsSunk = true;
            }
        }

        public bool Contains(Cell cell)
        {
            return cells.Contains(cell);
        }

        public bool Contains(int row, int col)
        {
            return cells.Count(x => x.Row == row && x.Column == col) > 0;
        }

        public static Ship WhichShip(List<Ship> ships, int row, int col)
        {
            foreach (Ship cShip in ships)
            {
                if (cShip.Contains(row, col)) return cShip;
            }
            return null;
        }
    }
}
