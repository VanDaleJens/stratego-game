using Stratego.Core.Enums;
using Stratego.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public abstract class PlayingPiece : ICanDefend
    {
        public string Name { get; }
        public byte Rank { get; protected set; }
        public PieceColor Color { get; }
        public byte Row { get; protected set; }
        public byte Column { get; protected set; }
        public bool IsCaptured { get; protected set; }

        public PlayingPiece(string name, byte row, byte column, byte rank, PieceColor color)
        {
            Name = name;
            Row = row;
            Column = column;
            Rank = rank;
            Color = color;
        }

        public virtual void Defend(MovablePlayingPiece opponent)
        {
            IsCaptured = Rank >= opponent.Rank;
        }
    }
}
