using System;
using System.Collections.Generic;
using Engine.Interfaces;
using PenguinShuffle.Utils;

namespace PenguinShuffle.BoardArea
{
    internal class RicochetBoardBuilder : IBoardBuilder
    {
        private readonly AssetManager assetManager;

        private Board board;

        public RicochetBoardBuilder(AssetManager assetManager, GameService gameService)
        {
            GameService = gameService;
            this.assetManager = assetManager;
        }

        public GameService GameService { get; set; }

        public void BuildBoard(Board _board)
        {
            board = _board;
            board.SquarePieces = new List<ISquarePiece>();

            buildBorder();
            buildCenter();
            buildSideWalls();
            buildInner();
        }


        public void AddPlayer(int x, int y, int characterNumber)
        {
            board.Players.Add(new Player(assetManager, board, characterNumber + 1, x, y));
        }

        private void buildInner()
        {
            var hs = new List<int>();
            var addedPieces = new List<ISquarePiece>();

            int totalGoals = 0;
            int tries = 0;
            while (totalGoals < 13)
            {
                if (tries > 5)
                {
                    hs = new List<int>();
                    totalGoals = 0;
                    tries = 0;

                    foreach (ISquarePiece squarePiece in addedPieces)
                        board.SquarePieces.Remove(squarePiece);

                    addedPieces = new List<ISquarePiece>();
                }
                List<int> range = NumberUtils.Shuffle(NumberUtils.MakeRange(1, BoardConstants.SquareWidth - 1));
                tries++;
                foreach (int nX in range)
                {
                    int nY = RandomUtil.RandomInt(1, BoardConstants.SquareHeight - 1);

                    if (hs.Contains(nY))
                    {
                        if (RandomUtil.RandomPercentUnder(85))
                        {
                            continue;
                        }
                    }

                    if (board.isSurrounding(nX, nY, 1))
                    {
                        continue;
                        /*           if (RandomUtil.RandomPercentUnder(85))
                        {
                            continue;
                        }
                        if (board.isSurrounding(nX, nY, 0)) continue;*/
                    }

                    hs.Add(nY);


                    if (RandomUtil.RandomPercentUnder(80))
                    {
                        Direction direction;
                        switch (RandomUtil.RandomInt(0, 4))
                        {
                            case 0:
                                direction = Direction.Left;
                                break;
                            case 1:
                                direction = Direction.Right;
                                break;
                            case 2:
                                direction = Direction.Top;
                                break;
                            case 3:
                                direction = Direction.Bottom;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("random");
                        }
                        Direction adjacent = direction.GetAdjacent();

                        var g = new GoalPiece(assetManager, board, nX, nY, GameService.ClassicGameState.Goals[totalGoals++]);
                        addedPieces.Add(g);
                        board.SquarePieces.Add(g);

                        var wall1 = new WallPiece(assetManager, board, nX, nY, direction);
                        var wall2 = new WallPiece(assetManager, board, nX, nY, adjacent);
                        addedPieces.Add(wall1);
                        addedPieces.Add(wall2);
                        board.SquarePieces.Add(wall1);
                        board.SquarePieces.Add(wall2);
                    }

                    if (totalGoals == 13) break;
                }
            }


            int count = 0;
            tries = 0;
            int totalSolidWalls = RandomUtil.RandomInt(3, 5);
            while (count < totalSolidWalls)
            {
                if (tries++ > 500) break;
                int nX = RandomUtil.RandomInt(1, BoardConstants.SquareWidth - 1);
                int nY = RandomUtil.RandomInt(1, BoardConstants.SquareHeight - 1);


                if (board.isSurrounding(nX, nY, 1))
                {
                    continue;
                }

                count++;

                ISquarePiece sw = new SolidWallPiece(assetManager, board, nX, nY);
                addedPieces.Add(sw);
                board.SquarePieces.Add(sw);
            }
        }


        private void buildBorder()
        {
            for (int x = 0; x < BoardConstants.SquareWidth; x++)
            {
                board.SquarePieces.Add(new BorderWallPiece(x, -1));
                board.SquarePieces.Add(new BorderWallPiece(x, BoardConstants.SquareHeight));
            }

            for (int y = 0; y < BoardConstants.SquareHeight; y++)
            {
                board.SquarePieces.Add(new BorderWallPiece(-1, y));
                board.SquarePieces.Add(new BorderWallPiece(BoardConstants.SquareWidth, y));
            }
        }

        private void buildCenter()
        {
            board.SquarePieces.Add(new SolidWallPiece(assetManager, board, BoardConstants.SquareWidth/2 - 1,
                BoardConstants.SquareHeight/2 - 1));
            board.SquarePieces.Add(new SolidWallPiece(assetManager, board, BoardConstants.SquareWidth/2 - 1,
                BoardConstants.SquareHeight/2));
            board.SquarePieces.Add(new SolidWallPiece(assetManager, board, BoardConstants.SquareWidth/2,
                BoardConstants.SquareHeight/2 - 1));
            board.SquarePieces.Add(new SolidWallPiece(assetManager, board, BoardConstants.SquareWidth/2,
                BoardConstants.SquareHeight/2));
        }

        private void buildSideWalls()
        {
            for (int wallSide = 0; wallSide < 4; wallSide++)
            {
                for (int wallNumber = 0; wallNumber < 2; wallNumber++)
                {
                    int randomX;
                    int randomY;
                    Direction direction;
                    switch (wallSide)
                    {
                        case 0:
                            randomX = RandomUtil.RandomInt(0, BoardConstants.SquareWidth - 6) + 3;
                            randomY = 0;

                            direction = Direction.Left;
                            break;
                        case 1:
                            randomX = BoardConstants.SquareWidth - 1;
                            randomY = RandomUtil.RandomInt(0, BoardConstants.SquareHeight - 6) + 3;
                            direction = Direction.Top;

                            break;
                        case 2:
                            randomX = RandomUtil.RandomInt(0, BoardConstants.SquareWidth - 6) + 3;
                            randomY = BoardConstants.SquareHeight - 1;
                            direction = Direction.Left;

                            break;
                        case 3:
                            randomX = 0;
                            randomY = RandomUtil.RandomInt(0, BoardConstants.SquareHeight - 6) + 3;
                            direction = Direction.Top;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (board.isSurrounding(randomX, randomY, 2))
                    {
                        wallNumber--;
                        continue;
                    }
                    board.SquarePieces.Add(new WallPiece(assetManager, board, randomX, randomY, direction));
                }
            }
        }
    }
}