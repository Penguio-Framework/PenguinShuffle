using Engine;
using Engine.Interfaces;
using PenguinShuffle.PuzzleArea;

namespace PenguinShuffle
{
    public class Game : IGame
    {
        public IScreen puzzleAreaScreen;
        public ILayout PuzzleAreaLayout { get; set; }

        public GameService GameService { get; set; }

        public IScreenManager ScreenManager { get; set; }
        public IClient Client { get; set; }
        public AssetManager AssetManager { get; set; }

        public void InitScreens(IRenderer renderer, IScreenManager screenManager)
        {
            ScreenManager = screenManager;


            GameService = new GameService(AssetManager);
            var screenTransitioner = new ScreenTransitioner(this);
            int width = 1536;
            int height = 2048;

            puzzleAreaScreen = screenManager.CreateScreen();
            PuzzleAreaLayout = puzzleAreaScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            PuzzleAreaLayout.LayoutView = new PuzzleAreaLayout(this, GameService, renderer, PuzzleAreaLayout, screenTransitioner);


            ScreenManager.ChangeScreen(puzzleAreaScreen);
        }



        public void BeforeDraw()
        {
        }

        public void AfterDraw()
        {
        }


        public void BeforeTick()
        {
        }

        public void AfterTick()
        {
        }


        public void LoadAssets(IRenderer renderer)
        {
            AssetManager = new AssetManager(renderer, Client);

            new AssetCollection(AssetManager);
        }
    }
     
}