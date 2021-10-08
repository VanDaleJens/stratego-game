using Stratego.Core.Pawns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Interfaces
{
    public interface ICanAttack
    {
        public void Attack(PlayingPiece opponent);
    }
}
