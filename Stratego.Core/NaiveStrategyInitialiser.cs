using System;
using System.Collections.Generic;
using System.Linq;
using Stratego.Core.Enums;
using Stratego.Core.Pawns;

namespace Stratego.Core
{
    public class NaiveStrategyInitialiser
    {
        private int counterStrategy = new Random().Next(2, 4);
        public void InitialisePieces(Player player, Location location, byte boardWidth, int numPieces, PieceColor pieceColor)
        {
            var rowsNeeded = numPieces / boardWidth;
            var playingPiecesArmyCount = GenerateArmyCountDictionary();
            byte startRow = 0;
            byte startColumn = 0;
            byte flagRowPosition = 0;

            if (location == Location.Bottom)
            {
                startColumn = (byte)(boardWidth - rowsNeeded);
                flagRowPosition = (byte)(boardWidth - (byte)new Random().Next(2, 4));
            }
            else if (location == Location.Top)
            {
                flagRowPosition = (byte)new Random().Next(2, 3);
            }

            CreateLayout(player,
                         boardWidth,
                         startColumn,
                         rowsNeeded,
                         startRow,
                         flagRowPosition,
                         playingPiecesArmyCount,
                         pieceColor);

            counterStrategy++;
        }

        private void CreateLayout(Player player, byte columns, byte startRow, int rows, byte startColumn, byte flagColumnPos,
            Dictionary<string, int> playingPiecesArmyCount, PieceColor pieceColor)
        {
            var rand = new Random();

            var randFlagPosColumn = new Random().Next(1, 9);

            if (counterStrategy % 2 == 0) //Use strategy "The Bomber", surround the flag with bombs, not on diagonals
            {
                for (var row = startRow; row < startRow + rows; row++)
                {
                    for (var column = startColumn; column < columns; column++)
                    {
                        if (row == flagColumnPos && column == randFlagPosColumn)
                        {
                            player.AddPlayingPiece(new Flag(row, column, pieceColor));
                        }   //Place a bomb on the left, right, under & above the Flag
                        else if ((row == flagColumnPos && column == randFlagPosColumn - 1) || (row == flagColumnPos && column == randFlagPosColumn + 1) || (row == flagColumnPos - 1 && column == randFlagPosColumn) || (row == flagColumnPos + 1 && column == randFlagPosColumn))
                        {
                            player.AddPlayingPiece(new Bomb(row, column, pieceColor));
                        }
                        else
                        {
                            player.AddPlayingPiece(SelectPlayingPiece(rand, playingPiecesArmyCount, row, column, pieceColor));
                        }
                    }
                }
            }
            else if (counterStrategy % 2 != 0) //Use strategy "Trumps wall", put 3 bombs horizontally in front of the flag and one bomb behind
            {
                for (var row = startRow; row < startRow + rows; row++)
                {
                    for (var column = startColumn; column < columns; column++)
                    {
                        if (row == flagColumnPos && column == randFlagPosColumn)
                        {
                            player.AddPlayingPiece(new Flag(row, column, pieceColor));
                        }  //Put 3 bombs in front of the flag and one behind
                        else if ((row == flagColumnPos - 1&& column == randFlagPosColumn + 1) || (row == flagColumnPos - 1 && column == randFlagPosColumn) || (row == flagColumnPos - 1 && column == randFlagPosColumn - 1) || (row == flagColumnPos + 1 && column == randFlagPosColumn))
                        {
                            player.AddPlayingPiece(new Bomb(row, column, pieceColor));
                        }
                        else
                        {
                            player.AddPlayingPiece(SelectPlayingPiece(rand, playingPiecesArmyCount, row, column, pieceColor));
                        }
                    }
                }
            }
        }

        private PlayingPiece SelectPlayingPiece(Random rand, Dictionary<string, int> playingPiecesArmyCount, byte row,
            byte column, PieceColor pieceColor)
        {
            KeyValuePair<string, int> pair;
            do
            {
                var armyIndex = rand.Next(playingPiecesArmyCount.Count);
                pair = playingPiecesArmyCount.ElementAt(armyIndex);
            } while (pair.Value == 0);

            playingPiecesArmyCount[pair.Key] = pair.Value - 1;
            return CreatePlayingPiece(pair.Key, row, column, pieceColor);
        }

        private PlayingPiece CreatePlayingPiece(string pieceName, byte row, byte column, PieceColor color)
        {
            PlayingPiece piece = null;
            switch (pieceName)
            {
                case "Marshal":
                    piece = new Marshal(row, column, color);
                    break;
                case "Captain":
                    piece = new Captain(row, column, color);
                    break;
                case "General":
                    piece = new General(row, column, color);
                    break;
                case "Colonel":
                    piece = new Colonel(row, column, color);
                    break;
                case "Major":
                    piece = new Major(row, column, color);
                    break;
                case "Lieutenant":
                    piece = new Lieutenant(row, column, color);
                    break;
                case "Sergeant":
                    piece = new Sergeant(row, column, color);
                    break;
                case "Miner":
                    piece = new Miner(row, column, color);
                    break;
                case "Scout":
                    piece = new Scout(row, column, color);
                    break;
                case "Cavalry":
                    piece = new Cavalry(row, column, color);
                    break;
                case "Spy":
                    piece = new Spy(row, column, color);
                    break;
                case "Bomb":
                    piece = new Bomb(row, column, color);
                    break;
                case "Veteran":
                    piece = new Veteran(row, column, color);
                    break;
            }

            return piece;
        }

        private Dictionary<string, int> GenerateArmyCountDictionary()
        {
            var armyDic = new Dictionary<string, int>();
            armyDic.Add("Marshal", 1);
            armyDic.Add("General", 1);
            armyDic.Add("Colonel", 2);
            armyDic.Add("Major", 3);
            armyDic.Add("Captain", 4);
            armyDic.Add("Lieutenant", 4);
            armyDic.Add("Sergeant", 4);
            armyDic.Add("Miner", 5);
            armyDic.Add("Scout", 5);
            armyDic.Add("Cavalry", 1);
            armyDic.Add("Spy", 1);
            armyDic.Add("Bomb", 2);
            armyDic.Add("Veteran", 2);
            return armyDic;
        }
    }
}