using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class WallPiece : ISquarePiece
    {
        private readonly AssetManager assetManager;
        private readonly IRenderer renderer;

        public WallPiece(AssetManager assetManager, Board board, int x, int y, Direction direction)
        {
            this.assetManager = assetManager;
            Board = board;
            Direction = direction;
            Position = new Point(x, y);
            Type = SquareTypes.Wall;
        }

        public Direction Direction { get; set; }
        public SquareTypes Type { get; set; }
        public Board Board { get; set; }

        public Point Position { get; set; }

        public CollisionOptions OnBeforeCollide(Player player, Direction direction)
        {
            switch (Direction)
            {
                case Direction.Left:
                    if (direction == Direction.Right)
                        return CollisionOptions.Stall;
                    break;
                case Direction.Right:
                    if (direction == Direction.Left)
                        return CollisionOptions.Stall;
                    break;
                case Direction.Top:
                    if (direction == Direction.Bottom)
                        return CollisionOptions.Stall;
                    break;
                case Direction.Bottom:
                    if (direction == Direction.Top)
                        return CollisionOptions.Stall;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return CollisionOptions.SlideInto;
        }

        public CollisionOptions OnAfterCollide(Player player, Direction direction)
        {
            if (direction == Direction)
                return CollisionOptions.Stall;
            return CollisionOptions.SlideOutOf;
        }

        public void Render(ILayer layer)
        {
            IImage wall;

            double xPos = 0d;
            double yPos = 0d;


            double width, height;

            switch (Direction)
            {
                case Direction.Left:
                    wall = assetManager.GetImage(Images.Tiles.WallVertical);
                    xPos = -BoardConstants.SquareSize/2d;
                    width = wall.Width;
                    height = BoardConstants.SquareSize;
                    break;
                case Direction.Right:
                    wall = assetManager.GetImage(Images.Tiles.WallVertical);
                    xPos = +BoardConstants.SquareSize/2d;
                    width = wall.Width;
                    height = BoardConstants.SquareSize;
                    break;
                case Direction.Top:
                    wall = assetManager.GetImage(Images.Tiles.WallHorizontal);
                    yPos = -BoardConstants.SquareSize/2d;
                    width = BoardConstants.SquareSize;
                    height = wall.Height;
                    break;
                case Direction.Bottom:
                    wall = assetManager.GetImage(Images.Tiles.WallHorizontal);
                    yPos = +BoardConstants.SquareSize/2d;
                    width = BoardConstants.SquareSize;
                    height = wall.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            layer.DrawImage(wall, Position.X*BoardConstants.SquareSize + BoardConstants.SquareSize/2d + xPos,
                Position.Y*BoardConstants.SquareSize + BoardConstants.SquareSize/2d + yPos,
                width,
                height, true);
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
        }
    }
}