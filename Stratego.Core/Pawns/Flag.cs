using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Flag : PlayingPiece
    {
        public Flag(byte row, byte column, PieceColor color) : base("Flag", row, column, 11, color)
        {
        }
    }
}
