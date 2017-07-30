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
    public class Game : BaseGame
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



        public override void InitScreens()
        {

            GameService = new GameService();

            var screenTransitioner = new ScreenTransitioner(this);
            ScreenManager.SetDefaultScreenSize(1536, 2048);


            landingScreen = ScreenManager.CreateDefaultScreenLayout(new LandingAreaLayout(GameService,  screenTransitioner));
            settingsScreen = ScreenManager.CreateDefaultScreenLayout(new SettingsAreaLayout(GameService,  screenTransitioner));
            boardSelectionScreen = ScreenManager.CreateDefaultScreenLayout(new BoardSelectionAreaLayout( GameService,  screenTransitioner));
            boardCardSelectionScreen = ScreenManager.CreateDefaultScreenLayout(new BoardCardSelectionAreaLayout( GameService,  screenTransitioner));
            boardViewingScreen = ScreenManager.CreateDefaultScreenLayout(new BoardViewingAreaLayout( GameService,  screenTransitioner));
            boardScreen = ScreenManager.CreateDefaultScreenLayout(new BoardSlidingAreaLayout( GameService,  screenTransitioner));

            ScreenManager.ChangeScreen(landingScreen);
        }


        public void InitSocketManager(ISocketManager socketManager)
        {
        }


        public override void BeforeDraw()
        {
        }

        public override void AfterDraw()
        {
        }


        public override void BeforeTick()
        {
        }

        public override void AfterTick()
        {
        }


        public override void LoadAssets()
        {
            Assets.LoadAssets(Renderer, AssetManager);
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
