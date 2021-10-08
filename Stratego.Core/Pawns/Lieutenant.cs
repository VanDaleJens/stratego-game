using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Lieutenant : MovablePlayingPiece
    {
        public Lieutenant(byte row, byte column, PieceColor color) : base("Lieutenant", row, column, 6, color)
        {
        }
    }
}
