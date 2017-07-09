using System;
using System.Collections.Generic;
using Engine;
using Engine.Interfaces;
using PenguinShuffle;
using PenguinShuffle.SubLayoutViews;
using PenguinShuffle.Utils;

namespace PenguinShuffle.BoardArea.Layouts
{
    public class BoardSelectionAreaLayout : BaseLayoutView
    {
        public BoardSelectionAreaLayout(GameService gameService,  ScreenTransitioner screenTransitioner)
        {
            GameService = gameService;
            ScreenTransitioner = screenTransitioner; 
        }

        public Dictionary<int, AnimatedCharacterSubLayout> CharacterAnimations;


        
        public ILayer MainLayer { get; set; }
        public GameService GameService { get; set; }
        public ScreenTransitioner ScreenTransitioner { get; set; }
        
        public BoardSelectionAreaLayoutState State { get; set; }

        public BoardSelectionAreaLayoutStatePositions Positions { get; set; }


        public override void Destroy()
        {
        }

        public override void InitLayoutView()
        {
            CharacterAnimations = new Dictionary<int, AnimatedCharacterSubLayout>();
            for (int i = 1; i <= 6; i++)
            {
                CharacterAnimations.Add(i, new AnimatedCharacterSubLayout(Client, i));
            }

            MainLayer = Renderer.CreateLayer(Layout);
            Renderer.AddLayer(MainLayer);

            Init();
        }
        public void Init()
        {
            State = new BoardSelectionAreaLayoutState(Layout);
            Positions = new BoardSelectionAreaLayoutStatePositions(Layout);

            TouchManager.ClearClickRect();
            TouchManager.ClearSwipeRect();

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(0, 0, Layout.Width, Layout.Height, wholeBoardClick));

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

            Layout.LayoutView.TouchManager.PushClickRect(new TouchRect(0, BoardConstants.TopAreaHeight, BoardConstants.TotalWidth, Layout.Height - BoardConstants.TopAreaHeight, touchBottom));


            int charWidth = 210;
            int charHeight = 200;
            for (int index = 0; index < Positions.CharacterPositions.Length; index++)
            {
                Point characterPosition = Positions.CharacterPositions[index];
                TouchManager.PushClickRect(new TouchRect(
                    characterPosition.X - charWidth / 2 + 0,
                    characterPosition.Y - charHeight / 2 + (BoardConstants.TopAreaHeight),
                    charWidth,
                    charHeight,
                    selectCharacter,
                    state: GameService.ClassicGameState.Characters[index]));
            }


            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.InitLayoutView(TouchManager);
            }


            if (!Client.UserPreferences.GetValueOrDefault("HasSeenTutorial", false))
            {
                Client.UserPreferences.AddOrUpdateValue("HasSeenTutorial", true);
                State.ShowingTutorial = 1;
            }
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


            GameService.ClassicGameState.Board.Render(elapsedGameTime, MainLayer, (layer, x, y) =>
            {
                switch (GameService.ClassicGameState.GameState)
                {
                    case GameSelectionState.PlayerPlace:
                        if (x == State.CurrentCharacter.X && y == State.CurrentCharacter.Y)
                        {
                            MainLayer.DrawImage(
                                Assets.Images.Character.Box.CharacterBox[ State.CurrentCharacter.CharacterNumber + 1],
                                State.CurrentCharacter.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                State.CurrentCharacter.Y * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                BoardConstants.SquareSize,
                                BoardConstants.SquareSize,
                                true);

                            return;
                        }
                        if (State.CurrentCharacter.Y >= BoardConstants.SquareHeight / 2 && x == State.CurrentCharacter.X && y == State.CurrentCharacter.Y + 1)
                        {
                            MainLayer.DrawImage(
                                Assets.Images.Character.Arrow.PlaceCharacterArrow[State.CurrentCharacter.CharacterNumber + 1],
                                State.CurrentCharacter.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                (State.CurrentCharacter.Y - 1) * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                BoardConstants.SquareSize,
                                BoardConstants.SquareSize,
                                true);

                            return;
                        }
                        if (State.CurrentCharacter.Y < BoardConstants.SquareHeight / 2 && x == State.CurrentCharacter.X && y == State.CurrentCharacter.Y - 1)
                        {
                            MainLayer.Save();
                            MainLayer.SetDrawingEffects(DrawingEffects.FlipVertically);
                            MainLayer.DrawImage(
                                Assets.Images.Character.Arrow.PlaceCharacterArrow[State.CurrentCharacter.CharacterNumber + 1],
                                State.CurrentCharacter.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                (State.CurrentCharacter.Y + 1) * BoardConstants.SquareSize + BoardConstants.SquareSize / 2,
                                BoardConstants.SquareSize,
                                BoardConstants.SquareSize,
                                true);
                            MainLayer.Restore();
                        }
                        break;
                }
            });

            MainLayer.Save();
            MainLayer.Translate(BoardConstants.SideOffset, BoardConstants.TopOffset);
            switch (GameService.ClassicGameState.GameState)
            {
                case GameSelectionState.PlayerPlace:
                    if (State.CurrentCharacter.X > -1)
                    {
                        IImage image = Assets.Images.Character.Stationary.Big.StationaryCharacters[State.CurrentCharacter.CharacterNumber + 1];
                        if (State.CurrentCharacter.Y < BoardConstants.SquareHeight / 2)
                        {
                            MainLayer.DrawImage(image, State.CurrentCharacter.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2, (State.CurrentCharacter.Y + 2) * BoardConstants.SquareSize + BoardConstants.SquareSize / 2 + BoardConstants.SquareSize / 2, 180 * Math.PI / 180, true);
                        }
                        else
                        {
                            MainLayer.DrawImage(image, State.CurrentCharacter.X * BoardConstants.SquareSize + BoardConstants.SquareSize / 2, (State.CurrentCharacter.Y - 2) * BoardConstants.SquareSize + BoardConstants.SquareSize / 2 - BoardConstants.SquareSize / 2, true);
                        }
                    }
                    break;
            }
            MainLayer.Restore();


            if (GameService.ClassicGameState.GameState == GameSelectionState.PlayerChoose || GameService.ClassicGameState.GameState == GameSelectionState.PlayerPlace)
            {
                drawBottomChoosePlayers();
            }


            MainLayer.Restore();
            if (State.ShowingTutorial > 0 && State.ShowingTutorial < 3)
            {
                MainLayer.DrawImage(Assets.Images.Layouts.Tutorials.Tutorial[State.ShowingTutorial], Positions.TutorialPosition, true);
            }


            TouchManager.Render(MainLayer);

            MainLayer.End();
        }


        private bool wholeBoardClick(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            switch (eventtype)
            {
                case TouchType.TouchUp:
                    if (collide)
                    {
                        if (State.ShowingTutorial > 2)
                        {
                            State.ShowingTutorial = 0;
                            return false;
                        }
                    }
                    break;
                case TouchType.TouchDown:
                    if (collide)
                    {
                        if (State.ShowingTutorial == 1 || State.ShowingTutorial == 2)
                        {
                            State.ShowingTutorial++;
                            Client.PlaySoundEffect(Assets.Sounds.Click);

                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        private bool selectCharacter(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.ShowingTutorial > 0) return true;

            switch (eventtype)
            {
                case TouchType.TouchDown:


                    foreach (Character chosenCharacter in GameService.ClassicGameState.Characters)
                    {
                        chosenCharacter.Selected = false;
                    }


                    var currentCharacter = ((Character)touchbox.State);
                    if (currentCharacter.Playing)
                    {
                        Client.PlaySoundEffect(Assets.Sounds.Click);
                        State.CurrentCharacter = null;
                        GameService.ClassicGameState.GameState = GameSelectionState.PlayerChoose;
                        return false;
                    }
                    switch (GameService.ClassicGameState.GameState)
                    {
                        case GameSelectionState.PlayerChoose:

                            Client.PlaySoundEffect(Assets.Sounds.Click);
                            State.CurrentCharacter = currentCharacter;
                            State.CurrentCharacter.X = -1;
                            GameService.ClassicGameState.GameState = GameSelectionState.PlayerPlace;
                            State.CurrentCharacter.Selected = true;
                            break;
                        case GameSelectionState.PlayerPlace:
                            Client.PlaySoundEffect(Assets.Sounds.Click);

                            State.CurrentCharacter.Selected = false;
                            if (State.CurrentCharacter == touchbox.State)
                            {
                                GameService.ClassicGameState.GameState = GameSelectionState.PlayerChoose;
                                State.CurrentCharacter.Selected = false;
                                State.CurrentCharacter = null;
                                return false;
                            }
                            State.CurrentCharacter = currentCharacter;
                            State.CurrentCharacter.Selected = true;
                            State.CurrentCharacter.X = -1;
                            break;
                    }


                    break;
            }
            return false;
        }


        private bool touchBottom(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (State.ShowingTutorial > 0) return true;

            return true;
        }


        private bool squareTouch(int boxX, int boxY, TouchType eventtype, TouchRect touchbox, int touchX, int touchY, bool collide)
        {
            if (State.ShowingTutorial > 0) return true;

            switch (GameService.ClassicGameState.GameState)
            {
                case GameSelectionState.PlayerPlace:
                    switch (eventtype)
                    {
                        case TouchType.TouchUp:

                            if (collide)
                            {
                                if (State.CurrentCharacter.X == -1) return false;
                                if (GameService.ClassicGameState.Board.getItemsOnBoard(null, State.CurrentCharacter.X, State.CurrentCharacter.Y).Any(a => a is SolidWallPiece || a is GoalPiece || a is PlayerPiece))
                                    return false;
                                Client.PlaySoundEffect(Assets.Sounds.Click);

                                State.CurrentCharacter.Selected = false;

                                GameService.ClassicGameState.Board.AddPlayer(State.CurrentCharacter.X, State.CurrentCharacter.Y, State.CurrentCharacter.CharacterNumber);

                                State.CurrentCharacter.Playing = true;

                                GameService.ClassicGameState.GameState = GameSelectionState.PlayerChoose;
                                State.CurrentPlayerChoosing++;
                                if (State.CurrentPlayerChoosing == GameService.ClassicGameState.NumberOfPlayers)
                                {
                                    var cs = new List<Character>();
                                    for (int i = 0; i < 6; i++)
                                    {
                                        while (true)
                                        {
                                            int rx = RandomUtil.RandomInt(0, BoardConstants.SquareWidth);
                                            int ry = RandomUtil.RandomInt(0, BoardConstants.SquareHeight);
                                            if (!GameService.ClassicGameState.Board.getItemsOnBoard(null, rx, ry).Any(a => a is SolidWallPiece || a is GoalPiece || a is PlayerPiece))
                                            {
                                                foreach (Character chosenCharacter in GameService.ClassicGameState.Characters)
                                                {
                                                    if (!chosenCharacter.Playing)
                                                    {
                                                        cs.Add(chosenCharacter);
                                                        chosenCharacter.Playing = true;

                                                        GameService.ClassicGameState.Board.AddPlayer(rx, ry, chosenCharacter.CharacterNumber);
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    foreach (Character c in cs)
                                    {
                                        c.Selected = false;
                                        c.Playing = false;
                                    }

                                    GameService.ClassicGameState.GameState = GameSelectionState.PickCard;
                                    ScreenTransitioner.ChangeToBoardCardSelectionScreen();
                                }
                            }

                            break;
                        case TouchType.TouchMove:

                            if (
                                !GameService.ClassicGameState.Board.getItemsOnBoard(null, boxX, boxY)
                                    .Any(a => a is SolidWallPiece || a is GoalPiece || a is PlayerPiece))
                            {
                                State.CurrentCharacter.X = boxX;
                                State.CurrentCharacter.Y = boxY;
                            }
                            break;
                    }

                    break;
            }
            return false;
        }


        public void Tick(TimeSpan elapsedGameTime)
        {
            GameService.ClassicGameState.Board.Tick(elapsedGameTime);
            foreach (var animatedCharacterSubLayout in CharacterAnimations)
            {
                animatedCharacterSubLayout.Value.TickLayoutView(elapsedGameTime);
            }
        }


        private void drawBottomChoosePlayers()
        {
            MainLayer.Save();
            MainLayer.Translate(0, BoardConstants.TopAreaHeight);

            MainLayer.DrawImage(Assets.Images.Layouts.StagingArea, 0, 0);

            MainLayer.DrawImage(Assets.Images.Layouts.TextBoard, Positions.PlaceYourCharacterPosition, true);

            switch (GameService.ClassicGameState.GameState)
            {
                case GameSelectionState.PlayerChoose:
                    MainLayer.DrawString((Assets.Fonts.BabyDoll._72), string.Format("Player {0}, Choose Your Penguin", (State.CurrentPlayerChoosing + 1)), Positions.PlaceYourCharacterPosition);
                    break;
                case GameSelectionState.PlayerPlace:
                    MainLayer.DrawString((Assets.Fonts.BabyDoll._72), string.Format("Player {0}, Place Your Penguin", (State.CurrentPlayerChoosing + 1)), Positions.PlaceYourCharacterPosition);
                    break;
            }

            for (int i = 0; i < 6; i++)
            {

                Character character = GameService.ClassicGameState.Characters[i];
                var realCharacterNumber = i + 1;
                if (character.Playing)
                {
                    MainLayer.DrawImage(Assets.Images.Character.Placement.CharacterPlacement[ realCharacterNumber], Positions.CharacterShadowPosition[i].X, Positions.CharacterShadowPosition[i].Y, true);
                }
                else
                {
                    MainLayer.DrawImage(Assets.Images.Character.CharacterShadow, Positions.CharacterShadowPosition[i].X, Positions.CharacterShadowPosition[i].Y, true);


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


                }
            }
            MainLayer.Restore();
        }
    }


    public class BoardSelectionAreaLayoutState
    {
        public BoardSelectionAreaLayoutState(BaseLayout layout)
        {
            Layout = layout;
        }

        public BaseLayout Layout { get; set; }
        public int CurrentPlayerChoosing { get; set; }
        public Character CurrentCharacter { get; set; }
        public int ShowingTutorial { get; set; }
    }

    public class BoardSelectionAreaLayoutStatePositions
    {
        public Color DarkColor = new Color(41, 40, 42);

        public BoardSelectionAreaLayoutStatePositions(BaseLayout layout)
        {
            Layout = layout;

            PlaceYourCharacterPosition = new Point(Layout.Width / 2, 109);
            CharacterShadowPosition = new Point[6];
            CharacterPositions = new Point[6];
            CharacterScorePositions = new Point[6];

            for (int i = 0; i < 6; i++)
            {
                CharacterPositions[i] = new Point(215 + 223 * i, 332);
                CharacterScorePositions[i] = new Point(195 + 223 * i, 445);
                CharacterShadowPosition[i] = new Point(215 + 223 * i, 424);
            }

            BackLayout = new BoardAreaLayoutStateBackPositions(layout);
            TutorialPosition = new Point(768, 1024);
        }

        public Point TutorialPosition { get; set; }
        public Point[] CharacterScorePositions { get; set; }
        public Point[] CharacterPositions { get; set; }
        public Point[] CharacterShadowPosition { get; set; }
        public Point PlaceYourCharacterPosition { get; set; }
        public BaseLayout Layout { get; set; }
        public BoardAreaLayoutStateBackPositions BackLayout { get; set; }
    }


    public class BoardAreaLayoutStateBackPositions
    {
        public BoardAreaLayoutStateBackPositions(BaseLayout layout)
        {
            Layout = layout;

            BackBoxPosition = new Point(BoardConstants.TotalWidth / 2, 770);

            BackBoxSize = new Point(1024, 768);
            BackContinueBoxSize = new Point(BackBoxSize.X / 2 - 1, 250);
            BackCancelBoxSize = new Point(BackBoxSize.X / 2 - 1, 250);

            BackTextPosition = new Point(BackBoxSize.X / 2, BackBoxSize.X / 4);


            BackContinueButtonPosition = new Point(0, BackBoxSize.Y - BackContinueBoxSize.Y);
            BackCancelButtonPosition = new Point(BackBoxSize.X / 2 + 1, BackBoxSize.Y - BackCancelBoxSize.Y);
        }

        public Point BackContinueButtonPosition { get; set; }
        public Point BackCancelButtonPosition { get; set; }
        public Point BackBoxPosition { get; set; }
        public Point BackContinueBoxSize { get; set; }
        public Point BackCancelBoxSize { get; set; }
        public Point BackBoxSize { get; set; }
        public Point BackTextPosition { get; set; }
        public BaseLayout Layout { get; set; }
    }
}