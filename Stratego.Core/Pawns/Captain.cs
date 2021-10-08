using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Captain : MovablePlayingPiece
    {
        public Captain(byte row, byte column, PieceColor color) : base("Captain", row, column, 5, color)
        {
        }
    }
}
