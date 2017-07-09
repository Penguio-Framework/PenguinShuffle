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

            GameService = new GameService(AssetManager);

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

/*
            AssetManager.CreateImage(Assets.Images.Layouts.PenguinLogo, "layouts/penguin-logo");
            AssetManager.CreateImage(Assets.Images.Layouts.ShuffleLogo, "layouts/shuffle-logo");
            AssetManager.CreateImage(Assets.Images.Layouts.MainBG, "layouts/cloudless-main-bg");
            AssetManager.CreateImage(Assets.Images.Layouts.Cloud1, "layouts/cloud1");
            AssetManager.CreateImage(Assets.Images.Layouts.Cloud2, "layouts/cloud2");
            AssetManager.CreateImage(Assets.Images.Layouts.Cloud3, "layouts/cloud3");

            AssetManager.CreateImage(Assets.Images.Layouts.HelperImage1, "layouts/helper-image-1");
            AssetManager.CreateImage(Assets.Images.Layouts.HelperImage2, "layouts/helper-image-2");

            AssetManager.CreateImage(Assets.Images.Layouts.PlayButton, "layouts/play-button");
            AssetManager.CreateImage(Assets.Images.Layouts.TextBoard, "layouts/text-board");

            Songs.Menu = AssetManager.CreateSong("Menu", "songs/menu-music");
            Songs.Ocean = AssetManager.CreateSong("Ocean", "songs/ocean-background");
            Assets.Sounds.Slide = AssetManager.CreateSoundEffect("Slide", "sounds/slide");
            Assets.Sounds.Click = AssetManager.CreateSoundEffect("Click", "sounds/click");
            Assets.Sounds.TimeTick = AssetManager.CreateSoundEffect("TimeTick", "sounds/time-tick");
            SoundEffects.TimeDing = AssetManager.CreateSoundEffect("TimeDing", "sounds/time-ding");

            AssetManager.CreateFont(Assets.Fonts.BabyDoll._100, "fonts/BabyDoll/BabyDoll-100pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._36, "fonts/BabyDoll/BabyDoll-36pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._48, "fonts/BabyDoll/BabyDoll-48pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._60, "fonts/BabyDoll/BabyDoll-60pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._72, "fonts/BabyDoll/BabyDoll-72pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._90, "fonts/BabyDoll/BabyDoll-90pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._120, "fonts/BabyDoll/BabyDoll-120pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._130, "fonts/BabyDoll/BabyDoll-130pt");
            AssetManager.CreateFont(Assets.Fonts.BabyDoll._240, "fonts/BabyDoll/BabyDoll-240pt");

            AssetManager.CreateFont(Assets.Fonts.MyriadPro._100, "fonts/Myriad Pro/Myriad Pro-100pt");


            AssetManager.CreateImage(Assets.Images.About.Bubble, "about/about-bubble");
            AssetManager.CreateImage(Assets.Images.About.Penguin, "about/main-penguin");
            AssetManager.CreateImage(Assets.Images.About.Contact, "about/about-contact");
            AssetManager.CreateImage(Assets.Images.About.Rate, "about/about-rate");

            AssetManager.CreateImage(Assets.Images.Layouts.Tutorial, Images.TutorialArguments, "layouts/tutorial{0}");
            AssetManager.CreateImage(Assets.Images.Layouts.TutorialButton, "layouts/about-button");

            AssetManager.CreateImage(Assets.Images.Layouts.SoundOff, "layouts/sound-off-button");
            AssetManager.CreateImage(Assets.Images.Layouts.SoundOn, "layouts/sound-on-button");
            AssetManager.CreateImage(Assets.Images.Layouts.Arrow, "layouts/arrow");
            AssetManager.CreateImage(Assets.Images.Layouts.BackButton, "layouts/back-button");
            AssetManager.CreateImage(Assets.Images.Layouts.ModeButton, "layouts/mode-button");
            AssetManager.CreateImage(Assets.Images.Layouts.PlayerSelectionButton, "layouts/player-selected");
            AssetManager.CreateImage(Assets.Images.Layouts.PlayerUnselectedButton, "layouts/player-unselected");
            AssetManager.CreateImage(Assets.Images.Layouts.StagingArea, "layouts/staging-area");
            AssetManager.CreateImage(Assets.Images.Layouts.NumberSelectionBar, "layouts/numberSelectBar");
            AssetManager.CreateImage(Assets.Images.Layouts.PlayerLocked, "layouts/player-locked");

            AssetManager.CreateImage(Assets.Images.Board.Box, "board/box");
            AssetManager.CreateImage(Assets.Images.Layouts.GameBackground, "layouts/gameBackground");


            AssetManager.CreateImage(Assets.Images.Character.Arrow, Images.CharacterArguments, "character/arrow/place-character-arrow-0{0}");
            AssetManager.CreateImage(Assets.Images.Character.BannersBig, Images.CharacterArguments, "character/banners/big/{0}-long-banner");
            AssetManager.CreateImage(Assets.Images.Character.BannersSmall, Images.CharacterArguments, "character/banners/small/{0}-short-banner");
            AssetManager.CreateImage(Assets.Images.Character.Placement, Images.CharacterArguments, "character/placement/{0}-character-placement");
            AssetManager.CreateImage(Assets.Images.Character.Sliding, Images.CharacterArguments, "character/sliding/sliding-characters-0{0}");
            AssetManager.CreateImage(Assets.Images.Character.LabelBox, Images.CharacterArguments, "character/labelBox/character.{0}.box");
            AssetManager.CreateImage(Assets.Images.Character.Box, Images.CharacterArguments, "character/box/character.{0}.box");
            AssetManager.CreateImage(Assets.Images.Character.StationaryBig, Images.CharacterArguments, "character/stationary/big/stationary-characters-0{0}");
            AssetManager.CreateImage(Assets.Images.Character.Stationary, Images.CharacterArguments, "character/stationary/small/stationary-characters-0{0}");
            AssetManager.CreateImage(Assets.Images.Character.Shadow, "character/character-shadow");

            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated1, Images.CharacterArguments, "character/animations/{0}/characters-animated-1");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated2, Images.CharacterArguments, "character/animations/{0}/characters-animated-2");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated3, Images.CharacterArguments, "character/animations/{0}/characters-animated-3");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated4, Images.CharacterArguments, "character/animations/{0}/characters-animated-4");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated5, Images.CharacterArguments, "character/animations/{0}/characters-animated-5");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated6, Images.CharacterArguments, "character/animations/{0}/characters-animated-6");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated7, Images.CharacterArguments, "character/animations/{0}/characters-animated-7");
            AssetManager.CreateImage(Assets.Images.Character.StandingAnimated8, Images.CharacterArguments, "character/animations/{0}/characters-animated-8");



            AssetManager.CreateImage(Assets.Images.Tiles.Big, Images.CharacterArguments, "tiles/big/fish-tiles-0{0}");
            AssetManager.CreateImage(Assets.Images.Tiles.Small, Images.CharacterArguments, "tiles/small/fish-tiles-0{0}");
            AssetManager.CreateImage(Assets.Images.Tiles.Extra, "tiles/fish-tiles-extra");

            AssetManager.CreateImage(Assets.Images.Tiles.WallHorizontal, "tiles/wall-horizontal");
            AssetManager.CreateImage(Assets.Images.Tiles.WallVertical, "tiles/wall-vertical");
            AssetManager.CreateImage(Assets.Images.Tiles.WallSolid, "tiles/wall-solid");
            //            AssetManager.CreateImage(Assets.Images.Tiles.WallSolidSides, Images.SolidWallArguments, "tiles/solid/solidWall.{0}");


            AssetManager.CreateImage(Assets.Images.Cards.Big, Images.CharacterArguments, "cards/big/cards-0{0}");
            AssetManager.CreateImage(Assets.Images.Cards.Small, Images.CharacterArguments, "cards/small/cards-0{0}");
            AssetManager.CreateImage(Assets.Images.Cards.Character, Images.CharacterArguments, "cards/character/cards-0{0}");
            AssetManager.CreateImage(Assets.Images.Cards.All, "cards/cards-all");
            AssetManager.CreateImage(Assets.Images.Cards.Extra, "cards/cards-extra");
            AssetManager.CreateImage(Assets.Images.Cards.Back, "cards/cards-back");*/
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
                    game.GameService.ClassicGameState.Board = new Board(game.AssetManager, game.GameService);
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
