using System;
using System.Collections.Generic;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class Board
    {
        public PuzzleBoardInfo BoardInfo { get; set; }
        
        public GameService GameService { get; set; }
        public IBoardBuilder BoardBuilder { get; set; }

        public Board( GameService gameService, PuzzleBoardInfo boardInfo = null)
        {
            BoardInfo = boardInfo;
            GameService = gameService;
        }

        public PlayerPosition MoveToPosition { get; set; }
        public List<ISquarePiece> SquarePieces { get; set; }
        public List<Player> Players { get; set; }

        public bool IsMoving
        {
            get { return MoveToPosition != null; }
        }


        public void Init()
        {
            SquarePieces = new List<ISquarePiece>();
            Players = new List<Player>();
        }

        private void BuildBoard()
        {
            if (BoardInfo != null)
            {
                BoardInfo.BuildBoard(this);
            }
            else
            {
                BoardBuilder = new RicochetBoardBuilder( GameService);
                BoardBuilder.BuildBoard(this);
            }
        }


        public IEnumerable<ISquarePiece> getItemsOnBoard(Player currentPlayer, int x, int y)
        {
            foreach (var squarePiece in SquarePieces)
            {
                if (squarePiece.Position.X == x && squarePiece.Position.Y == y)
                {
                    yield return squarePiece;
                }
            }
            foreach (var player in Players)
            {
                if (currentPlayer == player) continue;
                var playerSquare = new PlayerPiece(player.PlayerNumber, player.Position.X, player.Position.Y);

                var playerPosition = MoveToPosition;
                if (playerPosition != null && player == playerPosition.Character)
                {
                    playerSquare.Position.X = playerPosition.X;
                    playerSquare.Position.Y = playerPosition.Y;
                }

                if (player.Position.X == x && player.Position.Y == y)
                {
                    yield return playerSquare;
                }
            }
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            foreach (var squarePiece in SquarePieces)
            {
                squarePiece.Tick(elapsedGameTime);
            }
            foreach (var player in Players)
            {
                player.Tick(elapsedGameTime);
            }
        }

        public void ProcessMovement(Movement movement)
        {
            int x = movement.Player.Position.X, y = movement.Player.Position.Y;

            if (MoveToPosition != null && MoveToPosition.Character == movement.Player)
            {
                x = MoveToPosition.X;
                y = MoveToPosition.Y;
            }


            var currentDirection = movement.Direction;
            var done = false;


            foreach (var squarePiece in getItemsOnBoard(movement.Player, x, y))
            {
                switch (squarePiece.OnAfterCollide(movement.Player, currentDirection))
                {
                    case CollisionOptions.Stall:
                        return;
                }
            }


            while (!done)
            {
                switch (currentDirection)
                {
                    case Direction.Left:
                        x -= 1;
                        break;
                    case Direction.Right:
                        x += 1;
                        break;
                    case Direction.Top:
                        y -= 1;
                        break;
                    case Direction.Bottom:
                        y += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                var itemsOnBoard = getItemsOnBoard(movement.Player, x, y).ToArray();
                foreach (var squarePiece in itemsOnBoard)
                {
                    switch (squarePiece.OnBeforeCollide(movement.Player, currentDirection))
                    {
                        case CollisionOptions.Stall:
                            done = true;
                            break;
                    }
                }
                if (done) break;
                MoveToPosition = (new PlayerPosition(movement.Player, x, y, currentDirection));


                foreach (var squarePiece in itemsOnBoard)
                {
                    switch (squarePiece.OnAfterCollide(movement.Player, currentDirection))
                    {
                        case CollisionOptions.SlideOutOf:
                            break;
                        case CollisionOptions.Goal:
                            done = true;
                            break;
                        case CollisionOptions.Stall:
                            done = true;
                            break;
                    }
                }
            }
        }

        public bool isSurrounding(int x, int y, int depth)
        {
            var good = false;
            for (var sx = -depth; sx <= depth; sx++)
            {
                for (var sy = -depth; sy <= depth; sy++)
                {
                    good = good || getItemsOnBoard(null, x + sx, y + sy).Any(a => a is SolidWallPiece || a is WallPiece);
                }
            }

            return good;
        }

        public void Render(TimeSpan elapsedGameTime, ILayer layer, Action<ILayer, int, int> boxDrawn = null, bool disabled = false)
        {
            //            layer.Clear();
            layer.Save();


            var bg = Assets.Images.Layouts.GameBackground;
            layer.Translate(BoardConstants.SideOffset, BoardConstants.TopOffset);
            layer.DrawImage(bg, 0, 0, BoardConstants.SquareSize * BoardConstants.SquareWidth, BoardConstants.SquareSize * BoardConstants.SquareHeight);

            var image = Assets.Images.Board.Box;
            for (var x = 0; x < BoardConstants.SquareWidth; x++)
            {
                for (var y = 0; y < BoardConstants.SquareHeight; y++)
                {
                    if (boxDrawn != null)
                        boxDrawn(layer, x, y);

                    layer.DrawImage(image, x * BoardConstants.SquareSize + BoardConstants.SquareSize / 2d, y * BoardConstants.SquareSize + BoardConstants.SquareSize / 2d, BoardConstants.SquareSize, BoardConstants.SquareSize, true);
                }
            }


            foreach (var squarePiece in SquarePieces)
            {
                squarePiece.Render(layer);
            }
            if (!disabled)
            {
                foreach (var player in Players)
                {
                    player.Render(layer);
                }
            }
             
            layer.Restore();
        }

        public Player GetPlayerAtPoint(int x, int y)
        {
            foreach (var player in Players)
            {
                if (player.Position.X == x && player.Position.Y == y) return player;
            }
            return null;
        }

        public void StartGame()
        {
            Init();
            BuildBoard();
        }


        public void AddPlayer(int x, int y, int characterNumber)
        {
            BoardBuilder.AddPlayer(x, y, characterNumber);
        }
    }
}