using System;
using System.Collections.Generic;
using Engine;
using Engine.Animation;
using Engine.Interfaces;
using PenguinShuffle.SubLayoutViews;

namespace PenguinShuffle.BoardArea.Layouts
{
    public class BoardSlidingAreaLayout : ILayoutView
    {
        private static readonly int[] swipeOffsetPositions = { 0, 1, -1, 2, -2 };

        public BoardSlidingAreaLayout(Game game, GameService gameService, IRenderer renderer, ILayout layout, ScreenTransitioner screenTransitioner)
        {
            Game = game;
            Renderer = renderer;
            GameService = gameService;
            Layout = layout;
            ScreenTransitioner = screenTransitioner;
            State = new BoardAreaLayoutState(layout);
            Positions = new BoardAreaLayoutStatePositions(layout);

            MainLayer = Renderer.CreateLayer(Layout);
            Renderer.AddLayer(MainLayer);
            TouchManager = new TouchManager(Game.Client);

            CharacterAnimations = new Dictionary<int, AnimatedCharacterSubLayout>();
            for (int i = 1; i <= 6; i++)
            {
                CharacterAnimations.Add(i, new AnimatedCharacterSubLayout( Game, i));
            }

        }

        public Dictionary<int, AnimatedCharacterSubLayout> CharacterAnimations;

        public Game Game { get; set; }
        public IRenderer Renderer { get; set; }
        public ILayer MainLayer { get; set; }
        public BoardAreaLayoutState State { get; set; }
        
        public GameService GameService { get; set; }
        public ScreenTransitioner ScreenTransitioner { get; set; }
        public BoardAreaLayoutStatePositions Positions { get; set; }
        public ILayout Layout { get; set; }
        public ITouchManager TouchManager { get; private set; }

        public void Destroy()
        {
        }

        public void InitLayoutView()
        {
            Init();
        }

        public void Init()
        {
            TouchManager.ClearClickRect();
            TouchManager.ClearSwipeRect();

            for (int x = 0; x < BoardConstants.SquareWidth; x++)
            {
                for (int y = 0; y < BoardConstants.SquareHeight; y++)
                {
                    int x1 = x;
                    int y1 = y;
                    var touchRect = new TouchRect(x * BoardConstants.SquareSize + BoardConstants.SideOffset, y * BoardConstants.SquareSize + BoardConstants.TopOffset, BoardConstants.SquareSize, BoardConstants.SquareSize,
                        (type, box, touchX, touchY, collide) => squareTouch(x1, y1, type, box, touchX, touchY, collide));

                    Layout.LayoutView.TouchManager.PushClickRect(touchRect);
                }
            }
            Layout.LayoutView.TouchManager.PushSwipeRect(new TouchRect(0, 0, BoardConstants.TotalWidth, BoardConstants.TopAreaHeight, swipeMotion));


            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(Positions.UndoButtonRect, undoPress));
            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(Positions.PassButtonRect, passPress));

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(0, 0, Layout.Width, Layout.Height, wholeBoardClick));

            startTurn(GameService.ClassicGameState.ChosenNumbers[GameService.ClassicGameState.ChosenNumbers.Count - 1]);
            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.InitLayoutView(TouchManager);
            }
        }
        public void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Tick(elapsedGameTime);
        }

        public void Render(TimeSpan elapsedGameTime)
        {
            MainLayer.Begin();
            MainLayer.Save();

            MainLayer.DrawRectangle(new Color(252, 252, 252), 0, 0, MainLayer.Layout.Width, MainLayer.Layout.Height);

            GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer);


            MainLayer.Save();
            MainLayer.DrawImage(Assets.Images.Layouts.StagingArea, 0, BoardConstants.TopAreaHeight);
            if (State.Congrats && !GameService.ClassicGameState.Board.IsMoving)
            {
                drawBottomChoosePlayers();
            }
            else
            {
                drawBottomInfo();
            }
            MainLayer.Restore();

            if (State.Congrats && !GameService.ClassicGameState.Board.IsMoving)
            {
                MainLayer.DrawImage(Assets.Images.Character.LabelBox.CharacterBox[State.CurrentChosenNumber.Character.CharacterNumber + 1], Positions.CongratsPosition, true);
                MainLayer.DrawString((Assets.Fonts.BabyDoll._130), string.Format("Congrats\nPlayer {0}!", (State.CurrentChosenNumber.Character.CharacterNumber + 1)), Positions.CongratsPosition);
            }
            MainLayer.Restore();

            TouchManager.Render(MainLayer);
            MainLayer.End();
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
            GameService.ClassicGameState.Board.Tick(elapsedGameTime);
            if (State.Congrats && !GameService.ClassicGameState.Board.IsMoving && State.CardAnimationMotion != null) State.CardAnimationMotion.Tick(elapsedGameTime);
            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.TickLayoutView(elapsedGameTime);
            }
        }

        private bool wholeBoardClick(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (State.Congrats && State.CardAnimationMotion.Completed)
                {
                    Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                    GameService.ClassicGameState.CharacterWon(State.CurrentChosenNumber.Character);
                    ScreenTransitioner.ChangeToBoardCardSelectionScreen();
                    return false;
                }
            }
            return true;
        }


        private bool squareTouch(int boxX, int boxY, TouchType eventtype, TouchRect touchbox, int touchX, int touchY, bool collide)
        {
            switch (eventtype)
            {
                case TouchType.TouchDown:


                    break;
            }
            return true;
        }

        private void startTurn(ChosenNumber chosenNumber)
        {
            State.Congrats = false;
            State.Moves = new List<PlayerPosition>();
            State.CurrentChosenNumber = chosenNumber;
        }

        private bool swipeMotion(TouchRect touchbox, int startx, int starty, Point direction, double distance)
        {
            if (GameService.ClassicGameState.Board.IsMoving || State.Congrats) return false;

            if (getMovesLeft() <= 0)
            {
                //Todo: playSound();
                return false;
            }
            Player selectedPlayer = null;

            startx -= BoardConstants.SideOffset;
            starty -= BoardConstants.TopOffset;

            int sX = startx / BoardConstants.SquareSize;
            int sY = starty / BoardConstants.SquareSize;
            Direction moveDirection;

            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                if (direction.X > 0)
                {
                    moveDirection = Direction.Right;
                }
                else
                {
                    moveDirection = Direction.Left;
                }
            }
            else
            {
                if (direction.Y > 0)
                {
                    moveDirection = Direction.Bottom;
                }
                else
                {
                    moveDirection = Direction.Top;
                }
            }

            int swipeOffsetTries = 0;

            while (selectedPlayer == null && swipeOffsetTries < swipeOffsetPositions.Length)
            {
                int pX = 0;
                int pY = 0;

                switch (moveDirection)
                {
                    case Direction.Left:
                    case Direction.Right:
                        pY = swipeOffsetPositions[swipeOffsetTries];
                        break;
                    case Direction.Top:
                    case Direction.Bottom:
                        pX = swipeOffsetPositions[swipeOffsetTries];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                int oX = 0;
                int oY = 0;
                while (selectedPlayer == null && Math.Abs(oX) <= 2 && Math.Abs(oY) <= 2)
                {
                    selectedPlayer = GameService.ClassicGameState.Board.GetPlayerAtPoint(sX + oX + pX, sY + oY + pY);

                    switch (moveDirection)
                    {
                        case Direction.Left:
                            oX -= 1;
                            break;
                        case Direction.Right:
                            oX += 1;
                            break;
                        case Direction.Top:
                            oY -= 1;
                            break;
                        case Direction.Bottom:
                            oY += 1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                swipeOffsetTries++;
            }


            if (selectedPlayer == null) return false;

            var playerPosition = new PlayerPosition(selectedPlayer, selectedPlayer.Position.X, selectedPlayer.Position.Y, selectedPlayer.Direction);
            GameService.ClassicGameState.Board.ProcessMovement(new Movement(selectedPlayer, moveDirection));
            if (GameService.ClassicGameState.Board.MoveToPosition != null)
            {
                Game.Client.PlaySoundEffect(Assets.Sounds.Slide);
                Goal currentGoal = GameService.ClassicGameState.CurrentGoal;
                var goal = (GoalPiece)GameService.ClassicGameState.Board.getItemsOnBoard(null, GameService.ClassicGameState.Board.MoveToPosition.X, GameService.ClassicGameState.Board.MoveToPosition.Y).First(a => a.Type == SquareTypes.Goal);

                if (goal != null && (selectedPlayer.PlayerNumber - 1 == currentGoal.PlayerNumber || currentGoal.PlayerNumber == 0) && (goal.Goal == currentGoal))
                {
                    PlayerWon(goal);
                }
                State.Moves.Add(playerPosition);
                selectedPlayer.SetAnimating(GameService.ClassicGameState.Board.MoveToPosition);
            }


            return false;
        }

        private void PlayerWon(GoalPiece goal)
        {
            Point pos = Positions.CharacterPositions[State.CurrentChosenNumber.Character.CharacterNumber];
            State.Congrats = true;
            IImage goalImage = GoalPiece.GetGoalImage(GameService.ClassicGameState.CurrentGoal);
            State.CardAnimationMotion = MotionManager.StartMotion(Positions.CongratsPosition.X, Positions.CongratsPosition.Y, new WaitMotion(200))
                .Motion(new AnimationMotion(pos.X + 0, pos.Y + BoardConstants.TopAreaHeight, 2000, AnimationEasing.BounceEaseOut))
                .OnRender((layer, posX, posY, animationIndex, percentDone) =>
                {
                    MainLayer.Save();
                    MainLayer.SetDrawingTransparency(1 - percentDone);
                    MainLayer.DrawImage(goalImage, posX, posY, 220, 220, true);
                    MainLayer.Restore();
                }).OnComplete(() =>
                {
                    GameService.ClassicGameState.Board.SquarePieces.Remove(goal);

                    GameService.ClassicGameState.CharacterWon(State.CurrentChosenNumber.Character);
                    ScreenTransitioner.ChangeToBoardCardSelectionScreen();
                });
        }

        private bool passPress(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.Congrats) return false;
            if (GameService.ClassicGameState.Board.MoveToPosition != null) return false;
            if (eventtype == TouchType.TouchDown)
            {
                Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                for (int i = State.Moves.Count - 1; i >= 0; i--)
                {
                    PlayerPosition undo = State.Moves[i];
                    undo.Character.Position.X = undo.X;
                    undo.Character.Position.Y = undo.Y;
                    undo.Character.Direction = undo.Direction;
                }
                State.Moves.Clear();


                GameService.ClassicGameState.ChosenNumbers.Remove(State.CurrentChosenNumber);
                if (GameService.ClassicGameState.ChosenNumbers.Count == 0)
                {
                    ISquarePiece goal = GameService.ClassicGameState.Board.SquarePieces.First(a => a is GoalPiece && ((GoalPiece)a).Goal == GameService.ClassicGameState.CurrentGoal);

                    GameService.ClassicGameState.Board.SquarePieces.Remove(goal);

                    GameService.ClassicGameState.CharacterLost();
                    ScreenTransitioner.ChangeToBoardCardSelectionScreen();
                    return false;
                }
                startTurn(GameService.ClassicGameState.ChosenNumbers[GameService.ClassicGameState.ChosenNumbers.Count - 1]);
            }
            return false;
        }

        private bool undoPress(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.Congrats) return false;
            if (GameService.ClassicGameState.Board.MoveToPosition != null) return false;
            if (eventtype == TouchType.TouchDown)
            {
                Game.Client.PlaySoundEffect(Assets.Sounds.Click);
                if (State.Moves.Count > 0)
                {
                    PlayerPosition undo = State.Moves[State.Moves.Count - 1];
                    State.Moves.Remove(undo);
                    undo.Character.Position.X = undo.X;
                    undo.Character.Position.Y = undo.Y;
                    undo.Character.Direction = undo.Direction;
                }
            }
            return false;
        }

        private void drawBottomInfo()
        {
            MainLayer.Save();
            MainLayer.Translate(0, BoardConstants.TopAreaHeight);
            IImage smallGoalArrow = Assets.Images.Layouts.Arrow;
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

                MainLayer.DrawString((Assets.Fonts.BabyDoll._240), getMovesLeft().ToString(), Positions.NumberPosition, Positions.BlackColor);
                MainLayer.DrawString((Assets.Fonts.BabyDoll._48), "total moves", Positions.NumberPosition.X, Positions.NumberPosition.Y + 100, Positions.BlackColor);
                index++;
            }


            MainLayer.DrawImage(smallGoalArrow, Positions.SmallCardArrowPosition.X, Positions.SmallCardArrowPosition.Y, true);

            IImage baseImage = GoalPiece.GetBaseImage(GameService.ClassicGameState.CurrentGoal);

            IImage goalImage = GoalPiece.GetGoalImage(GameService.ClassicGameState.CurrentGoal);

            MainLayer.DrawImage(baseImage, Positions.SmallCardCharacterPosition.X, Positions.SmallCardCharacterPosition.Y, 150, 150, true);
            MainLayer.DrawImage(smallGoalArrow, Positions.SmallCardArrowPosition.X, Positions.SmallCardArrowPosition.Y, 50, 30, true);
            MainLayer.DrawImage(goalImage, Positions.SmallCardGoalPosition.X, Positions.SmallCardGoalPosition.Y, 150, 150, true);
            MainLayer.Restore();

            MainLayer.DrawRectangle(new Color(252, 252, 252), 0, Positions.UndoButtonRect.Y, Layout.Width, Positions.UndoButtonRect.Height);

            MainLayer.DrawRectangle(new Color(37, 170, 255), Positions.UndoButtonRect);
            MainLayer.DrawString((Assets.Fonts.BabyDoll._120), "Undo", Positions.UndoButtonRect.Center);

            MainLayer.DrawRectangle(new Color(37, 170, 255), Positions.PassButtonRect);
            MainLayer.DrawString((Assets.Fonts.BabyDoll._120), "Pass", Positions.PassButtonRect.Center);
        }

        private void drawBottomChoosePlayers()
        {
            if (State.CardAnimationMotion != null)
            {
                State.CardAnimationMotion.Render(MainLayer);
            }


            MainLayer.Save();
            MainLayer.Translate(0, BoardConstants.TopAreaHeight);
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

        private int getMovesLeft()
        {
            return State.CurrentChosenNumber.Number - State.Moves.Count;
        }
    }

    public class BoardAreaLayoutState
    {
        public BoardAreaLayoutState(ILayout layout)
        {
            Layout = layout;
        }

        public ILayout Layout { get; set; }

        public bool Congrats { get; set; }
        public ChosenNumber CurrentChosenNumber { get; set; }
        public List<PlayerPosition> Moves { get; set; }
        public MotionManager CardAnimationMotion { get; set; }
    }

    public class BoardAreaLayoutStatePositions
    {
        public Color BlackColor = new Color(41, 40, 42);

        public Color DarkColor = new Color(35, 31, 32);

        public BoardAreaLayoutStatePositions(ILayout layout)
        {
            Layout = layout;
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
            NumberPosition = new Point(Layout.Width / 2, 97);

            UndoButtonPosition = new Point(0, Layout.Height - 270);
            PassButtonPosition = new Point(770, Layout.Height - 270);

            SmallCardCharacterPosition = new Point(1152, 131);
            SmallCardArrowPosition = new Point(1279, 135);
            SmallCardGoalPosition = new Point(1411, 135);

            CongratsPosition = new Point(Layout.Width / 2, 767);


            PassButtonRect = new Rectangle(PassButtonPosition.X, PassButtonPosition.Y, Layout.Width / 2 - 1, 270);
            UndoButtonRect = new Rectangle(UndoButtonPosition.X, UndoButtonPosition.Y, Layout.Width / 2 + 1, 270);
        }

        public ILayout Layout { get; set; }


        public Point SmallCardCharacterPosition { get; set; }
        public Point SmallCardArrowPosition { get; set; }
        public Point SmallCardGoalPosition { get; set; }
        public Point[] CharacterPositions { get; set; }
        public Point[] CharacterShadowPositions { get; set; }
        public Point NumberPosition { get; set; }
        public Point[] CharacterScorePositions { get; set; }
        public Point[] FlagPositions { get; set; }
        public Point[] FlagPositionNumbers { get; set; }
        public Point[] FlagLongPositions { get; set; }
        public Point[] FlagLongPositionNumbers { get; set; }
        public Point CongratsPosition { get; set; }
        public Point UndoButtonPosition { get; set; }

        public Point PassButtonPosition { get; set; }
        public Rectangle UndoButtonRect { get; set; }
        public Rectangle PassButtonRect { get; set; }
    }
}