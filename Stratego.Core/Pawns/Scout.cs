using Stratego.Core.Enums;
using Stratego.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Pawns
{
    public class Scout : MovablePlayingPiece, ICanMoveMultipleSteps
    {
        public Scout(byte row, byte column, PieceColor color) : base("Scout", row, column, 9, color)
        {
        }

    }
}
