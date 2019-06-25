using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BattleshipGame
{
    public class ArtificialPlayer : Player
    {
        public ArtificialPlayer(int id, string name) : base(id, name) { }

        public override void Start()
        {
        }

        protected override void ActTurn()
        {
        }
    }
}