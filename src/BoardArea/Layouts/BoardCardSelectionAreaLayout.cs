using System;
using System.Collections.Generic;
using Engine;
using Engine.Animation;
using Engine.Interfaces;

namespace PenguinShuffle.BoardArea.Layouts
{
    public class BoardCardSelectionAreaLayout : BaseLayoutView
    {
        public BoardCardSelectionAreaLayout(GameService gameService,  ScreenTransitioner screenTransitioner)
        {
            GameService = gameService;
            ScreenTransitioner = screenTransitioner;
        

        }

        public ILayer MainLayer { get; set; }
        public BoardCardSelectionAreaLayoutState State { get; set; }

        public BoardCardSelectionAreaLayoutStatePositions Positions { get; set; }
        public GameService GameService { get; set; }
        public ScreenTransitioner ScreenTransitioner { get; set; }

         

        public override void Destroy()
        {
        }

        public override void InitLayoutView()
        {

            State = new BoardCardSelectionAreaLayoutState(Layout);
            Positions = new BoardCardSelectionAreaLayoutStatePositions(Layout);
            MainLayer = Renderer.CreateLayer(Layout);
            Renderer.AddLayer(MainLayer);
            Init();
        }

        public override void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Tick(elapsedGameTime);
        }

        public override void Render(TimeSpan elapsedGameTime)
        {
            MainLayer.Begin();

            MainLayer.Save();
            MainLayer.DrawRectangle(new Color(252, 252, 252), 0, 0, MainLayer.Layout.Width, MainLayer.Layout.Height);


            GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer);

            if (GameService.ClassicGameState.GameState == GameSelectionState.PickCard)
            {
                if (GameService.ClassicGameState.UnusedGoals.Length == 0)
                {
                    var winningChars = new List<Character>();
                    int winningScore = -1;
                    foreach (Character character in GameService.ClassicGameState.Characters)
                    {
                        if (!character.Playing) continue;
                        if (character.Score > winningScore)
                        {
                            winningChars.Clear();
                            winningChars.Add(character);
                        }
                        else
                        {
                            if (winningChars.Count > 0)
                            {
                                if (winningChars[0].Score == character.Score)
                                {
                                    winningChars.Add(character);
                                }
                            }
                        }
                    }
                    Character ch = winningChars.First();
                    
                    MainLayer.DrawImage(Assets.Images.Character.LabelBox.CharacterBox[ch.CharacterNumber + 1], Positions.CongratsPosition, true);

                    string charWinningString;
                    if (winningChars.Count == 1)
                    {
                        charWinningString = string.Format("Player {0}\nHas Won!", (ch.CharacterNumber + 1));
                    }
                    else
                    {
                        string plc = (winningChars[0].CharacterNumber + 1).ToString();

                        for (int index = 1; index < winningChars.Count - 1; index++)
                        {
                            Character winningChar = winningChars[index];
                            plc += ", " + (winningChar.CharacterNumber + 1);
                        }
                        plc = "And " + (winningChars[0].CharacterNumber + 1);

                        charWinningString = string.Format("Players {0}\nHave Won!", plc);
                    }
                    MainLayer.DrawString((Assets.Fonts.BabyDoll._130), charWinningString, Positions.CongratsPosition);
                }
                else
                {

                    if (State.CardAnimationMotion == null)
                    {
                        MainLayer.DrawImage(Assets.Images.Layouts.TextBoard, Positions.SelectACardPosition, true);
                        MainLayer.DrawString((Assets.Fonts.BabyDoll._72), "Please Select A Card", Positions.SelectACardPosition);
                    }

                    MainLayer.Translate(0, BoardConstants.TopAreaHeight);
                    drawBottomCards();
                }
            }


            MainLayer.Restore();

            TouchManager.Render(MainLayer);
            MainLayer.End();
        }

        public void Init()
        {
            TouchManager.ClearClickRect();
            TouchManager.PushClickRect(new TouchRect(0, 0, Layout.Width, Layout.Height, wholeBoardTouch));
            InitCardTouches();
        }

        private bool wholeBoardTouch(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (GameService.ClassicGameState.UnusedGoals.Length == 0)
                {
                    var gamesCompleted = Client.UserPreferences.GetValueOrDefault("GamesCompleted", 0) + 1;
                    if (gamesCompleted == 3)
                    {
                        Client.UserPreferences.AddOrUpdateValue("GamesCompleted", gamesCompleted);
                        Client.ClientSettings.RateApp();
                        Client.PlaySoundEffect(Assets.Sounds.Click);
                        ScreenTransitioner.ChangeToLanding();
                    }
                    return false;
                }
            }
            return true;
        }

        public void InitCardTouches()
        {
            Goal[] goals = GameService.ClassicGameState.UnusedGoals;
            for (int index = goals.Length - 1; index >= 0; index--)
            {
                Goal goal = goals[index];

                int topRow = goals.Length / 2;
                int bottomRow = goals.Length / 2 + goals.Length % 2;
                int posx, posy;


                if (index < topRow)
                {
                    int centerOffset = (Positions.BottomCardAreaWidth / (topRow)) / 2;
                    posx = Positions.BottomCardAreaWidth / topRow * (index) + centerOffset;
                    posy = Positions.BottomCardSize / 2 - Positions.CardShadowHeight;
                }
                else
                {
                    int centerOffset = (Positions.BottomCardAreaWidth / (bottomRow)) / 2;
                    posx = Positions.BottomCardAreaWidth / bottomRow * (index - topRow) + centerOffset;
                    posy = Positions.BottomCardSize - 40 + Positions.BottomCardSize / 2 - Positions.CardShadowHeight;
                }


                posy += BoardConstants.TopAreaHeight;

                TouchManager.PushClickRect(new TouchRect(posx, posy, Positions.BottomCardSize, Positions.BottomCardSize, tapCard, state: goal, pointIsCenter: true));
            }
        }

        private bool tapCard(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                if (State.CardAnimationMotion != null) return false;

                Client.PlaySoundEffect(Assets.Sounds.Click);
                var gc = (Goal)touchbox.State;
                IImage goalImage = GoalPiece.GetGoalImage(gc);


                Goal[] goals = GameService.ClassicGameState.UnusedGoals;
                int topRow = goals.Length / 2;
                int bottomRow = goals.Length / 2 + goals.Length % 2;
                int posx, posy;
                int index = Array.IndexOf(goals, gc);

                int yAnimateOffset = 0;
                if (index < topRow)
                {
                    int centerOffset = (Positions.BottomCardAreaWidth / (topRow)) / 2;

                    posx = ((Positions.BottomCardAreaWidth / topRow * (index))) + centerOffset;
                    posy = (Positions.BottomCardSize / 2) - Positions.CardShadowHeight;
                    yAnimateOffset = 0;
                }
                else
                {
                    int centerOffset = (Positions.BottomCardAreaWidth / (bottomRow)) / 2;

                    posx = ((Positions.BottomCardAreaWidth / bottomRow * (index - topRow))) + centerOffset;
                    posy = Positions.BottomCardSize - 40 + Positions.BottomCardSize / 2 - Positions.CardShadowHeight;
                    yAnimateOffset = 300;
                }

                State.CurrentlySelectedGoal = gc;

                State.CardAnimationMotion = MotionManager.StartMotion(posx, posy, new AnimationMotion(Positions.Layout.Width / 2, posy - 850 - yAnimateOffset, 1500, AnimationEasing.BounceEaseIn))
                    .Motion(new WaitMotion(1000))
                    .Motion(new AnimationMotion(Positions.Layout.Width / 2, -3000, 1000, AnimationEasing.CubicEaseOut))
                    .OnRender((layer, posX, posY, animationIndex, percentDone) => { MainLayer.DrawImage(goalImage, posX, posY, true); })
                    .OnComplete(() =>
                    {
                        gc.Used = true;

                        GameService.ClassicGameState.CurrentGoal = gc;
                        State.CardAnimationMotion = null;

                        GameService.ClassicGameState.GameState = GameSelectionState.SearchingBoard;
                        ScreenTransitioner.ChangeToBoardViewingScreen();
                    });

                return false;
            }
            return true;
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
            GameService.ClassicGameState.Board.Tick(elapsedGameTime);
            if (State.CardAnimationMotion != null) State.CardAnimationMotion.Tick(elapsedGameTime);
        }


        private void drawBottomCards()
        {
            MainLayer.Save();

            IImage image = Assets.Images.Cards.CardsBack;


            Goal[] goalCards = GameService.ClassicGameState.UnusedGoals;


            int topRow = goalCards.Length / 2;
            int bottomRow = goalCards.Length / 2 + goalCards.Length % 2;

            if (topRow != 0)
            {
                int topCenterOffset = (Positions.BottomCardAreaWidth / (topRow)) / 2;
                for (int index = 0; index < topRow; index++)
                {
                    if (goalCards[index] == State.CurrentlySelectedGoal) continue;
                    MainLayer.DrawImage(image, (Positions.BottomCardAreaWidth / topRow * index) + topCenterOffset, Positions.BottomCardSize / 2 - Positions.CardShadowHeight, Positions.BottomCardSize, Positions.BottomCardSize, true);
                }
            }

            int bottomCenterOffset = (Positions.BottomCardAreaWidth / (bottomRow)) / 2;
            for (int index = topRow; index < topRow + bottomRow; index++)
            {
                if (goalCards[index] == State.CurrentlySelectedGoal) continue;
                MainLayer.DrawImage(image, (Positions.BottomCardAreaWidth / bottomRow * (index - topRow)) + bottomCenterOffset, Positions.BottomCardSize - 40 + Positions.BottomCardSize / 2 - Positions.CardShadowHeight, Positions.BottomCardSize, Positions.BottomCardSize, true);
            }

            if (State.CardAnimationMotion != null)
            {
                State.CardAnimationMotion.Render(MainLayer);
            }

            MainLayer.Restore();
        }
    }


    public class BoardCardSelectionAreaLayoutState
    {
        public BoardCardSelectionAreaLayoutState(BaseLayout layout)
        {
            Layout = layout;
        }

        public BaseLayout Layout { get; set; }
        public int CurrentPlayerChoosing { get; set; }
        public MotionManager CardAnimationMotion { get; set; }
        public Goal CurrentlySelectedGoal { get; set; }
    }

    public class BoardCardSelectionAreaLayoutStatePositions
    {
        public BoardCardSelectionAreaLayoutStatePositions(BaseLayout layout)
        {
            Layout = layout;
            BottomCardSize = 304;
            BottomCardAreaWidth = layout.Width;
            CardShadowHeight = 12;
            CongratsPosition = new Point(Layout.Width / 2, 767);
            SelectACardPosition = new Point(Layout.Width / 2, 767);
        }

        public BaseLayout Layout { get; set; }
        public Point CongratsPosition { get; set; }
        public int BottomCardSize { get; set; }
        public int BottomCardAreaWidth { get; set; }
        public int CardShadowHeight { get; set; }
        public Point SelectACardPosition { get; set; }
    }
}