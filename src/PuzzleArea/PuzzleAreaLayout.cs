using System;
using System.Collections.Generic;
using Engine;
using Engine.Interfaces;

namespace DemolitionRobots.PuzzleArea
{
    public class PuzzleAreaLayout : ILayoutView
    {
        private ILayer mainLayer;
        public Game Game { get; set; }
        public GameService GameService { get; set; }
        public IRenderer Renderer { get; set; }

        public PuzzleAreaLayout(Game game, GameService gameService, IRenderer renderer, ILayout layout, ScreenTransitioner screenTransitioner)
        {
            Game = game;
            GameService = gameService;
            Renderer = renderer;
            Layout = layout;
            ScreenTransitioner = screenTransitioner;

        }

        public void InitLayoutView()
        {

            TouchManager = new TouchManager(Game.Client);
            mainLayer = Renderer.CreateLayer(Layout.Width, Layout.Height, Layout);
            Renderer.AddLayer(mainLayer);

            BoardConstants.Width = 600;
            BoardConstants.Height = 600;

        }

        public void TickLayoutView(TimeSpan elapsedGameTime)
        {

        }

        public ITouchManager TouchManager { get; private set; }
        public ILayout Layout { get; set; }
        public ScreenTransitioner ScreenTransitioner { get; set; }

        public void Render(TimeSpan elapsedGameTime)
        {


            var puzzle = new Puzzle()
            {
                BoardHeight = 3,
                BoardWidth = 3,
                TotalMoves = 7,
                TotalTime = 7,
                Pieces = new List<IPuzzlePiece>()
            };



            mainLayer.Begin();
            mainLayer.Save();



            mainLayer.DrawRectangle(new Color(58, 58, 60), 0, 0, mainLayer.Layout.Width, mainLayer.Layout.Height);

            var squareWidth = BoardConstants.Width / puzzle.BoardWidth;
            var squareHeight = BoardConstants.Height / puzzle.BoardHeight;


            mainLayer.Translate(100, 600);
            mainLayer.DrawRectangle(new Color(196, 154, 108), 0, 0, BoardConstants.Width, BoardConstants.Height);

            for (int x = 0; x < puzzle.BoardWidth; x++)
            {
                for (int y = 0; y < puzzle.BoardHeight; y++)
                {
                    mainLayer.StrokeRectangle(new Color(231, 232, 233), x * squareWidth, y * squareHeight, squareWidth, squareHeight, 5);
                }
            }

            foreach (var pieces in puzzle.Pieces.GroupBy(a=>a.DrawPriority).OrderBy(a=>a.Key))
            {
                foreach (var puzzlePiece in pieces)
                {
                    puzzlePiece.Render(mainLayer);
                }
            }
            
            mainLayer.Restore();
//            mainLayer.DrawImage(Images.Layouts.Cloud[Images.Layouts.CloudOptions._1], 500, 500);
            mainLayer.DrawImage(Images.Layouts.Foob, 200, 500);
            mainLayer.DrawImage(Images.Layouts.Logo, 0, 0);
//            mainLayer.DrawImage(Images.Layouts.ModeButton, 500, 800);

//            mainLayer.DrawString(Fonts.BabyDoll.BabyDollPt[Fonts.BabyDoll.BabyDollPtOptions._120], "Hello", 500, 800);

            mainLayer.End();

        }

        public void Destroy()
        {

        }
    }

    public static class BoardConstants
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
    }

    public class Puzzle
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int TotalMoves { get; set; }
        public int TotalTime { get; set; }
        public List<IPuzzlePiece> Pieces { get; set; }
    }

    public interface IPuzzlePiece
    {
        int DrawPriority { get; set; }
        Point Position { get; set; }
        Point Size { get; set; }
        PuzzlePieceType Type { get; set; }
        CollisionOptions OnBeforeCollide(Player player, Direction direction);
        CollisionOptions OnAfterCollide(Player player, Direction direction);
        void Render(ILayer layer);
        void Tick(TimeSpan elapsedGameTime);
        string Serialize();
    }

    public class Player
    {

    }
    public enum CollisionOptions
    {
        SlideInto,
        Stall,
        SlideOutOf,
        Goal
    }

    public enum PuzzlePieceType
    {
        Player,
        Warp,
        Building,
        BuildingButton,
        Arrow
    }
}