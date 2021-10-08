using Stratego.Core.Enums;
using Stratego.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public abstract class MovablePlayingPiece : PlayingPiece, ICanAttack
    {
        public MovablePlayingPiece(string name, byte row, byte column, byte rank, PieceColor color) : base(name, row, column, rank, color)
        {
        }

        public virtual void Attack(PlayingPiece opponent)
        {
            opponent.Defend(this);
            IsCaptured = !opponent.IsCaptured || opponent.Rank == this.Rank;
        }

        public void MovePiece(Direction direction)
        {
            MovePiece(direction, 1);
        }

        public void MovePiece(Direction direction, byte steps)
        {
            switch (direction)
            {
                case Direction.Up:
                    Row -= steps;
                    break;
                case Direction.Right:
                    Column += steps;
                    break;
                case Direction.Down:
                    Row += steps;
                    break;
                case Direction.Left:
                    Column -= steps;
                    break;
            }
        }
    }
}
