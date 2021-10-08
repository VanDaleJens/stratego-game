using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Bomb : PlayingPiece
    {
        public Bomb(byte row, byte column, PieceColor color) : base("Bomb", row, column, 0, color)
        {

        }

        public override void Defend(MovablePlayingPiece opponent)
        {
            IsCaptured = opponent is Miner;
        }
    }
}