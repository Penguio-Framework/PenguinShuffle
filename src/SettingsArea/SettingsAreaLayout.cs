using System;
using System.Runtime.CompilerServices;
using Engine;
using Engine.Animation;
using Engine.Interfaces;

namespace PenguinShuffle.SettingsArea
{
    public class SettingsAreaLayout : ILayoutView
    {
        public SettingsAreaLayout(Game game, GameService gameService, IRenderer renderer, ILayout layout, ScreenTransitioner screenManager)
        {
            this.game = game;
            Renderer = renderer;
            GameService = gameService;
            Layout = layout;
            AssetManager = game.AssetManager;
            State = new SettingsAreaLayoutState(layout);

            State.ScreenManager = screenManager;
        }

        private Game game { get; set; }
        private IRenderer Renderer { get; set; }
        public GameService GameService { get; set; }
        public AssetManager AssetManager { get; set; }
        public SettingsAreaLayoutState State { get; set; }
        public ILayer mainLayer { get; set; }
        public ILayout Layout { get; set; }
        public ITouchManager TouchManager { get; private set; }

        public void InitLayoutView()
        {
            Init();
        }

        public void Render(TimeSpan elapsedGameTime)
        {
            mainLayer.Begin();

            GameService.CloudSubLayout.Render(mainLayer);

            if (State.MenuAnimation.Completed)
            {
                if (!State.StartClicked)
                {
                    drawMenu(0);
                }
            }
            else
            {
                State.MenuAnimation.Render(mainLayer);
            }

            if (State.StartGameAnimation.Completed)
            {
                if (!State.StartClicked)
                {
                    drawStartGame(0);
                }
            }
            else
            {
                State.StartGameAnimation.Render(mainLayer);
            }


            mainLayer.End();
        }

        public void Destroy()
        {
        }

        public void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Tick(elapsedGameTime);
            State.MenuAnimation.Tick(elapsedGameTime);
            State.StartGameAnimation.Tick(elapsedGameTime);
        }

        public void Init()
        {
            mainLayer = Renderer.CreateLayer(Layout);
            Renderer.AddLayer(mainLayer);
            State.SelectedNumberOfPlayers = 1;

            TouchManager = new TouchManager(game.Client);

            for (int j = 0; j < 6; j++)
            {
                TouchManager.PushClickRect(new TouchRect(State.Positions.NumberOfPlayersPositions[j].X, State.Positions.NumberOfPlayersPositions[j].Y, State.Positions.ButtonSize.X, State.Positions.ButtonSize.Y, numberOfPlayersClick, state: j + 1, pointIsCenter: true));
            }

            TouchManager.PushClickRect(new TouchRect(State.Positions.BackPosition.X, State.Positions.BackPosition.Y, 102, 113, backClick));

            //            TouchManager.PushClickRect(new TouchRect(State.Positions.ModesButtonPosition.X - 692, State.Positions.ModesButtonPosition.Y - 107, 1384, 215, changeModeClick));
            TouchManager.PushClickRect(new TouchRect(State.Positions.StartGamePosition.X, State.Positions.StartGamePosition.Y, Layout.Width, 257, startGameClick));

            State.StartClicked = false;
            State.StartGameAnimation = MotionManager.StartMotion(0, -400)
                .Motion(new WaitMotion(500))
                .Motion(new AnimationMotion(0, 0, 500, AnimationEasing.CubicEaseIn))
                .Motion(new WaitMotion(500))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { drawStartGame(posY); })
                .OnComplete(() =>
                {
                    if (State.StartClicked)
                    {
                        if (!HasMultiplayer)
                        {
                            State.SelectedNumberOfPlayers = 1;
                        }

                        State.ScreenManager.StartGame(State.SelectedNumberOfPlayers, State.SelectedMode);
                    }
                });

            State.MenuAnimation = MotionManager.StartMotion(-1500, 0)
                .Motion(new AnimationMotion(0, 0, 800, AnimationEasing.CubicEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { drawMenu(posX); });

            GameService.CloudSubLayout.InitLayoutView(TouchManager);
        }

        private bool backClick(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.StartClicked) return false;
            if (eventtype == TouchType.TouchDown)
            {
                if (State.MenuAnimation.Completed)
                {
                    game.Client.PlaySoundEffect(SoundEffects.Click);
                    GameService.CloudSubLayout.SlideLeft();
                    State.ScreenManager.ChangeToLanding();
                }
            }
            return false;
        }

        private int tickCount = 0;

        private bool HasMultiplayer
        {
            get { return GameService.HasMultiplayer; }
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            GameService.CloudSubLayout.TickLayoutView(elapsedGameTime);
 
        }


        private void drawStartGame(double yOffset)
        {
            mainLayer.Save();
            mainLayer.Translate(0, -yOffset);
            mainLayer.DrawRectangle(new Color(37, 170, 255), State.Positions.StartGameRect);
            mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._120), "Start Game", State.Positions.StartGameRect.Center);
            mainLayer.Restore();
        }

        private void drawMenu(double xOffset)
        {
            mainLayer.Save();
            mainLayer.Translate(-xOffset, 0);


            //            mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.TextBoard), State.Positions.ModePosition, true);
            //            mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._100), "Mode", State.Positions.ModePosition);


            switch (State.SelectedMode)
            {
                case GameMode.Classic:

                    mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.TextBoard), State.Positions.NumberOfPlayersPosition, true);
                    mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._100), "Number Of Players", State.Positions.NumberOfPlayersPosition);

                    break;
                case GameMode.Puzzle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mainLayer.Restore();

            mainLayer.Save();
            mainLayer.Translate(xOffset, 0);

            /*
                        mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.ModeButton), State.Positions.ModesButtonPosition, true);
                        switch (State.SelectedMode)
                        {
                            case GameMode.Classic:
                                mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._100), "Classic", State.Positions.ModesButtonPosition, Images.Layouts.DarkFontColor);
                                break;
                            case GameMode.Puzzle:
                                mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._100), "Puzzle", State.Positions.ModesButtonPosition, Images.Layouts.DarkFontColor);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }*/






            switch (State.SelectedMode)
            {
                case GameMode.Classic:

                    for (int i = 1; i <= 6; i++)
                    {
                        Point numberOfPlayersPosition = State.Positions.NumberOfPlayersPositions[i - 1];

                        if (i > 1 && !HasMultiplayer)
                        {
                            mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.PlayerLocked), numberOfPlayersPosition, true);
                            mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._120), i.ToString(), numberOfPlayersPosition, Images.Layouts.DarkFontColor);
                        }
                        else
                        {
                            if (i == State.SelectedNumberOfPlayers)
                            {
                                mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.PlayerSelectionButton), numberOfPlayersPosition, true);
                                mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._120), i.ToString(), numberOfPlayersPosition);
                            }
                            else
                            {
                                mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.PlayerUnselectedButton), numberOfPlayersPosition, true);
                                mainLayer.DrawString(Renderer.GetFont(Fonts.BabyDoll._120), i.ToString(), numberOfPlayersPosition, Images.Layouts.DarkFontColor);
                            }
                        }
                    }
                    break;
                case GameMode.Puzzle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            mainLayer.DrawImage(AssetManager.GetImage(Images.Layouts.BackButton), State.Positions.BackPosition);


            mainLayer.Restore();
        }


        private bool startGameClick(TouchType eventType, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.StartClicked) return false;
            switch (eventType)
            {
                case TouchType.TouchDown:
                    if (State.MenuAnimation.Completed)
                    {
                        game.Client.PlaySoundEffect(SoundEffects.Click);
                        State.StartClicked = true;
                        State.MenuAnimation.Reverse();
                        State.StartGameAnimation.Reverse();
                        State.MenuAnimation.Restart();
                        State.StartGameAnimation.Restart();
                    }
                    break;
            }
            return false;
        }

        private bool changeModeClick(TouchType eventType, TouchRect touchbox, int x, int y, bool collide)
        {
            switch (eventType)
            {
                case TouchType.TouchDown:
                    if (State.MenuAnimation.Completed)
                    {
                        game.Client.PlaySoundEffect(SoundEffects.Click);
                        State.SelectedMode = (State.SelectedMode == GameMode.Classic) ? GameMode.Puzzle : GameMode.Classic;
                    }
                    break;
            }
            return false;
        }

        public bool numberOfPlayersClick(TouchType eventType, TouchRect touchBox, int x, int y, bool collide)
        {
            if (State.StartClicked) return false;
            switch (eventType)
            {
                case TouchType.TouchDown:
                    if (State.MenuAnimation.Completed)
                    {
                        game.Client.PlaySoundEffect(SoundEffects.Click);

                        var selectedNumberOfPlayers = (int)touchBox.State;
                        if (selectedNumberOfPlayers == 1)
                        {
                            State.SelectedNumberOfPlayers = selectedNumberOfPlayers;
                        }
                        else
                        {
                            if (HasMultiplayer)
                            {
                                State.SelectedNumberOfPlayers = selectedNumberOfPlayers;
                            }
                            else
                            {
//                                game.Client.ClientSettings.PurchaseProduct(GameService.Products[0]);
                            }
                        }

                    }
                    break;
            }
            return false;
        }
    }

    public class SettingsAreaLayoutState
    {
        public SettingsAreaLayoutState(ILayout layout)
        {
            Layout = layout;
            Positions = new SettingsAreaLayoutStatePositions(layout);
        }

        public ScreenTransitioner ScreenManager { get; set; }
        public GameMode SelectedMode { get; set; }
        public int SelectedNumberOfPlayers { get; set; }
        public ILayout Layout { get; set; }
        public SettingsAreaLayoutStatePositions Positions { get; set; }
        public MotionManager StartGameAnimation { get; set; }
        public MotionManager MenuAnimation { get; set; }
        public bool StartClicked { get; set; }
    }

    public class SettingsAreaLayoutStatePositions
    {
        public Point ButtonSize = new Point(213, 213);

        public SettingsAreaLayoutStatePositions(ILayout layout)
        {
            Layout = layout;
            NumberOfPlayersPosition = new Point(740, 595);


            NumberOfPlayersPositions = new Point[6];

            for (int i = 0; i < 6; i++)
            {
                NumberOfPlayersPositions[i] = new Point(185 + 232 * i, 865);
            }

            ModePosition = new Point(762, 203);
            ModesButtonPosition = new Point(774, 479);
            StartGamePosition = new Point(0, Layout.Height - 256);
            StartGameRect = new Rectangle(StartGamePosition.X, StartGamePosition.Y, Layout.Width, 256);

            BackPosition = new Point(0, StartGamePosition.Y - 122);
        }

        public ILayout Layout { get; set; }

        public Point StartGamePosition { get; set; }
        public Point ModesButtonPosition { get; set; }
        public Point ModePosition { get; set; }
        public Point[] NumberOfPlayersPositions { get; set; }
        public Point NumberOfPlayersPosition { get; set; }
        public Rectangle StartGameRect { get; set; }

        public Point BackPosition { get; set; }
    }
}