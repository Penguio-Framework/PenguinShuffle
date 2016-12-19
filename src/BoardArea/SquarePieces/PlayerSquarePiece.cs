using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class PlayerPiece : ISquarePiece
    {
        public PlayerPiece(int playerNumber, int x, int y)
        {
            PlayerNumber = playerNumber;
            Type = SquareTypes.Player;
            Position = new Point(x, y);
        }

        public int PlayerNumber { get; set; }

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
}