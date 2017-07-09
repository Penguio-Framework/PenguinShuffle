using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class Player
    {
        private const double animationCoolDown = 0.063*2.5;
        private const double animationMinVelocity = 3.2*2.5;
        private readonly Board _board;
        private readonly IRenderer _renderer;
        private readonly AssetManager assetManager;
        private double animationVelocity;

        public Player(AssetManager assetManager, Board board, int playerNumber, int x, int y)
        {
            this.assetManager = assetManager;
            _board = board;
            PlayerNumber = playerNumber;
            Direction = Direction.Bottom;
            CurrentAnimatedOffset = new PointF(0, 0);
            Position = new Point(x, y);
        }

        public int PlayerNumber { get; set; }
        public PlayerPosition AnimateToPosition { get; set; }


        public Point Position { get; set; }
        public Direction Direction { get; set; }


        public int AnimationDistance { get; set; }

        public PointF CurrentAnimatedOffset { get; set; }

        public void Render(ILayer layer)
        {
            if (AnimateToPosition != null)
            {
                animationVelocity = Math.Max(animationMinVelocity, animationVelocity - animationCoolDown);
                switch (AnimateToPosition.Direction)
                {
                    case Direction.Left:
                        CurrentAnimatedOffset.X += -animationVelocity;
                        break;
                    case Direction.Right:
                        CurrentAnimatedOffset.X += +animationVelocity;
                        break;
                    case Direction.Top:
                        CurrentAnimatedOffset.Y += -animationVelocity;
                        break;
                    case Direction.Bottom:
                        CurrentAnimatedOffset.Y += +animationVelocity;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (Position.X + (int) (CurrentAnimatedOffset.X/BoardConstants.SquareSize) == AnimateToPosition.X &&
                    Position.Y + (int) (CurrentAnimatedOffset.Y/BoardConstants.SquareSize) == AnimateToPosition.Y)
                {
                    Position.X = AnimateToPosition.X;
                    Position.Y = AnimateToPosition.Y;
                    Direction = AnimateToPosition.Direction;
                    CurrentAnimatedOffset = new PointF(0, 0);
                    _board.MoveToPosition = null;
                    AnimateToPosition = null;
                }
            }


            var posX = (int) (CurrentAnimatedOffset.X + Position.X*BoardConstants.SquareSize + BoardConstants.SquareSize/2d);
            var posY = (int) (CurrentAnimatedOffset.Y + Position.Y*BoardConstants.SquareSize + BoardConstants.SquareSize/2d);

            IImage image;
            switch (AnimateToPosition != null ? AnimateToPosition.Direction : Direction)
            {
                case Direction.Left:
                    image = Assets.Images.Character.Stationary.Small.StationaryCharacters[ PlayerNumber];
                    layer.DrawImage(image, posX, posY, 90*Math.PI/180, BoardConstants.SquareSize, BoardConstants.SquareSize, true);
                    break;
                case Direction.Right:
                    image = Assets.Images.Character.Stationary.Small.StationaryCharacters[PlayerNumber];
                    layer.DrawImage(image, posX, posY, 270*Math.PI/180, BoardConstants.SquareSize, BoardConstants.SquareSize, true);
                    break;
                case Direction.Top:
                    image = Assets.Images.Character.Sliding.SlidingCharacters[PlayerNumber];
                    layer.DrawImage(image, posX, posY, BoardConstants.SquareSize, BoardConstants.SquareSize, true);
                    break;
                case Direction.Bottom:
                    image = Assets.Images.Character.Stationary.Small.StationaryCharacters[PlayerNumber];
                    layer.DrawImage(image, posX, posY, BoardConstants.SquareSize, BoardConstants.SquareSize, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetAnimating(PlayerPosition newPosition)
        {
            AnimateToPosition = new PlayerPosition(this, newPosition.X, newPosition.Y, newPosition.Direction);
            CurrentAnimatedOffset = new PointF(0, 0);
            animationVelocity = 8.1*2.5;
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
        }
    }
}