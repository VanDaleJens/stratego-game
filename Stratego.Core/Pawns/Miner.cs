using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Miner : MovablePlayingPiece
    {
        public Miner(byte row, byte column, PieceColor color) : base("Miner", row, column, 8, color)
        {

        }
    }
}
