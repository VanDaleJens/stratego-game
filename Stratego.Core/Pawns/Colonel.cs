using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Colonel : MovablePlayingPiece
    {
        public Colonel(byte row, byte column, PieceColor color) : base("Colonel", row, column, 3, color)
        {
        }
    }
}
