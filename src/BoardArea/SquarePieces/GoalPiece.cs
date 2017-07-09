using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class GoalPiece : ISquarePiece
    {
        

        public GoalPiece(Board board, int x, int y, Goal goal)
        {
            Board = board;
            Goal = goal;
            Position = new Point(x, y);
            Type = SquareTypes.Goal;
        }

        public Goal Goal { get; set; }
        public Board Board { get; set; }
        public Point Position { get; set; }
        public SquareTypes Type { get; set; }

        public CollisionOptions OnBeforeCollide(Player player, Direction direction)
        {
            return CollisionOptions.SlideInto;
        }

        public CollisionOptions OnAfterCollide(Player player, Direction direction)
        {
            return CollisionOptions.Goal;
        }

        public void Render(ILayer layer)
        {
            IImage goal;
            if (Goal.IsExtra)
            {
                goal = Assets.Images.Tiles.FishTilesExtra;
            }
            else
            {
                if (Goal.GoalSize == GoalSize.Small)
                {
                    goal = Assets.Images.Tiles.Small.FishTiles[Goal.PlayerNumber + 1];
                }
                else
                {
                    goal = Assets.Images.Tiles.Big.FishTiles[Goal.PlayerNumber + 1];
                }
            }

            layer.DrawImage(goal, Position.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2d, Position.Y * BoardConstants.SquareSize + BoardConstants.SquareSize / 2d,
                BoardConstants.SquareSize,
                BoardConstants.SquareSize, true);
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
        }

        public static IImage GetGoalImage(Goal gc)
        {
            if (gc.IsExtra)
            {
                return Assets.Images.Cards.CardsExtra;
            }
            if (gc.GoalSize == GoalSize.Small)
            {
                return Assets.Images.Cards.Small.Cards[gc.PlayerNumber + 1];
            }
            return Assets.Images.Cards.Big.Cards[gc.PlayerNumber + 1];
        }

        public static IImage GetBaseImage(Goal gc)
        {
            if (gc.IsExtra)
            {
                return Assets.Images.Cards.CardsAll;
            }
            return Assets.Images.Cards.Character.Cards[gc.PlayerNumber + 1];
        }
    }
}