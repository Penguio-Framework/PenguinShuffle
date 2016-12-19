using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public interface ISquarePiece
    {
        Board Board { get; set; }
        Point Position { get; set; }
        SquareTypes Type { get; set; }
        CollisionOptions OnBeforeCollide(Player player, Direction direction);
        CollisionOptions OnAfterCollide(Player player, Direction direction);
        void Render(ILayer layer);
        void Tick(TimeSpan elapsedGameTime);
    }
}