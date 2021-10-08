using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Major : MovablePlayingPiece
    {
        public Major(byte row, byte column, PieceColor color) : base("Major", row, column, 4, color)
        {
        }
    }
}
