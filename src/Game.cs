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
        private IRenderer _renderer;
        public IScreen boardCardSelectionScreen;
        public IScreen boardScreen;
        public IScreen boardSelectionScreen;
        public IScreen boardViewingScreen;
        public IScreen choosePuzzleScreen;
        public IScreen createBoardScreen;
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
        public ISocket Socket { get; set; }
        public IClient Client { get; set; }
        public AssetManager AssetManager { get; set; }

        public void OnProductPurchased(string sku)
        {
           /* if (GameService.Products.First(a => a.Identifier == sku) != null)
            {
                GameService.HasMultiplayer = true;
            }*/
        }

        public void InitScreens(IRenderer renderer, IScreenManager screenManager)
        {
            ScreenManager = screenManager;
            _renderer = renderer;

            GameService = new GameService(AssetManager);

/*
            GameService.Products = Client.ClientSettings.GetAvailableProducts(new List<string>()
            {
                "product.multiplayerEnabled"
            });
            if (GameService.Products.Count > 0)
            {
                GameService.HasMultiplayer = Client.ClientSettings.IsProductPurchased(GameService.Products[0]);
            }
            else
            {
                GameService.HasMultiplayer = false;
            }*/

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
            AssetManager = new AssetManager(renderer, Client);
//            Client.SetCustomLetterbox(AssetManager.CreateImage(Images.Layouts.Letterbox, "images/layouts/gray-bg"));


            AssetManager.CreateImage(Images.Layouts.PenguinLogo, "images/layouts/penguin-logo");
            AssetManager.CreateImage(Images.Layouts.ShuffleLogo, "images/layouts/shuffle-logo");
            AssetManager.CreateImage(Images.Layouts.MainBG, "images/layouts/cloudless-main-bg");
            AssetManager.CreateImage(Images.Layouts.Cloud1, "images/layouts/cloud1");
            AssetManager.CreateImage(Images.Layouts.Cloud2, "images/layouts/cloud2");
            AssetManager.CreateImage(Images.Layouts.Cloud3, "images/layouts/cloud3");

            AssetManager.CreateImage(Images.Layouts.HelperImage1, "images/layouts/helper-image-1");
            AssetManager.CreateImage(Images.Layouts.HelperImage2, "images/layouts/helper-image-2");

            AssetManager.CreateImage(Images.Layouts.PlayButton, "images/layouts/play-button");
            AssetManager.CreateImage(Images.Layouts.TextBoard, "images/layouts/text-board");

            Songs.Menu = AssetManager.CreateSong("Menu", "songs/menu-music");
            Songs.Ocean = AssetManager.CreateSong("Ocean", "songs/ocean-background");
            SoundEffects.Slide = AssetManager.CreateSoundEffect("Slide", "sounds/slide");
            SoundEffects.Click = AssetManager.CreateSoundEffect("Click", "sounds/click");
            SoundEffects.TimeTick = AssetManager.CreateSoundEffect("TimeTick", "sounds/time-tick");
            SoundEffects.TimeDing = AssetManager.CreateSoundEffect("TimeDing", "sounds/time-ding");

            AssetManager.CreateFont(Fonts.BabyDoll._100, "fonts/BabyDoll/BabyDoll-100pt");
            AssetManager.CreateFont(Fonts.BabyDoll._36, "fonts/BabyDoll/BabyDoll-36pt");
            AssetManager.CreateFont(Fonts.BabyDoll._48, "fonts/BabyDoll/BabyDoll-48pt");
            AssetManager.CreateFont(Fonts.BabyDoll._60, "fonts/BabyDoll/BabyDoll-60pt");
            AssetManager.CreateFont(Fonts.BabyDoll._72, "fonts/BabyDoll/BabyDoll-72pt");
            AssetManager.CreateFont(Fonts.BabyDoll._90, "fonts/BabyDoll/BabyDoll-90pt");
            AssetManager.CreateFont(Fonts.BabyDoll._120, "fonts/BabyDoll/BabyDoll-120pt");
            AssetManager.CreateFont(Fonts.BabyDoll._130, "fonts/BabyDoll/BabyDoll-130pt");
            AssetManager.CreateFont(Fonts.BabyDoll._240, "fonts/BabyDoll/BabyDoll-240pt");

            AssetManager.CreateFont(Fonts.MyriadPro._100, "fonts/Myriad Pro/Myriad Pro-100pt");


            AssetManager.CreateImage(Images.About.Bubble, "images/about/about-bubble");
            AssetManager.CreateImage(Images.About.Penguin, "images/about/main-penguin");
            AssetManager.CreateImage(Images.About.Contact, "images/about/about-contact");
            AssetManager.CreateImage(Images.About.Rate, "images/about/about-rate");

            AssetManager.CreateImage(Images.Layouts.Tutorial, Images.TutorialArguments, "images/layouts/tutorial{0}");
            AssetManager.CreateImage(Images.Layouts.TutorialButton, "images/layouts/about-button");

            AssetManager.CreateImage(Images.Layouts.SoundOff, "images/layouts/sound-off-button");
            AssetManager.CreateImage(Images.Layouts.SoundOn, "images/layouts/sound-on-button");
            AssetManager.CreateImage(Images.Layouts.Arrow, "images/layouts/arrow");
            AssetManager.CreateImage(Images.Layouts.BackButton, "images/layouts/back-button");
            AssetManager.CreateImage(Images.Layouts.ModeButton, "images/layouts/mode-button");
            AssetManager.CreateImage(Images.Layouts.PlayerSelectionButton, "images/layouts/player-selected");
            AssetManager.CreateImage(Images.Layouts.PlayerUnselectedButton, "images/layouts/player-unselected");
            AssetManager.CreateImage(Images.Layouts.StagingArea, "images/layouts/staging-area");
            AssetManager.CreateImage(Images.Layouts.NumberSelectionBar, "images/layouts/numberSelectBar");
            AssetManager.CreateImage(Images.Layouts.PlayerLocked, "images/layouts/player-locked");

            AssetManager.CreateImage(Images.Board.Box, "images/board/box");
            AssetManager.CreateImage(Images.Layouts.GameBackground, "images/layouts/gameBackground");


            AssetManager.CreateImage(Images.Characters.Arrow, Images.CharacterArguments, "images/character/arrow/place-character-arrow-0{0}");
            AssetManager.CreateImage(Images.Characters.BannersBig, Images.CharacterArguments, "images/character/banners/big/{0}-long-banner");
            AssetManager.CreateImage(Images.Characters.BannersSmall, Images.CharacterArguments, "images/character/banners/small/{0}-short-banner");
            AssetManager.CreateImage(Images.Characters.Placement, Images.CharacterArguments, "images/character/placement/{0}-character-placement");
            AssetManager.CreateImage(Images.Characters.Sliding, Images.CharacterArguments, "images/character/sliding/sliding-characters-0{0}");
            AssetManager.CreateImage(Images.Characters.LabelBox, Images.CharacterArguments, "images/character/labelBox/character.{0}.box");
            AssetManager.CreateImage(Images.Characters.Box, Images.CharacterArguments, "images/character/box/character.{0}.box");
            AssetManager.CreateImage(Images.Characters.StationaryBig, Images.CharacterArguments, "images/character/stationary/big/stationary-characters-0{0}");
            AssetManager.CreateImage(Images.Characters.Stationary, Images.CharacterArguments, "images/character/stationary/small/stationary-characters-0{0}");
            AssetManager.CreateImage(Images.Characters.Shadow, "images/character/character-shadow");

            AssetManager.CreateImage(Images.Characters.StandingAnimated1, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-1");
            AssetManager.CreateImage(Images.Characters.StandingAnimated2, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-2");
            AssetManager.CreateImage(Images.Characters.StandingAnimated3, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-3");
            AssetManager.CreateImage(Images.Characters.StandingAnimated4, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-4");
            AssetManager.CreateImage(Images.Characters.StandingAnimated5, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-5");
            AssetManager.CreateImage(Images.Characters.StandingAnimated6, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-6");
            AssetManager.CreateImage(Images.Characters.StandingAnimated7, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-7");
            AssetManager.CreateImage(Images.Characters.StandingAnimated8, Images.CharacterArguments, "images/character/animations/{0}/characters-animated-8");



            AssetManager.CreateImage(Images.Tiles.Big, Images.CharacterArguments, "images/tiles/big/fish-tiles-0{0}");
            AssetManager.CreateImage(Images.Tiles.Small, Images.CharacterArguments, "images/tiles/small/fish-tiles-0{0}");
            AssetManager.CreateImage(Images.Tiles.Extra, "images/tiles/fish-tiles-extra");

            AssetManager.CreateImage(Images.Tiles.WallHorizontal, "images/tiles/wall-horizontal");
            AssetManager.CreateImage(Images.Tiles.WallVertical, "images/tiles/wall-vertical");
            AssetManager.CreateImage(Images.Tiles.WallSolid, "images/tiles/wall-solid");
            //            AssetManager.CreateImage(Images.Tiles.WallSolidSides, Images.SolidWallArguments, "images/tiles/solid/solidWall.{0}");


            AssetManager.CreateImage(Images.Cards.Big, Images.CharacterArguments, "images/cards/big/cards-0{0}");
            AssetManager.CreateImage(Images.Cards.Small, Images.CharacterArguments, "images/cards/small/cards-0{0}");
            AssetManager.CreateImage(Images.Cards.Character, Images.CharacterArguments, "images/cards/character/cards-0{0}");
            AssetManager.CreateImage(Images.Cards.All, "images/cards/cards-all");
            AssetManager.CreateImage(Images.Cards.Extra, "images/cards/cards-extra");
            AssetManager.CreateImage(Images.Cards.Back, "images/cards/cards-back");
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


            game.Client.PlaySong(Songs.Ocean);
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
#if createBoard
            _game.GameService.GameMode=GameMode.Create;
            
            _game.ScreenManager.ChangeScreen(_game.createBoardScreen);
#else
            game.ScreenManager.ChangeScreen(game.settingsScreen);
#endif
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
            game.Client.PlaySong(Songs.Menu);
            game.ScreenManager.ChangeScreen(game.landingScreen);
        }
    }
}

namespace PenguinShuffle
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
            public static Color DarkFontColor = new Color(109, 110, 113);
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
}