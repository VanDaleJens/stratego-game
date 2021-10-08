using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Spy : MovablePlayingPiece
    {
        public Spy(byte row, byte column, PieceColor color) : base("Spy", row, column, 10, color)
        {
        }
    }
}
