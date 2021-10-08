using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Veteran : MovablePlayingPiece
    {
        public Veteran(byte row, byte column, PieceColor color) : base("Veteran", row, column, 1, color)
        {
        }

        public override void Defend(MovablePlayingPiece opponent)
        {
            IsCaptured = Rank >= opponent.Rank;
            if (!IsCaptured && Rank < 9) Rank++;
        }

        public override void Attack(PlayingPiece opponent)
        {
            opponent.Defend(this);
            IsCaptured = !opponent.IsCaptured || opponent.Rank == this.Rank;
            if (!IsCaptured && Rank < 9) Rank++;
        }
    }
}
