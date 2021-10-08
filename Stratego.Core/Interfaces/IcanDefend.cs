using Stratego.Core.Pawns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Interfaces
{
    public interface ICanDefend
    {
        public void Defend(MovablePlayingPiece opponent);
    }
}
