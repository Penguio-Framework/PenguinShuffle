using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class BorderWallPiece : ISquarePiece
    {
        public BorderWallPiece(int x, int y)
        {
            Board = null;
            Position = new Point(x, y);
            Type = SquareTypes.BorderWall;
        }

        public Board Board { get; set; }
        public Point Position { get; set; }
        public SquareTypes Type { get; set; }

        public CollisionOptions OnBeforeCollide(Player player, Direction direction)
        {
            return CollisionOptions.Stall;
        }

        public CollisionOptions OnAfterCollide(Player player, Direction direction)
        {
            return CollisionOptions.Stall;
        }

        public void Render(ILayer layer)
        {
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
        }
    }

    public enum SquareTypes
    {
        Goal = 0,
        Player = 1,
        SolidWall = 2,
        Wall = 3,
        BorderWall = 4
    }
}