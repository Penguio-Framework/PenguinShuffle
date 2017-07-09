using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine;
using Engine.Interfaces;
using PenguinShuffle;
using PenguinShuffle.BoardArea;
using PenguinShuffle.BoardArea.Layouts;
using PenguinShuffle.LandingArea;
using PenguinShuffle.SettingsArea;

namespace PenguinShuffle
{
    public class Game : IGame
    {
        public IScreen boardCardSelectionScreen;
        public IScreen boardScreen;
        public IScreen boardSelectionScreen;
        public IScreen boardViewingScreen;
        public IScreen choosePuzzleScreen;
        public IScreen landingScreen;
        public IScreen puzzleBoardScreen;
        public IScreen settingsScreen;

        public GameService GameService { get; set; }

        public ILayout SettingsAreaLayout { get; set; }
        public ILayout LandingAreaLayout { get; set; }
        public ILayout BoardAreaLayout { get; set; }
        public ILayout BoardAreaViewingLayout { get; set; }
        public ILayout BoardAreaCardSelectionLayout { get; set; }
        public ILayout ChoosePuzzleAreaLayout { get; set; }
        public ILayout PuzzleBoardAreaLayout { get; set; }
        public ILayout BoardAreaSelectionLayout { get; set; }
        public ILayout CreateBoardAreaLayout { get; set; }

        public IScreenManager ScreenManager { get; set; }
        public IClient Client { get; set; }

        public AssetManager AssetManager { get; set; }



        public void InitScreens(IRenderer renderer, IScreenManager screenManager)
        {
            ScreenManager = screenManager;

            GameService = new GameService();

            var screenTransitioner = new ScreenTransitioner(this);
            int width = 1536;
            int height = 2048;

            landingScreen = screenManager.CreateScreen();
            LandingAreaLayout = landingScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            LandingAreaLayout.LayoutView = new LandingAreaLayout(this, GameService, renderer, LandingAreaLayout, screenTransitioner);


            settingsScreen = screenManager.CreateScreen();
            SettingsAreaLayout = settingsScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            SettingsAreaLayout.LayoutView = new SettingsAreaLayout(this, GameService, renderer, SettingsAreaLayout, screenTransitioner);

            boardSelectionScreen = screenManager.CreateScreen();
            BoardAreaSelectionLayout = boardSelectionScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            BoardAreaSelectionLayout.LayoutView = new BoardSelectionAreaLayout(this, GameService, renderer, BoardAreaSelectionLayout, screenTransitioner);


            boardCardSelectionScreen = screenManager.CreateScreen();
            BoardAreaCardSelectionLayout = boardCardSelectionScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            BoardAreaCardSelectionLayout.LayoutView = new BoardCardSelectionAreaLayout(this, GameService, renderer, BoardAreaCardSelectionLayout, screenTransitioner);


            boardViewingScreen = screenManager.CreateScreen();
            BoardAreaViewingLayout = boardViewingScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            BoardAreaViewingLayout.LayoutView = new BoardViewingAreaLayout(this, GameService, renderer, BoardAreaViewingLayout, screenTransitioner);


            boardScreen = screenManager.CreateScreen();
            BoardAreaLayout = boardScreen.CreateLayout(width, height).MakeActive().SetScreenOrientation(ScreenOrientation.Vertical);
            BoardAreaLayout.LayoutView = new BoardSlidingAreaLayout(this, GameService, renderer, BoardAreaLayout, screenTransitioner);


            ScreenManager.ChangeScreen(landingScreen);
        }


        public void InitSocketManager(ISocketManager socketManager)
        {
            /*
                        Socket = socketManager.Create("http://192.168.1.3:3000/");
                        Socket.OnConnect = () =>
                        {
                            Socket.Emit("shoes2", new { Fuck = "Yopu" });
                        };
                        Socket.On<int>("shoes", a =>
                        {
                        });
                        Socket.On<int>("shoes3", a =>
                        {
                        });

                        Socket.Connect();
            */
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
            Assets.LoadAssets(renderer, AssetManager);
            Client.SetCustomLetterbox(Assets.Images.Layouts.GrayBg);
        }
    }

    public class ScreenTransitioner
    {
        private readonly Game game;

        public ScreenTransitioner(Game game)
        {
            this.game = game;
        }

        public void StartGame(int numberOfPlayers, GameMode gameMode)
        {
            game.GameService.GameMode = gameMode;


            game.Client.PlaySong(Assets.Songs.OceanBackground);
            switch (gameMode)
            {
                case GameMode.Classic:
                    game.GameService.ClassicGameState.NumberOfPlayers = numberOfPlayers;
                    game.GameService.ClassicGameState.Board = new Board(game.GameService);
                    game.GameService.ClassicGameState.Board.StartGame();

                    game.ScreenManager.ChangeScreen(game.boardSelectionScreen);
                    break;
                case GameMode.Puzzle:
                    game.ScreenManager.ChangeScreen(game.choosePuzzleScreen);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("gameMode");
            }
        }

        public void ChangeToChoosePuzzleScreen()
        {
            game.ScreenManager.ChangeScreen(game.choosePuzzleScreen);
        }

        public void ChangeToBoardScreen()
        {
            game.ScreenManager.ChangeScreen(game.boardScreen);
        }


        public void ChangeToSettingsScreen()
        {
            game.ScreenManager.ChangeScreen(game.settingsScreen);

        }

        public void ChangeToBoardCardSelectionScreen()
        {
            game.ScreenManager.ChangeScreen(game.boardCardSelectionScreen);
        }

        public void ChangeToBoardViewingScreen()
        {
            game.ScreenManager.ChangeScreen(game.boardViewingScreen);
        }

        public void ChangeToPuzzleBoardScreen()
        {
            game.ScreenManager.ChangeScreen(game.puzzleBoardScreen);
        }

        public void ChangeToLanding()
        {
            game.Client.PlaySong(Assets.Songs.MenuMusic);
            game.ScreenManager.ChangeScreen(game.landingScreen);
        }
    }
}

public class Colors
{
    public static Color DarkFontColor = new Color(109, 110, 113);
}
/*namespace PenguinShuffle
{
    public class Fonts
    {
        public class BabyDoll
        {
            public static string _36 = Guid.NewGuid().ToString();
            public static string _48 = Guid.NewGuid().ToString();
            public static string _60 = Guid.NewGuid().ToString();
            public static string _72 = Guid.NewGuid().ToString();
            public static string _90 = Guid.NewGuid().ToString();
            public static string _100 = Guid.NewGuid().ToString();
            public static string _120 = Guid.NewGuid().ToString();
            public static string _130 = Guid.NewGuid().ToString();
            public static string _240 = Guid.NewGuid().ToString();
        }
        public class MyriadPro
        {
            public static string _100 = Guid.NewGuid().ToString();
        }
    }
    public class Songs
    {
        public static ISong Menu { get; set; }
        public static ISong Ocean { get; set; }
    }
    public class SoundEffects
    {
        public static ISoundEffect Slide { get; set; }
        public static ISoundEffect Click { get; set; }
        public static ISoundEffect TimeTick { get; set; }
        public static ISoundEffect TimeDing { get; set; }
    }

    public class Images
    {
        public static object[] CharacterArguments = range(1, 6);
        public static object[] SolidWallArguments = range(1, 15);
        public static object[] TutorialArguments = range(1, 2);

        static Images()
        {
        }

        public static int ImageCount { get; set; }

        private static object[] range(int start, int end)
        {
            var js = new object[end - start + 1];
            int ind = 0;
            for (; start <= end; start++)
            {
                js[ind++] = start;
            }
            return js;
        }

        public class About
        {
            public static string Bubble = Guid.NewGuid().ToString();
            public static string Penguin = Guid.NewGuid().ToString();
            public static string Contact = Guid.NewGuid().ToString();
            public static string Rate = Guid.NewGuid().ToString();
        }


        public class Board
        {
            public static string Box = Guid.NewGuid().ToString();
        }

        public class Cards
        {
            public static string Back = Guid.NewGuid().ToString();
            public static string Extra = Guid.NewGuid().ToString();
            public static string Small = Guid.NewGuid().ToString();
            public static string Character = Guid.NewGuid().ToString();
            public static string Big = Guid.NewGuid().ToString();
            public static string All = Guid.NewGuid().ToString();
        }

        public class Characters
        {
            public static string Arrow = Guid.NewGuid().ToString();
            public static string BannersBig = Guid.NewGuid().ToString();
            public static string BannersSmall = Guid.NewGuid().ToString();
            public static string Placement = Guid.NewGuid().ToString();
            public static string Sliding = Guid.NewGuid().ToString();
            public static string StationaryBig = Guid.NewGuid().ToString();
            public static string Stationary = Guid.NewGuid().ToString();
            public static string Box = Guid.NewGuid().ToString();
            public static string Shadow = Guid.NewGuid().ToString();
            public static string LabelBox = Guid.NewGuid().ToString();
            public static string StandingAnimated1 = Guid.NewGuid().ToString();
            public static string StandingAnimated2 = Guid.NewGuid().ToString();
            public static string StandingAnimated3 = Guid.NewGuid().ToString();
            public static string StandingAnimated4 = Guid.NewGuid().ToString();
            public static string StandingAnimated5 = Guid.NewGuid().ToString();
            public static string StandingAnimated6 = Guid.NewGuid().ToString();
            public static string StandingAnimated7 = Guid.NewGuid().ToString();
            public static string StandingAnimated8 = Guid.NewGuid().ToString();

        }

        public class ChoosePuzzle
        {
            public static string PuzzleBoard = Guid.NewGuid().ToString();
            public static string PuzzleLocked = Guid.NewGuid().ToString();
        }

        public class Layouts
        {
            public static string TutorialButton = Guid.NewGuid().ToString();
            public static string Arrow = Guid.NewGuid().ToString();
            public static string BackButton = Guid.NewGuid().ToString();
            public static string ModeButton = Guid.NewGuid().ToString();
            public static string PlayButton = Guid.NewGuid().ToString();
            public static string PlayerSelectionButton = Guid.NewGuid().ToString();
            public static string PlayerUnselectedButton = Guid.NewGuid().ToString();
            public static string PenguinLogo = Guid.NewGuid().ToString();
            public static string ShuffleLogo = Guid.NewGuid().ToString();
            public static string MainBG = Guid.NewGuid().ToString();
            public static string Cloud1 = Guid.NewGuid().ToString();
            public static string Cloud2 = Guid.NewGuid().ToString();
            public static string Cloud3 = Guid.NewGuid().ToString();
            public static string StagingArea = Guid.NewGuid().ToString();
            public static string TextBoard = Guid.NewGuid().ToString();
            public static string NumberSelectionBar = Guid.NewGuid().ToString();
            public static string PlayerLocked = Guid.NewGuid().ToString();
            public static string GameBackground = Guid.NewGuid().ToString();
            public static string Tutorial = Guid.NewGuid().ToString();
            public static string SoundOff = Guid.NewGuid().ToString();
            public static string SoundOn = Guid.NewGuid().ToString();
            public static string Letterbox = Guid.NewGuid().ToString();
            public static string HelperImage1 = Guid.NewGuid().ToString();
            public static string HelperImage2 = Guid.NewGuid().ToString();
        }

        public class Tiles
        {
            public static string Big = Guid.NewGuid().ToString();
            public static string Small = Guid.NewGuid().ToString();
            public static string Extra = Guid.NewGuid().ToString();
            public static string WallHorizontal = Guid.NewGuid().ToString();
            public static string WallVertical = Guid.NewGuid().ToString();
            public static string WallSolid = Guid.NewGuid().ToString();
            //            public static string WallSolidSides = Guid.NewGuid().ToString();
        }
    }
}*/
