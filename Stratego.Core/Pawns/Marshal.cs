using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Marshal : MovablePlayingPiece
    {
        public Marshal(byte row, byte column, PieceColor color) : base("Marshal", row, column, 1, color)
        {
        }

        public override void Defend(MovablePlayingPiece opponent)
        {
            if (opponent is Spy) IsCaptured = true;
            else base.Defend(opponent);
        }
    }
}
