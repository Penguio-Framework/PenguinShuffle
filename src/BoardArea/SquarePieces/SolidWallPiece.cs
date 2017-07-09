using System;
using System.Collections.Generic;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea
{
    public class SolidWallPiece : ISquarePiece
    {
        


        public SolidWallPiece( Board board, int x, int y)
        {
            Board = board;
            Position = new Point(x, y);
            Type = SquareTypes.SolidWall;


    /*        if (SolidWallImages == null)
            {
                SolidWallImages = new Dictionary<int, IImage>();

                SolidWallImages.Add((int) (Direction.Left | Direction.Top | Direction.Bottom | Direction.Right), assetManager.GetImage(Assets.Images.Tiles.WallSolid));

                SolidWallImages.Add((int) (Direction.Left), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left)));
                SolidWallImages.Add((int) (Direction.Top), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Top)));
                SolidWallImages.Add((int) (Direction.Bottom), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Bottom)));
                SolidWallImages.Add((int) (Direction.Right), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Right)));

                SolidWallImages.Add((int) (Direction.Left | Direction.Right), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left | Direction.Right)));
                SolidWallImages.Add((int) (Direction.Left | Direction.Top), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left | Direction.Top)));
                SolidWallImages.Add((int) (Direction.Left | Direction.Bottom), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left | Direction.Bottom)));

                SolidWallImages.Add((int) (Direction.Top | Direction.Bottom), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Top | Direction.Bottom)));
                SolidWallImages.Add((int) (Direction.Top | Direction.Right), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Top | Direction.Right)));

                SolidWallImages.Add((int) (Direction.Right | Direction.Bottom), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Right | Direction.Bottom)));

                SolidWallImages.Add((int) (Direction.Left | Direction.Right | Direction.Bottom), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left | Direction.Right | Direction.Bottom)));
                SolidWallImages.Add((int) (Direction.Left | Direction.Right | Direction.Top), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Left | Direction.Right | Direction.Top)));

                SolidWallImages.Add((int) (Direction.Top | Direction.Bottom | Direction.Left), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Top | Direction.Bottom | Direction.Left)));
                SolidWallImages.Add((int) (Direction.Top | Direction.Bottom | Direction.Right), assetManager.GetImage(Assets.Images.Tiles.WallSolidSides, (int) (Direction.Top | Direction.Bottom | Direction.Right)));
            }*/
        }


//        public static Dictionary<int, IImage> SolidWallImages { get; set; }
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
            int val = 0;
            val += (Board.getItemsOnBoard(null, Position.X - 1, Position.Y).Any(a => a is SolidWallPiece)
                ? (int) Engine.Interfaces.Direction.Left
                : 0);
            val += Board.getItemsOnBoard(null, Position.X + 1, Position.Y).Any(a => a is SolidWallPiece)
                ? (int) Engine.Interfaces.Direction.Right
                : 0;
            val += Board.getItemsOnBoard(null, Position.X, Position.Y - 1).Any(a => a is SolidWallPiece)
                ? (int) Engine.Interfaces.Direction.Up
                : 0;
            val += Board.getItemsOnBoard(null, Position.X, Position.Y + 1).Any(a => a is SolidWallPiece)
                ? (int) Engine.Interfaces.Direction.Down
                : 0;

//            IImage image = SolidWallImages[val == 0 ? 15 : val];
            IImage image = Assets.Images.Tiles.WallSolid;

            layer.DrawImage(image, Position.X*BoardConstants.SquareSize + BoardConstants.SquareSize/2d,
                Position.Y*BoardConstants.SquareSize + BoardConstants.SquareSize/2d,
                BoardConstants.SquareSize,
                BoardConstants.SquareSize, true);
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
        }
    }
}