using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class GoalPiece : ISquarePiece
    {
        private readonly AssetManager assetManager;

        public GoalPiece(AssetManager assetManager, Board board, int x, int y, Goal goal)
        {
            this.assetManager = assetManager;
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
                goal = assetManager.GetImage(Images.Tiles.Extra);
            }
            else
            {
                if (Goal.GoalSize == GoalSize.Small)
                {
                    goal = assetManager.GetImage(Images.Tiles.Small, Goal.PlayerNumber + 1);
                }
                else
                {
                    goal = assetManager.GetImage(Images.Tiles.Big, Goal.PlayerNumber + 1);
                }
            }

            layer.DrawImage(goal, Position.X*BoardConstants.SquareSize + BoardConstants.SquareSize/2d, Position.Y*BoardConstants.SquareSize + BoardConstants.SquareSize/2d,
                BoardConstants.SquareSize,
                BoardConstants.SquareSize, true);
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
        }

        public static IImage GetGoalImage(AssetManager assetManager, Goal gc)
        {
            if (gc.IsExtra)
            {
                return assetManager.GetImage(Images.Cards.Extra);
            }
            if (gc.GoalSize == GoalSize.Small)
            {
                return assetManager.GetImage(Images.Cards.Small, gc.PlayerNumber + 1);
            }
            return assetManager.GetImage(Images.Cards.Big, gc.PlayerNumber + 1);
        }

        public static IImage GetBaseImage(AssetManager assetManager, Goal gc)
        {
            if (gc.IsExtra)
            {
                return assetManager.GetImage(Images.Cards.All);
            }
            return assetManager.GetImage(Images.Cards.Character, gc.PlayerNumber + 1);
        }
    }
}