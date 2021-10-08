using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class General : MovablePlayingPiece
    {
        public General(byte row, byte column, PieceColor color) : base("General", row, column, 2, color)
        {
        }
    }
}
