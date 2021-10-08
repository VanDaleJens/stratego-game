using System;
using System.Collections.Generic;
using Stratego.Core.Enums;
using Stratego.Core.Interfaces;
using Stratego.Core.Pawns;
using System.Linq;

namespace Stratego.Core
{
    public class GameBoard
    {
        private const byte BOARD_WIDTH = 10;
        private const byte MAX_PIECES_PER_PLAYER = 40;
        private readonly PlayerNPC npc;
        private readonly Player player;
        public List<string> BattleLog { get; protected set; }
        public bool GameEnded { get; set; }
        public string WinnerText { get; protected set; }
        public bool isPlayerTurn;
        private int turnCounter = 1;

        public GameBoard(PieceColor player1Color, PieceColor player2Color)
        {
            player = new Player(MAX_PIECES_PER_PLAYER);
            npc = new PlayerNPC(MAX_PIECES_PER_PLAYER);

            BattleLog = new List<string>();
            InitBoard(player1Color, player2Color);
            GameEnded = false;
        }

        private void InitBoard(PieceColor player1Color, PieceColor player2Color)
        {
            var naiveStrategyInitialiser = new NaiveStrategyInitialiser();
            npc.RemoveAllPlayingPieces();
            naiveStrategyInitialiser.InitialisePieces(npc, Location.Top, BOARD_WIDTH, MAX_PIECES_PER_PLAYER, player2Color);

            player.RemoveAllPlayingPieces();
            naiveStrategyInitialiser.InitialisePieces(player, Location.Bottom, BOARD_WIDTH, MAX_PIECES_PER_PLAYER, player1Color);
        }

        public PlayingPiece[,] GetPlayingField()
        {
            var pieces = new PlayingPiece[BOARD_WIDTH, BOARD_WIDTH];

            AddToField(pieces, player.GetActivePlayingPieces());
            AddToField(pieces, npc.GetActivePlayingPieces());

            return pieces;
        }

        private void AddToField(PlayingPiece[,] board, PlayingPiece[] newPieces)
        {
            foreach (var piece in newPieces)
            {
                board[piece.Row, piece.Column] = piece;
            }
        }

        public bool HasMovablePieceOn(byte row, byte column)
        {
            PlayingPiece piece = GetPlayingPiece(row, column);
            return piece != null && piece is MovablePlayingPiece;
        }

        private PlayingPiece GetPlayingPiece(byte row, byte column)
        {
            var currentPieces = npc.GetActivePlayingPieces();
            if (isPlayerTurn) currentPieces = player.GetActivePlayingPieces();

            foreach (var piece in currentPieces)
            {
                if (piece.Row == row && piece.Column == column) return piece;
            }

            return null;
        }

        private bool CanMove(PlayingPiece p, byte row, byte column)
        {
            var horizontalMovement = p.Column - column;
            var verticalMovement = p.Row - row;
            var absHorizontalDistance = Math.Abs(horizontalMovement);
            var absVerticalDistance = Math.Abs(verticalMovement);

            if (absHorizontalDistance > 0 && absVerticalDistance > 0)
            {
                return false;
            }

            var direction = CalculateDirection(p, row, column);

            var steps = CalculateSteps(p, row, column, direction);

            return GetPlayingPiece(p.Row, p.Column) != null &&
                   p is MovablePlayingPiece &&
                   (steps <= 1 || p is ICanMoveMultipleSteps) &&
                   HasNoObstacles(p, direction, steps);
        }

        public bool CanMove(byte pieceRow, byte pieceColumn, byte row, byte column)
        {
            return CanMove(GetPlayingPiece(pieceRow, pieceColumn), row, column);
        }

        private void MovePiece(PlayingPiece p, byte row, byte column)
        {
            var currentMovablePiece = (MovablePlayingPiece)p;
            var direction = CalculateDirection(p, row, column);

            switch (p)
            {
                case ICanMoveMultipleSteps multipleSteps:
                    var steps = CalculateSteps(p, row, column, direction);
                    multipleSteps.MovePiece(direction, steps);
                    break;
                default:
                    currentMovablePiece.MovePiece(direction);
                    break;
            }

            AttackIfPossible(currentMovablePiece);
        }

        public void MovePiece(byte pieceRow, byte pieceColumn, byte row, byte column)
        {
            MovePiece(GetPlayingPiece(pieceRow, pieceColumn), row, column);
            turnCounter++;
            if (turnCounter % 2 == 0) isPlayerTurn = true;
            else isPlayerTurn = false;
        }

        private Direction CalculateDirection(PlayingPiece p, byte row, byte column)
        {
            var rowMovement = p.Row - row;
            var columnMovement = p.Column - column;
            Direction direction;

            if (rowMovement > 0)
            {
                direction = Direction.Up;
            }
            else if (rowMovement < 0)
            {
                direction = Direction.Down;
            }
            else if (columnMovement > 0)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = Direction.Right;
            }

            return direction;
        }

        private byte CalculateSteps(PlayingPiece piece, byte row, byte column, Direction direction)
        {
            var horizontalMovement = piece.Column - column;
            var verticalMovement = piece.Row - row;
            var absHorizontalDistance = Math.Abs(horizontalMovement);
            var absVerticalDistance = Math.Abs(verticalMovement);

            byte steps;
            if (direction == Direction.Left || direction == Direction.Right)
            {
                steps = (byte)absHorizontalDistance;
            }
            else
            {
                steps = (byte)absVerticalDistance;
            }

            return steps;
        }

        private bool HasNoObstacles(PlayingPiece playingPiece, Direction direction, int steps) // moet ook false worden
        {
            var validMove = true;


            if (direction == Direction.Down || direction == Direction.Up)
            {
                validMove &= HasNoVerticalObstacles(playingPiece.Row, playingPiece.Column, direction, steps);
            }
            else
            {
                validMove &= HasNoHorizontalObstacles(playingPiece.Row, playingPiece.Column, direction, steps);
            }

            return validMove;
        }

        private bool HasNoHorizontalObstacles(byte playingPieceRow, byte playingPieceColumn, Direction direction, int steps)//Als water is hier false returnen
        {
            var playingPieces = GetPlayingField();

            var validMove = true;
            var directionIndex = direction == Direction.Right ? 1 : -1; //rechts of links


            for (var i = 1; i <= steps; i++)
            {
                if (playingPieceRow == 4 || playingPieceRow == 5)
                {

                    if (playingPieceColumn + (i * directionIndex) == 2 || playingPieceColumn + (i * directionIndex) == 3 || playingPieceColumn + (i * directionIndex) == 6 || playingPieceColumn + (i * directionIndex) == 7) 
                    {
                        if (GetPlayingPiece(playingPieceRow, playingPieceColumn) is Cavalry)
                        {
                            validMove = true;
                        }
                        else 
                        {
                            validMove = false;
                        }
                    } 
                }


                if (i == steps)
                {
                    validMove &= GetPlayingPiece(playingPieceRow, (byte)(playingPieceColumn + i * directionIndex)) == null;
                }
                else
                {
                    validMove &= playingPieces[playingPieceRow, playingPieceColumn + i * directionIndex] == null; //retourneert veld zoals hij is als de steps niet correct zijn
                }
            }
            return validMove;
        }

        private bool HasNoVerticalObstacles(byte playingPieceRow, byte playingPieceColumn, Direction direction, int steps)//Als water is hier false returnen
        {
            var playingPieces = GetPlayingField();

            var validMove = true;
            var directionIndex = direction == Direction.Up ? -1 : 1;//boven of onder

            for (var i = 1; i <= steps; i++)
            {
                if (playingPieceColumn == 2 || playingPieceColumn == 3 || playingPieceColumn == 6 || playingPieceColumn == 7)
                {
                    if (playingPieceRow + (i * directionIndex) == 4 || playingPieceRow + (i * directionIndex) == 5) 
                    {
                        if (GetPlayingPiece(playingPieceRow, playingPieceColumn) is Cavalry)
                        {
                            validMove = true;
                        }
                        else
                        {
                            validMove = false;
                        }
                    } 
                }
                if (i == steps)
                {
                    validMove &= GetPlayingPiece((byte)(playingPieceRow + i * directionIndex), playingPieceColumn) == null;
                }
                else
                {
                    validMove &= playingPieces[playingPieceRow + i * directionIndex, playingPieceColumn] == null; //retourneert veld zoals hij is als de steps niet correct zijn
                }
            }

            return validMove;
        }

        private void AttackIfPossible(MovablePlayingPiece p)
        {
            var opponent = npc.GetPiece(p.Row, p.Column);
            if (!isPlayerTurn) opponent = player.GetPiece(p.Row, p.Column); // if ispalyer == not true

            BattleLog.Clear();

            if (opponent != null)
            {
                p.Attack(opponent);

                if (p.IsCaptured && opponent.IsCaptured == false) UpdateBattleLog(opponent, p);
                else if (opponent.IsCaptured && p.IsCaptured == false) UpdateBattleLog(p, opponent);
                else if (opponent.IsCaptured && p.IsCaptured)
                {
                    UpdateBattleLog(p, opponent);
                    UpdateBattleLog(opponent, p);
                }
            }
        }

        public List<string> UpdateBattleLog(PlayingPiece player1, PlayingPiece player2)
        {
            BattleLog.Add($"{player1.Color} {player1.Name} captured {player2.Color} {player2.Name}");

            return BattleLog;
        }

        public void CheckWinner()
        {

            PlayingPiece[] allPiecesPlayer = player.GetActivePlayingPieces();
            PlayingPiece[] allPiecesNPC = npc.GetActivePlayingPieces();


            List<MovablePlayingPiece> totalMovePieces = new List<MovablePlayingPiece>();

            string loser = "";
            string winner = "";

            bool containsFlagPlayer = allPiecesPlayer.Any(piece => piece is Flag);
            bool containsFlagNPC = allPiecesNPC.Any(piece => piece is Flag);

            // When flag is captured

            if (!containsFlagPlayer)
            {
                WinnerText = $"{npc.Name} has captured the flag and is the winner! \n" +
                $"Please click New Game to start a new game of Stratego.";
                GameEnded = true;
                return;
            }
            if (!containsFlagNPC)
            {
                WinnerText = $"{player.Name} has captured the flag and is the winner! \n" +
                $"Please click New Game to start a new game of Stratego.";
                GameEnded = true;
                return;
            }

            // When all MoveablePlayingPiece are captured

            foreach (PlayingPiece movePiece in allPiecesPlayer)
            {
                if (movePiece is MovablePlayingPiece)
                {
                    totalMovePieces.Add((MovablePlayingPiece)movePiece);
                }
            }
            if (totalMovePieces.Count == 0)
            {
                loser = player.Name;
                winner = npc.Name;

                GameEnded = true;
            }
            totalMovePieces.Clear();

            foreach (PlayingPiece movePiece in allPiecesNPC)
            {
                if (movePiece is MovablePlayingPiece)
                {
                    totalMovePieces.Add((MovablePlayingPiece)movePiece);
                }
            }
            if (totalMovePieces.Count == 0)
            {
                loser = npc.Name;
                winner = player.Name;

                GameEnded = true;
            }

            WinnerText = $"The winner is {winner} because {loser} has no movable pieces anymore! \n" +
                $"Please click New Game to start a new game of Stratego.";
        }
    }
}



