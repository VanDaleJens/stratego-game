using System;
using System.Collections.Generic;
using Stratego.Core.Pawns;

namespace Stratego.Core
{
    public class Player
    {
        private readonly int maxPieces;

        private readonly List<PlayingPiece> pieces;

        public Player(int maxPieces = 40)
        {
            pieces = new List<PlayingPiece>();
            this.maxPieces = maxPieces;
        }

        public string Name { get; } = "Player1";

        public void AddPlayingPiece(PlayingPiece piece)
        {
            if (pieces.Count == maxPieces)
            {
                throw new InvalidOperationException();
            }

            pieces.Add(piece);
        }

        public void RemoveAllPlayingPieces()
        {
            pieces.Clear();
        }

        public PlayingPiece[] GetActivePlayingPieces()
        {
            List<PlayingPiece> alivePieces = new List<PlayingPiece>();
            foreach (PlayingPiece piece in pieces){
                if(!piece.IsCaptured)
                {
                    alivePieces.Add(piece);
                }
            }
            return alivePieces.ToArray();
        }

        public PlayingPiece GetPiece(byte row, byte column)
        {
            foreach (var piece in pieces)
            {
                if (piece.Row == row && piece.Column == column && !piece.IsCaptured)
                {
                    return piece;
                }
            }

            return null;
        }
    }
}