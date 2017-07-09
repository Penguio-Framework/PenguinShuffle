using System;
using System.Collections.Generic;
using Engine;
using Engine.Animation;
using Engine.Interfaces;
using PenguinShuffle.SubLayoutViews;

namespace PenguinShuffle.BoardArea.Layouts
{
    public class BoardViewingAreaLayout : ILayoutView
    {
        private readonly Color buttonBlue = new Color(37, 170, 225);
        private readonly Color white = new Color(255, 255, 255);

        public BoardViewingAreaLayout(Game game, GameService gameService, IRenderer renderer, ILayout layout, ScreenTransitioner screenTransitioner)
        {
            Game = game;
            Renderer = renderer;
            GameService = gameService;
            Layout = layout;
            ScreenTransitioner = screenTransitioner;

            MainLayer = Renderer.CreateLayer(Layout);
            Renderer.AddLayer(MainLayer);

            TouchManager = new TouchManager(Game.Client);

            CharacterAnimations = new Dictionary<int, AnimatedCharacterSubLayout>();
            for (int i = 1; i <= 6; i++)
            {
                CharacterAnimations.Add(i, new AnimatedCharacterSubLayout(Game, i));
            }

        }

        public Dictionary<int, AnimatedCharacterSubLayout> CharacterAnimations;

        public Game Game { get; set; }
        public IRenderer Renderer { get; set; }
        public ILayer MainLayer { get; set; }
        public BoardViewingAreaLayoutStatePositions Positions { get; set; }
        public BoardViewingAreaLayoutState State { get; set; }
        public GameService GameService { get; set; }
        public ScreenTransitioner ScreenTransitioner { get; set; }
        
        public ILayout Layout { get; set; }
        public ITouchManager TouchManager { get; private set; }


        public void Destroy()
        {
        }

        public void InitLayoutView()
        {
            Init();
        }

        public void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Tick(elapsedGameTime);
        }


        public void Init()
        {
            State = new BoardViewingAreaLayoutState(Layout);
            Positions = new BoardViewingAreaLayoutStatePositions(Layout);

            TouchManager.ClearSwipeRect();
            TouchManager.ClearClickRect();


            int numX2 = Positions.BackLayout.BackBoxPosition.X - Positions.BackLayout.BackBoxSize.X / 2;
            int numY2 = Positions.BackLayout.BackBoxPosition.Y - Positions.BackLayout.BackBoxSize.Y / 2;

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(numX2 + Positions.BackLayout.BackContinueButtonPosition.X, numY2 + Positions.BackLayout.BackContinueButtonPosition.Y,
                Positions.BackLayout.BackContinueBoxSize.X, Positions.BackLayout.BackContinueBoxSize.Y, backContinue));

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(numX2 + Positions.BackLayout.BackCancelButtonPosition.X, numY2 + Positions.BackLayout.BackCancelButtonPosition.Y, Positions.BackLayout.BackCancelBoxSize.X, Positions.BackLayout.BackCancelBoxSize.Y, backCancel));


            TouchManager.PushClickRect(new TouchRect(Positions.BackPosition.X, Positions.BackPosition.Y, 102, 113, backClick));


            int charWidth = 170;
            int charHeight = 162;
            for (int index = 0; index < Positions.CharacterPositions.Length; index++)
            {
                Point characterPosition = Positions.CharacterPositions[index];
                TouchManager.PushClickRect(new TouchRect(characterPosition.X - charWidth / 2 + 0, characterPosition.Y - charHeight / 2 + (BoardConstants.TopAreaHeight), charWidth, charHeight, selectCharacter, state: GameService.ClassicGameState.Characters[index]));
            }


            int numX = Positions.NumberSelectionBoxPosition.X - Positions.NumberSelectionBoxSize.X / 2;
            int numY = Positions.NumberSelectionBoxPosition.Y - Positions.NumberSelectionBoxSize.Y / 2;

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(numX + Positions.NumberSelectionSelectButtonPosition.X, numY + Positions.NumberSelectionSelectButtonPosition.Y, Positions.NumberSelectionSelectBoxSize.X, Positions.NumberSelectionSelectBoxSize.Y, numberSelectionSelect));
            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(numX, numY, Positions.NumberSelectionBoxSize.X, Positions.NumberSelectionBoxSize.Y - Positions.NumberSelectionSelectBoxSize.Y, numberSelectionDrag));

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(0, 0, Layout.Width, Layout.Height, wholeBoardClick));
            Layout.LayoutView.TouchManager.PushSwipeRect(new TouchRect(0, 0, Layout.Width, Layout.Height, wholeBoardSwipe));
            State.TimePausedStart = 0;
            State.TimePaused = 0;
            State.TotalChosens = 0;
            State.CurrentNumber = 15;

            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.InitLayoutView(TouchManager);
            }
        }


        private bool backContinue(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.BackDialogVisible)
            {
                switch (eventtype)
                {
                    case TouchType.TouchDown:
                        Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                        ScreenTransitioner.ChangeToLanding();
                        break;
                }
                return false;
            }
            return true;
        }

        private bool backCancel(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.BackDialogVisible)
            {
                switch (eventtype)
                {
                    case TouchType.TouchDown:
                        Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                        State.BackDialogVisible = false;
                        if (GameService.ClassicGameState.GameState == GameSelectionState.TimerStarted)
                        {
                            State.CurrentTick = Game.Client.PlaySoundEffect(Assets.Sounds.TimeTick, true);
                        }
                        break;
                }
                return false;
            }
            return true;
        }


        private bool backClick(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (State.CurrentTick != null) State.CurrentTick.Stop();

                Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                State.BackDialogVisible = true;
            }
            return false;
        }

        private void drawBackDialog(TimeSpan elapsedGameTime)
        {
            MainLayer.Save();
            GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer, disabled: true);

            MainLayer.DrawRectangle(new Color(10, 10, 10, 180), 0, 0, BoardConstants.TotalWidth, BoardConstants.TotalHeight);


            var nsBoard = new Rectangle(Positions.BackLayout.BackBoxPosition.X, Positions.BackLayout.BackBoxPosition.Y, Positions.BackLayout.BackBoxSize.X, Positions.BackLayout.BackBoxSize.Y, true);
            MainLayer.DrawRectangle(white, nsBoard, true);

            MainLayer.Translate(nsBoard.X, nsBoard.Y);

            MainLayer.DrawString((Assets.Fonts.BabyDoll._72), "Are you sure you would like\nto go back?\nYour progress will be lost.", Positions.BackLayout.BackTextPosition.X, Positions.BackLayout.BackTextPosition.Y, Colors.DarkFontColor);


            var cancelButtonRect = new Rectangle(Positions.BackLayout.BackCancelButtonPosition.X, Positions.BackLayout.BackCancelButtonPosition.Y, Positions.BackLayout.BackCancelBoxSize.X, Positions.BackLayout.BackCancelBoxSize.Y);
            MainLayer.DrawRectangle(buttonBlue, cancelButtonRect);
            MainLayer.DrawString((Assets.Fonts.BabyDoll._100), "Cancel", cancelButtonRect.Center.X, cancelButtonRect.Center.Y);


            var continueButtonRect = new Rectangle(Positions.BackLayout.BackContinueButtonPosition.X, Positions.BackLayout.BackContinueButtonPosition.Y, Positions.BackLayout.BackContinueBoxSize.X, Positions.BackLayout.BackContinueBoxSize.Y);
            MainLayer.DrawRectangle(buttonBlue, continueButtonRect);

            MainLayer.DrawString((Assets.Fonts.BabyDoll._100), "Continue", continueButtonRect.Center.X, continueButtonRect.Center.Y);

            MainLayer.Restore();
        }


        private bool wholeBoardSwipe(TouchRect touchbox, int startx, int starty, Point direction, double distance)
        {
            if (State.BackDialogVisible) return true;
            if (GameService.ClassicGameState.GameState == GameSelectionState.ChooseNumber)
            {
                State.SwipeDistance = direction.Y;
                if (State.SwipeDistance > 0)
                {
                    State.SwipeAcceleration = -10;
                }
                else
                {
                    State.SwipeAcceleration = 10;
                }
            }
            return false;
        }


        private bool wholeBoardClick(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            switch (eventtype)
            {
                case TouchType.TouchMove:
                    if (State.BackDialogVisible) return true;
                    if (State.NSDragMouseDownPosition != null)
                    {
                        int i1 = Positions.NumberSelectionBoxPosition.Y - Positions.NumberSelectionBoxSize.Y / 2;

                        State.CurrentNumber = Math.Max(Math.Min(State.NSDragMouseDownStartingNumber - ((y - i1) - State.NSDragMouseDownPosition.Y) / 50, 60), 1);
                    }
                    break;
                case TouchType.TouchDown:
                    if (State.BackDialogVisible)
                    {
                        Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                        State.BackDialogVisible = false;
                        return true;
                    }


                    closeNumberSelect();
                    break;
            }
            return true;
        }

        private bool selectCharacter(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.BackDialogVisible)
                return true;
            switch (eventtype)
            {
                case TouchType.TouchUp:
                    if (!collide) return false;
                    Game.Client.PlaySoundEffect(Assets.Sounds.Click);

                    if (closeNumberSelect())
                    {
                        return false;
                    }

                    switch (GameService.ClassicGameState.GameState)
                    {
                        case GameSelectionState.SearchingBoard:
                        case GameSelectionState.TimerStarted:
                            var currentCharacter = ((Character)touchbox.State);
                            if (currentCharacter.Playing)
                            {
                                State.CurrentNumber = 15;
                                State.CurrentCharacter = currentCharacter;
                                GameService.ClassicGameState.GameState = GameSelectionState.ChooseNumber;
                                State.CurrentCharacter.Selected = true;
                                State.TimePausedStart = DateTime.Now.Ticks;
                                if (State.CurrentTick != null) State.CurrentTick.Stop();
                            }
                            break;
                    }
                    return false;
            }
            return true;
        }

        private bool closeNumberSelect()
        {
            if (State.BackDialogVisible)
                return true;

            if (GameService.ClassicGameState.GameState == GameSelectionState.ChooseNumber)
            {
                State.CurrentCharacter.Selected = false;

                if (GameService.ClassicGameState.ChosenNumbers.Count == 0)
                {
                    GameService.ClassicGameState.GameState = GameSelectionState.SearchingBoard;
                    if (State.CurrentTick != null) State.CurrentTick.Stop();
                }
                else
                {
                    GameService.ClassicGameState.GameState = GameSelectionState.TimerStarted;
                    State.CurrentTick = Game.Client.PlaySoundEffect(Assets.Sounds.TimeTick, true);
                }
                return true;
            }
            return false;
        }

        private bool numberSelectionDrag(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.BackDialogVisible)
                return true;

            if (GameService.ClassicGameState.GameState == GameSelectionState.ChooseNumber)
            {
                switch (eventtype)
                {
                    case TouchType.TouchUp:
                        State.NSDragMouseDownPosition = null;
                        break;
                    case TouchType.TouchDown:
                        Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                        State.SwipeDistance = 0;
                        State.NSDragMouseDownPosition = new Point(x, y);
                        State.NSDragMouseDownStartingNumber = State.CurrentNumber;
                        break;
                    case TouchType.TouchMove:
                        if (State.NSDragMouseDownPosition != null)
                        {
                            State.CurrentNumber = Math.Max(Math.Min(State.NSDragMouseDownStartingNumber - (y - State.NSDragMouseDownPosition.Y) / 50, 60), 1);
                        }
                        break;
                }
                return false;
            }
            return true;
        }

        private bool numberSelectionSelect(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.BackDialogVisible)
                return true;

            if (GameService.ClassicGameState.GameState == GameSelectionState.ChooseNumber)
            {
                switch (eventtype)
                {
                    case TouchType.TouchDown:
                        Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                        if (GameService.ClassicGameState.TimeStarted == 0)
                        {
                            GameService.ClassicGameState.TimeStarted = DateTime.Now.Ticks;
                        }
                        else
                        {
                            State.TimePaused += DateTime.Now.Ticks - State.TimePausedStart;
                        }

                        GameService.ClassicGameState.GameState = GameSelectionState.TimerStarted;
                        State.CurrentTick = Game.Client.PlaySoundEffect(Assets.Sounds.TimeTick, true);
                        State.TotalChosens++;
                        State.CurrentCharacter.Selected = false;
                        foreach (ChosenNumber chosenNumber in GameService.ClassicGameState.ChosenNumbers)
                        {
                            if (chosenNumber.Character == State.CurrentCharacter)
                            {
                                chosenNumber.Number = State.CurrentNumber;
                                chosenNumber.OrderChosenIn = State.TotalChosens;
                                resetNumberSelect();
                                return false;
                            }
                        }
                        GameService.ClassicGameState.ChosenNumbers.Add(new ChosenNumber
                        {
                            Number = State.CurrentNumber,
                            Character = State.CurrentCharacter,
                            OrderChosenIn = State.TotalChosens
                        });
                        resetNumberSelect();
                        return false;
                }
            }
            return true;
        }

        private void resetNumberSelect()
        {
            State.SwipeDistance = 0;
            State.CurrentNumber = 15;
            List<EnumerableExtensions.GroupByItem<ChosenNumber, int>> items = GameService.ClassicGameState.ChosenNumbers.GroupBy(a => a.Number);
            items.Sort((number, cn) => cn.Key.CompareTo(number.Key));
            foreach (var item in items)
            {
                item.Values.Sort((number, cn) => cn.OrderChosenIn.CompareTo(number.OrderChosenIn));
            }
            GameService.ClassicGameState.ChosenNumbers = items.SelectMany(a => a.Values);
            if (GameService.ClassicGameState.ChosenNumbers.Count == GameService.ClassicGameState.Characters.Count(a => a.Playing))
            {
                if (State.CurrentTick != null) State.CurrentTick.Stop();
                ScreenTransitioner.ChangeToBoardScreen();
            }
        }

        public void Render(TimeSpan elapsedGameTime)
        {
            MainLayer.Begin();
            MainLayer.Save();

            MainLayer.DrawRectangle(new Color(252, 252, 252), 0, 0, MainLayer.Layout.Width, MainLayer.Layout.Height);

            if (GameService.ClassicGameState.GameState != GameSelectionState.ChooseNumber)
            {
                GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer);
            }
            else
            {
                drawNumberSelection(elapsedGameTime);
            }


            MainLayer.Translate(0, BoardConstants.TopAreaHeight);

            MainLayer.DrawImage(Assets.Images.Layouts.StagingArea, 0, 0);

            drawBottomInfo();

            drawBottomChoosePlayers();
            MainLayer.Restore();

            MainLayer.DrawImage(Assets.Images.Layouts.BackButton, Positions.BackPosition);


            if (State.BackDialogVisible)
            {
                drawBackDialog(elapsedGameTime);
            }

            TouchManager.Render(MainLayer);
            MainLayer.End();
        }


        private void drawNumberSelection(TimeSpan elapsedGameTime)
        {
            MainLayer.Save();
            GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer, disabled: true);
            MainLayer.DrawRectangle(new Color(10, 10, 10, 120), 0, 0, BoardConstants.TotalWidth, BoardConstants.TopAreaHeight);


            var nsBoard = new Rectangle(Positions.NumberSelectionBoxPosition.X, Positions.NumberSelectionBoxPosition.Y, Positions.NumberSelectionBoxSize.X, Positions.NumberSelectionBoxSize.Y, true);
            MainLayer.DrawRectangle(white, nsBoard);

            MainLayer.Translate(nsBoard.X, nsBoard.Y);


            var selectButtonRect = new Rectangle(Positions.NumberSelectionSelectButtonPosition.X, Positions.NumberSelectionSelectButtonPosition.Y, Positions.NumberSelectionSelectBoxSize.X, Positions.NumberSelectionSelectBoxSize.Y);
            MainLayer.DrawRectangle(buttonBlue, selectButtonRect);

            MainLayer.DrawString((Assets.Fonts.BabyDoll._100), "Select", selectButtonRect.Center.X, selectButtonRect.Center.Y);

            IImage barImage = Assets.Images.Layouts.NumberSelectBar;
            MainLayer.DrawImage(barImage, Positions.TopBarPosition, true);

            MainLayer.DrawImage(barImage, Positions.BottomBarPosition, true);

            for (int i = Math.Max(State.CurrentNumber - 2, 1); i <= Math.Min(State.CurrentNumber + 2, 60); i++)
            {
                Point numberPosition = null;
                Color color = null;
                switch (i - State.CurrentNumber + 2)
                {
                    case 0:
                        color = Positions.DarkColorLighter;
                        numberPosition = Positions.NSFirstShadowPosition.Center;
                        break;
                    case 1:
                        color = Positions.DarkColorLight;
                        numberPosition = Positions.NSSecondShadowPosition.Center;
                        break;
                    case 2:
                        color = Positions.DarkColor;
                        numberPosition = Positions.NSMiddleNumberPosition;
                        break;
                    case 3:
                        color = Positions.DarkColorLight;
                        numberPosition = Positions.NSThirdShadowPosition.Center;
                        break;
                    case 4:
                        color = Positions.DarkColorLighter;
                        numberPosition = Positions.NSForthShadowPosition.Center;
                        break;
                }
                MainLayer.DrawString((Assets.Fonts.MyriadPro._100), i.ToString(), numberPosition, color);
            }
            MainLayer.Restore();

        }

        private void drawBottomInfo()
        {
            IImage baseImage = GoalPiece.GetBaseImage(GameService.ClassicGameState.CurrentGoal);
            IImage goalImage = GoalPiece.GetGoalImage( GameService.ClassicGameState.CurrentGoal);

            IImage goalArrow = Assets.Images.Layouts.Arrow;
            IImage smallGoalArrow = Assets.Images.Layouts.Arrow;
            if ((GameService.ClassicGameState.GameState == GameSelectionState.ChooseNumber && GameService.ClassicGameState.ChosenNumbers.Count == 0) || GameService.ClassicGameState.GameState == GameSelectionState.SearchingBoard)
            {
                MainLayer.DrawImage(Assets.Images.Layouts.HelperImage1, Positions.Help1Position, true);

                MainLayer.DrawImage(goalArrow, Positions.CardArrowPosition.X, Positions.CardArrowPosition.Y, true);

                MainLayer.DrawImage(baseImage, Positions.CardCharacterPosition.X, Positions.CardCharacterPosition.Y, 192, 192, true);

                MainLayer.DrawImage(goalImage, Positions.CardGoalPosition.X, Positions.CardGoalPosition.Y, 192, 192, true);

                MainLayer.DrawImage(Assets.Images.Layouts.HelperImage2, Positions.Help2Position, true);
            }
            else
            {
                int index = 0;
                for (int chosenNumberIndex = Math.Max(GameService.ClassicGameState.ChosenNumbers.Count - 4, 0); chosenNumberIndex < GameService.ClassicGameState.ChosenNumbers.Count; chosenNumberIndex++)
                {
                    ChosenNumber chosenNumber = GameService.ClassicGameState.ChosenNumbers[chosenNumberIndex];
                    if (chosenNumberIndex == GameService.ClassicGameState.ChosenNumbers.Count - 1)
                    {
                        IImage flagImage = Assets.Images.Character.Banners.Big.LongBanner[chosenNumber.Character.CharacterNumber + 1];
                        MainLayer.DrawImage(flagImage, Positions.FlagLongPositions[index].X, Positions.FlagLongPositions[index].Y, true);

                        MainLayer.DrawString((Assets.Fonts.BabyDoll._90), chosenNumber.Number.ToString(), Positions.FlagLongPositionNumbers[index].X, Positions.FlagLongPositionNumbers[index].Y);
                    }
                    else
                    {
                        IImage flagImage = Assets.Images.Character.Banners.Small.ShortBanner[chosenNumber.Character.CharacterNumber + 1];
                        MainLayer.DrawImage(flagImage, Positions.FlagPositions[index].X, Positions.FlagPositions[index].Y, true);

                        MainLayer.DrawString((Assets.Fonts.BabyDoll._90), chosenNumber.Number.ToString(), Positions.FlagPositionNumbers[index].X, Positions.FlagPositionNumbers[index].Y);
                    }
                    if (GameService.ClassicGameState.GameState != GameSelectionState.ChooseNumber)
                    {
                        long timeLeft = Math.Max(GetTimeLeft(), 0);

                        MainLayer.DrawString((Assets.Fonts.BabyDoll._240), timeLeft.ToString(), Positions.TimePosition.X, Positions.TimePosition.Y, Positions.BlackColor);
                        MainLayer.DrawString((Assets.Fonts.BabyDoll._48), "time left", Positions.TimePosition.X, Positions.TimePosition.Y + 100, Positions.BlackColor);
                    }
                    index++;
                }

                MainLayer.DrawImage(baseImage, Positions.SmallCardCharacterPosition.X, Positions.SmallCardCharacterPosition.Y, 150, 150, true);
                MainLayer.DrawImage(smallGoalArrow, Positions.SmallCardArrowPosition.X, Positions.SmallCardArrowPosition.Y, 50, 30, true);
                MainLayer.DrawImage(goalImage, Positions.SmallCardGoalPosition.X, Positions.SmallCardGoalPosition.Y, 150, 150, true);
            }
        }

        private long GetTimeLeft()
        {
            long ticks = (DateTime.Now.Ticks - GameService.ClassicGameState.TimeStarted) - State.TimePaused;
            long seconds = ticks / 10000000;
            long timeLeft = State.TotalNumberOfSeconds - seconds;
            return timeLeft;
        }

        public void Tick(TimeSpan elapsedGameTime)
        {
            switch (GameService.ClassicGameState.GameState)
            {
                case GameSelectionState.TimerStarted:
                    if (GetTimeLeft() == 0)
                    {
                        if (State.CurrentTick != null) State.CurrentTick.Stop();
                        ISoundEffect sfx = Game.Client.PlaySoundEffect(Assets.Sounds.TimeTick);
                        Game.Client.Timeout(() =>
                        {
                            sfx.Stop();
                            ScreenTransitioner.ChangeToBoardScreen();
                        }, 1500);
                    }
                    break;
                case GameSelectionState.ChooseNumber:
                    if (State.SwipeDistance != 0)
                    {
                        if (State.SwipeDistance > 0)
                        {
                            State.SwipeDistance += State.SwipeAcceleration;
                            if (State.SwipeDistance <= 0)
                            {
                                State.SwipeDistance = 0;
                            }
                        }
                        else
                        {
                            State.SwipeDistance += State.SwipeAcceleration;
                            if (State.SwipeDistance >= 0)
                            {
                                State.SwipeDistance = 0;
                            }
                        }

                        if (State.Ticks++ % 5 == 0)
                        {
                            State.CurrentNumber = Math.Min(Math.Max(State.CurrentNumber - Math.Sign(State.SwipeDistance), 1), 60);
                        }
                    }


                    break;
            }
            GameService.ClassicGameState.Board.Tick(elapsedGameTime);

            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.TickLayoutView(elapsedGameTime);
            }
        }

        private void drawBottomChoosePlayers()
        {
            MainLayer.Save();
            foreach (Character character in GameService.ClassicGameState.Characters)
            {
                var realCharacterNumber = character.CharacterNumber + 1;

                if (!character.Playing)
                {
                    MainLayer.DrawImage(Assets.Images.Character.Placement.CharacterPlacement[realCharacterNumber], Positions.CharacterShadowPositions[character.CharacterNumber], true);
                }

                else
                {
                    MainLayer.DrawImage(Assets.Images.Character.CharacterShadow, Positions.CharacterShadowPositions[character.CharacterNumber], true);
                    MainLayer.Save();

                    var animatedCharacterSubLayout = CharacterAnimations[realCharacterNumber];
                    animatedCharacterSubLayout.Selected = character.Selected;
                    if (character.Selected)
                    {
                        MainLayer.Translate(Positions.CharacterPositions[character.CharacterNumber].X, Positions.CharacterPositions[character.CharacterNumber].Y - 20);
                    }
                    else
                    {
                        MainLayer.Translate(Positions.CharacterPositions[character.CharacterNumber].X, Positions.CharacterPositions[character.CharacterNumber].Y);
                    }


                    animatedCharacterSubLayout.Render(MainLayer);

                    MainLayer.Restore();


                    var characterScorePosition = Positions.CharacterScorePositions[character.CharacterNumber];
                    MainLayer.DrawString((Assets.Fonts.BabyDoll._36), "x", characterScorePosition.X, characterScorePosition.Y + 13, Positions.DarkColor, false);
                    MainLayer.DrawString((Assets.Fonts.BabyDoll._60), character.Score.ToString(), characterScorePosition.X + 25, characterScorePosition.Y, Positions.DarkColor, false);
                }
            }
            MainLayer.Restore();
        }
    }


    public class BoardViewingAreaLayoutState
    {
        public int CurrentNumber = 15;
        public int TotalNumberOfSeconds = 20;

        public BoardViewingAreaLayoutState(ILayout layout)
        {
            Layout = layout;
        }

        public ILayout Layout { get; set; }

        public Point NSDragMouseDownPosition { get; set; }
        public int NSDragMouseDownStartingNumber { get; set; }
        public double SwipeDistance { get; set; }
        public long Ticks { get; set; }
        public long TimePaused { get; set; }
        public long TimePausedStart { get; set; }
        public Character CurrentCharacter { get; set; }
        public double SwipeAcceleration { get; set; }
        public int TotalChosens { get; set; }
        public bool BackDialogVisible { get; set; }
        public ISoundEffect CurrentTick { get; set; }
    }

    public class BoardViewingAreaLayoutStatePositions
    {
        public Color BlackColor = new Color(41, 40, 42);
        public Color DarkColor = new Color(35, 31, 32);
        public Color DarkColorLight = new Color(135, 131, 132);
        public Color DarkColorLighter = new Color(235, 231, 232);


        public BoardViewingAreaLayoutStatePositions(ILayout layout)
        {
            Layout = layout;


            CardCharacterPosition = new Point(605, 135);
            CardArrowPosition = new Point(765, 135);
            CardGoalPosition = new Point(931, 135);

            CharacterShadowPositions = new Point[6];
            CharacterPositions = new Point[6];
            CharacterScorePositions = new Point[6];
            FlagPositions = new Point[6];
            FlagPositionNumbers = new Point[6];
            FlagLongPositions = new Point[6];
            FlagLongPositionNumbers = new Point[6];
            int flagHeight = 200;
            int longFlagHeight = 260;
            int flagOffset = -25;
            int longOffset = 13;


            for (int i = 0; i < 6; i++)
            {
                CharacterPositions[i] = new Point(215 + 223 * i, 332);
                CharacterShadowPositions[i] = new Point(215 + 223 * i, 424);
                CharacterScorePositions[i] = new Point(195 + 223 * i, 445);
                FlagPositions[i] = new Point(104 + 130 * i, flagHeight / 2 + flagOffset);
                FlagPositionNumbers[i] = new Point(104 + 130 * i, 76);
                FlagLongPositions[i] = new Point(104 + 130 * i, longFlagHeight / 2 + flagOffset - longOffset);
                FlagLongPositionNumbers[i] = new Point(104 + 130 * i, 110);
            }
            TimePosition = new Point(Layout.Width / 2, 97);


            SmallCardCharacterPosition = new Point(1152, 131);
            SmallCardArrowPosition = new Point(1279, 135);
            SmallCardGoalPosition = new Point(1411, 135);

            Help1Position = new Point(Layout.Width / 4 - 100, 135);
            Help2Position = new Point(Layout.Width - (Layout.Width / 4) + 100, 135);

            initNumberSelection();
            BackPosition = new Point(0, layout.Height - 150);
            BackLayout = new BoardAreaLayoutStateBackPositions(layout);
        }

        public Point CardCharacterPosition { get; set; }
        public Point CardArrowPosition { get; set; }
        public Point CardGoalPosition { get; set; }

        public Point SmallCardCharacterPosition { get; set; }
        public Point SmallCardArrowPosition { get; set; }
        public Point SmallCardGoalPosition { get; set; }


        public Point TopBarPosition { get; set; }
        public Point BottomBarPosition { get; set; }

        public Point NumberSelectionSelectButtonPosition { get; set; }
        public Point NumberSelectionBoxPosition { get; set; }
        public Point NumberSelectionSelectBoxSize { get; set; }
        public Point NumberSelectionBoxSize { get; set; }
        public Point[] CharacterPositions { get; set; }

        public Point[] CharacterShadowPositions { get; set; }

        public Point TimePosition { get; set; }

        public Point[] CharacterScorePositions { get; set; }

        public Point[] FlagPositions { get; set; }
        public Point[] FlagPositionNumbers { get; set; }
        public Point[] FlagLongPositions { get; set; }
        public Point[] FlagLongPositionNumbers { get; set; }

        public Point NSMiddleNumberPosition { get; set; }

        public Rectangle NSFirstShadowPosition { get; set; }
        public Rectangle NSSecondShadowPosition { get; set; }
        public Rectangle NSThirdShadowPosition { get; set; }
        public Rectangle NSForthShadowPosition { get; set; }
        public Point BackPosition { get; set; }

        public ILayout Layout { get; set; }
        public BoardAreaLayoutStateBackPositions BackLayout { get; set; }
        public PointF Help1Position { get; set; }
        public PointF Help2Position { get; set; }


        private void initNumberSelection()
        {
            NumberSelectionBoxPosition = new Point(BoardConstants.TotalWidth / 2, BoardConstants.TopAreaHeight / 2);

            NumberSelectionBoxSize = new Point(1024, 1024);
            NumberSelectionSelectBoxSize = new Point(1024, 340);


            NumberSelectionSelectButtonPosition = new Point(0, NumberSelectionBoxSize.Y - NumberSelectionSelectBoxSize.Y);

            TopBarPosition = new Point(NumberSelectionSelectBoxSize.X / 2, 263);
            BottomBarPosition = new Point(NumberSelectionSelectBoxSize.X / 2, 420);

            NSFirstShadowPosition = new Rectangle(0, 0, 1024, 142);
            NSSecondShadowPosition = new Rectangle(0, 142, 1024, 114);
            NSThirdShadowPosition = new Rectangle(0, 435, 1024, 100);
            NSForthShadowPosition = new Rectangle(0, 548, 1024, 106);
            NSMiddleNumberPosition = new Point(NumberSelectionSelectBoxSize.X / 2, 354);
        }
    }
}