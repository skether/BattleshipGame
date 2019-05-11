using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    class Ship
    {
        public int Size { get; private set; }
        public bool IsHorizontal { get; set; }

        public List<Cell> cells;

        public Ship(int size)
        {
            if (size < 1 || size > Math.Min(PlayField.ColumnCount, PlayField.RowCount)) throw new ArgumentOutOfRangeException("size", size, "Size is not allowed to be greater than the smallest dimension of the playfield, or to be smaller than 1.");
            Size = size;
            IsHorizontal = true;
            cells = new List<Cell>();
        }
    }
}
