using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Sergeant : MovablePlayingPiece
    {
        public Sergeant(byte row, byte column, PieceColor color) : base("Sergeant", row, column, 7, color)
        {
        }
    }
}
