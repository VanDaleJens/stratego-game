using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Cavalry : MovablePlayingPiece
    {
        public Cavalry(byte row, byte column, PieceColor color) : base("Cavalry", row, column, 9, color)
        {

        }
    }
}
