using System;
using Engine;
using Engine.Animation;
using Engine.Interfaces;
using PenguinShuffle.BoardArea;
using PenguinShuffle.SubLayoutViews;
using PenguinShuffle.Utils;

namespace PenguinShuffle.LandingArea
{
    public class LandingAreaLayout : BaseLayoutView
    {
        private readonly ScreenTransitioner _screenTransitioner;
        private ILayer mainLayer;

        public LandingAreaLayout( GameService gameService,   ScreenTransitioner screenTransitioner)
        {
            _screenTransitioner = screenTransitioner;
            GameService = gameService;
        }

        public LandingAreaLayoutState State { get; set; }

        public LandingAreaLayoutStatePositions Positions { get; set; }


        public MotionManager AboutOpenDialogAnimation { get; set; }
        public MotionManager AboutCloseDialogAnimation { get; set; }
        public MotionManager AboutOpenAnimation { get; set; }
        public MotionManager AboutCloseAnimation { get; set; }

        public MotionManager PenguinLogoAnimation { get; set; }
        public MotionManager ShuffleLogoAnimation { get; set; }
        public MotionManager LeavePenguinLogoAnimation { get; set; }
        public MotionManager LeaveShuffleLogoAnimation { get; set; }
        public MotionManager RandomPenguinHop { get; set; }


        public GameService GameService { get; set; }

        public MotionManager InitialPenguinHop { get; set; }

        public MotionManager PlayButtonAnimation { get; set; }
        public MotionManager StartGameAnimation { get; set; }

        public override void Render(TimeSpan elapsedGameTime)
        {
            mainLayer.Begin();


            GameService.CloudSubLayout.Render(mainLayer);
            State.SoundSubLayout.Render(mainLayer);

            mainLayer.Save();

            IImage logoImage = Assets.Images.Layouts.PenguinLogo;
            IImage shuffleImage = Assets.Images.Layouts.ShuffleLogo;

            if (PenguinLogoAnimation.Completed)
            {
                mainLayer.DrawImage(logoImage, Positions.PenguinLogoLocation, true);
            }
            else
            {
                PenguinLogoAnimation.Render(mainLayer);
            }

            if (ShuffleLogoAnimation.Completed)
            {
                mainLayer.DrawImage(shuffleImage, Positions.ShuffleLogoLocation, true);
            }
            else
            {
                ShuffleLogoAnimation.Render(mainLayer);
            }


            if (!PlayButtonAnimation.Completed)
            {
                PlayButtonAnimation.Render(mainLayer);
            }

            else
            {
                if (State.ShowingTutorial > 0)
                {
                    mainLayer.DrawImage(Assets.Images.Layouts.Tutorials.Tutorial[State.ShowingTutorial], Positions.TutorialPosition, true);
                }
                else
                {
                    if (!State.StartClicked)
                    {
                        mainLayer.DrawImage(Assets.Images.Layouts.PlayButton, Positions.StartLocation, true);
                    }
                }
            }

            if (State.ShowingTutorial > 0)
            {
                mainLayer.DrawImage(Assets.Images.Layouts.Tutorials.Tutorial[State.ShowingTutorial], Positions.TutorialPosition, true);
            }
            else
            {
                mainLayer.DrawImage(Assets.Images.Layouts.AboutButton, Positions.TutorialButtonPosition, true);

                if (State.StartClicked)
                {
                    StartGameAnimation.Render(mainLayer);
                }
                else
                {
                    renderAbout();
                }
            }

            mainLayer.Restore();


            //            TouchManager.Render(mainLayer);
            mainLayer.End();
        }

        public override void Destroy()
        {
        }

        public override void InitLayoutView()
        {
            Client.PlaySong(Assets.Songs.MenuMusic);
            Positions = new LandingAreaLayoutStatePositions(Layout);
            Init();
        }

        public override void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Tick(elapsedGameTime);
        }

        public void Init()
        {
            State = new LandingAreaLayoutState();
            State.SoundSubLayout = new SoundSubLayout( Client);

            setNextHop();
            GameService.CloudSubLayout.InitLayoutView(TouchManager);
            GameService.CloudSubLayout.SlideLeft();
            mainLayer = Renderer.CreateLayer(Layout.Width, Layout.Height, Layout);
            Renderer.AddLayer(mainLayer);


            TouchManager.PushClickRect(new TouchRect(Positions.StartLocation.X, Positions.StartLocation.Y, 1300, 324, startGame, true));
            TouchManager.PushClickRect(new TouchRect(Positions.TutorialButtonPosition.X, Positions.TutorialButtonPosition.Y, 200, 200, tutorialTrigger, true));

            TouchManager.PushClickRect(new TouchRect(Positions.AboutBubblePosition + Positions.AboutContactUsPosition, Positions.AboutButtonSize, aboutContactTouch));
            TouchManager.PushClickRect(new TouchRect(Positions.AboutBubblePosition + Positions.AboutRatePosition, Positions.AboutButtonSize, aboutRateTouch));

            TouchManager.PushClickRect(new TouchRect(Positions.AboutOpenPenguinPosition, 533, 512, toggleOpening, true));


            TouchManager.PushClickRect(new TouchRect(0, 0, 1536, 2048, closeBox));

            IImage logoImage = Assets.Images.Layouts.PenguinLogo;
            IImage shuffleImage = Assets.Images.Layouts.ShuffleLogo;

            PenguinLogoAnimation = MotionManager.StartMotion(-1000, Positions.PenguinLogoLocation.Y)
                .Motion(new WaitMotion(300))
                .Motion(new AnimationMotion(Positions.PenguinLogoLocation.X, Positions.PenguinLogoLocation.Y, 500, AnimationEasing.SineEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => mainLayer.DrawImage(logoImage, posX, posY, true))
                .OnComplete(() => { });

            ShuffleLogoAnimation = MotionManager.StartMotion(-1000, Positions.ShuffleLogoLocation.Y)
                .Motion(new WaitMotion(300))
                .Motion(new AnimationMotion(Positions.ShuffleLogoLocation.X, Positions.ShuffleLogoLocation.Y, 600, AnimationEasing.SineEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => mainLayer.DrawImage(shuffleImage, posX, posY, true))
                .OnComplete(() => { });

            PlayButtonAnimation = MotionManager.StartMotion(-1000, Positions.StartLocation.Y)
                .Motion(new WaitMotion(300))
                .Motion(new AnimationMotion(Positions.StartLocation, 700, AnimationEasing.SineEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => mainLayer.DrawImage(Assets.Images.Layouts.PlayButton, posX, posY, true))
                .OnComplete(() => { });


            State.AboutState = AboutState.Closed;


            IImage penguinImage = Assets.Images.About.MainPenguin;
            AboutOpenDialogAnimation = MotionManager.StartMotion(0, 0)
                .Motion(new AnimationMotion(0, 100, 600, AnimationEasing.Linear))
                .OnRender((layer, posX, posY, animationIndex, percentDone) =>
                {
                    layer.Save();
                    layer.SetDrawingTransparency(posY / 100f);
                    aboutBoxRender(layer);
                    layer.Restore();
                });

            RandomPenguinHop = MotionManager.StartMotion(Positions.AboutPenguinPosition)
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y - 25, 200, AnimationEasing.CubicEaseOut))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition, 150, AnimationEasing.CubicEaseIn))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y - 50, 200, AnimationEasing.CubicEaseOut))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition, 150, AnimationEasing.CubicEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(penguinImage, posX, posY, true); })
                .DontStart();


            InitialPenguinHop = MotionManager.StartMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y + 1000)
                .Motion(new WaitMotion(300))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y - 25, 1000, AnimationEasing.CubicEaseOut))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition, 150, AnimationEasing.CubicEaseIn))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y - 50, 200, AnimationEasing.CubicEaseOut))
                .Motion(new AnimationMotion(Positions.AboutPenguinPosition, 150, AnimationEasing.CubicEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(penguinImage, posX, posY, true); });

            AboutCloseDialogAnimation = MotionManager.StartMotion(0, 100)
                .Motion(new AnimationMotion(0, 0, 600, AnimationEasing.Linear))
                .OnRender((layer, posX, posY, animationIndex, percentDone) =>
                {
                    layer.Save();
                    layer.SetDrawingTransparency(posY / 100f);
                    aboutBoxRender(layer);
                    layer.Restore();
                });

            AboutOpenAnimation = MotionManager.StartMotion(Positions.AboutPenguinPosition,
                new AnimationMotion(Positions.AboutOpenPenguinPosition, 600, AnimationEasing.BounceEaseOut))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(penguinImage, posX, posY, true); }).OnComplete(() =>
                {
                    setNextHop();
                    State.AboutState = AboutState.Opened;
                });

            AboutCloseAnimation = MotionManager.StartMotion(Positions.AboutOpenPenguinPosition,
                new AnimationMotion(Positions.AboutPenguinPosition, 400, AnimationEasing.CubicEaseIn))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(penguinImage, posX, posY, true); }).OnComplete(() =>
                {
                    setNextHop();
                    State.AboutState = AboutState.Closed;
                });

            StartGameAnimation = MotionManager.StartMotion(Positions.AboutPenguinPosition).
                Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, 1300, 400, AnimationEasing.QuadEaseOut)).
                Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y + 100, 350, AnimationEasing.QuadEaseIn)).
                Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, 700, 400, AnimationEasing.QuadEaseOut)).
                Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, Positions.AboutPenguinPosition.Y + 200, 350, AnimationEasing.QuadEaseIn)).
                Motion(new AnimationMotion(Positions.AboutPenguinPosition.X, -500, 500, AnimationEasing.QuadEaseOut))
                .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(penguinImage, posX, posY, true); }).OnComplete(() =>
                {
                    GameService.CloudSubLayout.SlideRight();
                    _screenTransitioner.ChangeToSettingsScreen();
                });


            State.SoundSubLayout.InitLayoutView(TouchManager);
        }

        private void setNextHop()
        {
            State.NextHop = new TimeSpan(0, 0, 0, 0, RandomUtil.RandomInt(1700, 3500));
            State.StartHop = DateTime.Now;
        }


        private void aboutBoxRender(ILayer layer)
        {
            layer.Save();
            layer.Translate(Positions.AboutBubblePosition);

            layer.DrawImage(Assets.Images.About.AboutBubble);
            layer.DrawImage(Assets.Images.About.AboutContact, Positions.AboutContactUsPosition);
            layer.DrawImage(Assets.Images.About.AboutRate, Positions.AboutRatePosition);

            layer.Restore();
        }

        private bool aboutContactTouch(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.AboutState != AboutState.Opened) return true;
            if (eventtype == TouchType.TouchDown)
            {
                Client.PlaySoundEffect(Assets.Sounds.Click);
                Client.ClientSettings.SendEmail(BoardConstants.EmailAddress, BoardConstants.EmailSubject, BoardConstants.EmailMessage);
                return false;
            }
            return true;
        }

        private bool aboutRateTouch(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.AboutState != AboutState.Opened) return true;
            if (eventtype == TouchType.TouchDown)
            {
                Client.PlaySoundEffect(Assets.Sounds.Click);
                Client.ClientSettings.OpenAppStore();
                return false;
            }
            return true;
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
            if (DateTime.Now - State.StartHop > State.NextHop)
            {
                setNextHop();
                RandomPenguinHop.Restart();
            }

            if (!PenguinLogoAnimation.Completed)
            {
                PenguinLogoAnimation.Tick(elapsedGameTime);
            }
            if (!ShuffleLogoAnimation.Completed)
            {
                ShuffleLogoAnimation.Tick(elapsedGameTime);
            }
            if (!RandomPenguinHop.Completed)
            {
                RandomPenguinHop.Tick(elapsedGameTime);
            }
            if (!PlayButtonAnimation.Completed)
            {
                PlayButtonAnimation.Tick(elapsedGameTime);
            }
            if (!InitialPenguinHop.Completed)
            {
                InitialPenguinHop.Tick(elapsedGameTime);
            }

            switch (State.AboutState)
            {
                case AboutState.Closed:
                    break;
                case AboutState.Opened:
                    break;
                case AboutState.Opening:
                    State.NextHop = new TimeSpan(1, 0, 0);
                    AboutOpenAnimation.Tick(elapsedGameTime);
                    AboutOpenDialogAnimation.Tick(elapsedGameTime);
                    break;
                case AboutState.Closing:
                    State.NextHop = new TimeSpan(1, 0, 0);
                    AboutCloseAnimation.Tick(elapsedGameTime);
                    AboutCloseDialogAnimation.Tick(elapsedGameTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (State.StartClicked)
            {
                StartGameAnimation.Tick(elapsedGameTime);
            }
            GameService.CloudSubLayout.TickLayoutView(elapsedGameTime);
            State.SoundSubLayout.TickLayoutView(elapsedGameTime);
        }

        public bool toggleOpening(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.StartClicked) return false;
            if (eventtype == TouchType.TouchDown)
            {
                if (State.AboutState == AboutState.Opened)
                {
                    Client.PlaySoundEffect(Assets.Sounds.Click);
                    State.AboutState = AboutState.Closing;
                    AboutCloseAnimation.Restart();
                    AboutCloseDialogAnimation.Restart();
                }
                else if (State.AboutState == AboutState.Closed)
                {
                    Client.PlaySoundEffect(Assets.Sounds.Click);
                    State.AboutState = AboutState.Opening;
                    AboutOpenAnimation.Restart();
                    AboutOpenDialogAnimation.Restart();
                }

                return true;
            }
            return false;
        }


        private void renderAbout()
        {
            if (!InitialPenguinHop.Completed)
            {
                InitialPenguinHop.Render(mainLayer);
                return;
            }
            switch (State.AboutState)
            {
                case AboutState.Closed:

                    if (!RandomPenguinHop.Completed)
                    {
                        RandomPenguinHop.Render(mainLayer);
                    }
                    else
                    {
                        mainLayer.DrawImage(Assets.Images.About.MainPenguin, Positions.AboutPenguinPosition, true);
                    }

                    break;
                case AboutState.Opening:
                    AboutOpenAnimation.Render(mainLayer);
                    AboutOpenDialogAnimation.Render(mainLayer);
                    break;
                case AboutState.Opened:
                    mainLayer.DrawImage(Assets.Images.About.MainPenguin, Positions.AboutOpenPenguinPosition, true);
                    aboutBoxRender(mainLayer);
                    break;
                case AboutState.Closing:
                    AboutCloseAnimation.Render(mainLayer);
                    AboutCloseDialogAnimation.Render(mainLayer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private bool closeBox(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (State.ShowingTutorial > 0)
                {
                    State.ShowingTutorial++;
                    Client.PlaySoundEffect(Assets.Sounds.Click);

                    if (State.ShowingTutorial > 2)
                    {
                        State.ShowingTutorial = 0;
                    }
                    return false;
                }
                if (State.AboutState == AboutState.Opened)
                {
                    Client.PlaySoundEffect(Assets.Sounds.Click);
                    State.AboutState = AboutState.Closing;
                    AboutCloseAnimation.Restart();
                    AboutCloseDialogAnimation.Restart();
                    return false;
                }
                return true;
            }
            return true;
        }


        private bool tutorialTrigger(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (State.StartClicked || !PlayButtonAnimation.Completed) return false;
                Client.PlaySoundEffect(Assets.Sounds.Click);
                State.ShowingTutorial = 1;
                return false;
            }
            return true;
        }

        private bool startGame(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.ShowingTutorial != 0) return true;
            switch (eventtype)
            {
                case TouchType.TouchDown:
                    if (State.StartClicked) return false;
                    Client.PlaySoundEffect(Assets.Sounds.Click);
                    State.StartClicked = true;

                    IImage logoImage = Assets.Images.Layouts.PenguinLogo;
                    IImage shuffleImage = Assets.Images.Layouts.ShuffleLogo;

                    PenguinLogoAnimation = MotionManager.StartMotion(Positions.PenguinLogoLocation)
                        .Motion(new AnimationMotion(Layout.Width + 1000, Positions.PenguinLogoLocation.Y, 1000, AnimationEasing.SineEaseIn))
                        .Motion(new WaitMotion(5000))
                        .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(logoImage, posX, posY, true); })
                        .OnComplete(() => { });

                    ShuffleLogoAnimation = MotionManager.StartMotion(Positions.ShuffleLogoLocation)
                        .Motion(new AnimationMotion(Layout.Width + 1000, Positions.ShuffleLogoLocation.Y, 1400, AnimationEasing.SineEaseIn))
                        .Motion(new WaitMotion(5000))
                        .OnRender((layer, posX, posY, animationIndex, percentDone) => { mainLayer.DrawImage(shuffleImage, posX, posY, true); })
                        .OnComplete(() => { });
                    return true;
            }
            return false;
        }
    }

    public class LandingAreaLayoutState
    {
        public TimeSpan NextHop { get; set; }
        public DateTime StartHop { get; set; }
        public SoundSubLayout SoundSubLayout { get; set; }
        public int ShowingTutorial { get; set; }
        public AboutState AboutState { get; set; }
        public bool StartClicked { get; set; }
    }

    public enum AboutState
    {
        Closed,
        Opening,
        Opened,
        Closing
    }

    public class LandingAreaLayoutStatePositions
    {
        public readonly Point AboutBubblePosition;
        public readonly Point AboutBubbleSize;
        public readonly Point AboutButtonSize;
        public readonly Point AboutContactUsPosition;
        public readonly Point AboutOpenPenguinPosition;
        public readonly Point AboutPenguinPosition;
        public readonly Point AboutRatePosition;
        public readonly Point PenguinLogoLocation;
        public readonly Point ShuffleLogoLocation;
        public readonly Point StartLocation;
        public readonly Point TutorialButtonPosition;
        public readonly Point TutorialPosition;

        public LandingAreaLayoutStatePositions(BaseLayout layout)
        {
            ShuffleLogoLocation = new Point(1030, 526);
            PenguinLogoLocation = new Point(layout.Width / 2, 262);
            StartLocation = new Point(760, 950);


            TutorialButtonPosition = new Point(1410, 1930);
            TutorialPosition = new Point(768, 1024);

            AboutPenguinPosition = new Point(layout.Width / 2, layout.Height);
            AboutOpenPenguinPosition = new Point(layout.Width / 2, layout.Height - 512 / 7);

            AboutBubbleSize = new Point(830, 598);
            AboutButtonSize = new Point(324, 94);

            AboutBubblePosition = new Point(layout.Width / 2 - AboutBubbleSize.Width / 2, layout.Height - 620 - AboutBubbleSize.Height / 2);
            AboutRatePosition = new Point(600, 434) - (AboutButtonSize / 2);
            AboutContactUsPosition = new Point(230, 434) - (AboutButtonSize / 2);
        }
    }
}