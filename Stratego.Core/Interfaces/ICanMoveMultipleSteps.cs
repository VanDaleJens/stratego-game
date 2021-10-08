using Stratego.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratego.Core.Interfaces
{
    public interface ICanMoveMultipleSteps
    {
        public void MovePiece(Direction direction, byte steps );
    }
}
