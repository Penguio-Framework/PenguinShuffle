using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using Engine.Interfaces;
using PenguinShuffle.BoardArea;
using PenguinShuffle.SettingsArea;
using PenguinShuffle.SubLayoutViews;
using PenguinShuffle.Utils;
using Direction = PenguinShuffle.BoardArea.Direction;

namespace PenguinShuffle
{
    public class GameService
    {
        private GameMode gameMode;

        public GameService(AssetManager assetManager)
        {
            AssetManager = assetManager;
            CloudSubLayout = new CloudSubLayout(assetManager);
        }


        public CloudSubLayout CloudSubLayout { get; set; }

        public ClassicGameState ClassicGameState { get; private set; }
        public PuzzleGameState PuzzleGameState { get; private set; }
        public CreateGameState CreateGameState { get; private set; }

        public GameMode GameMode
        {
            get { return gameMode; }
            set
            {
                gameMode = value;
                switch (value)
                {
                    case GameMode.Classic:
                        ClassicGameState = new ClassicGameState();
                        break;
                    case GameMode.Puzzle:
                        PuzzleGameState = new PuzzleGameState();
                        break;
                    case GameMode.Create:
                        CreateGameState = new CreateGameState();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        public AssetManager AssetManager { get; set; }
//        public List<IPurchasableProduct> Products { get; set; }
        public bool HasMultiplayer { get; set; }
    }

    public class PuzzleGameState
    {
        public PuzzleGameState()
        {
            Puzzles = new List<Puzzle>();
            for (int i = 0; i < 30; i++)
            {
                Puzzles.Add(new Puzzle {Locked = true, PuzzleNumber = i + 1, BoardInfo = PuzzleBoardInfo.Deserialize("13;3;7;3|3|2|0;5|3|2|0;5|3|2|0;7|1|1|3")});
            }
            Puzzles[0].Locked = false;
        }

        public List<Puzzle> Puzzles { get; set; }
        public Puzzle CurrentPuzzle { get; set; }

        public void SetPuzzle(AssetManager assetManager, Puzzle puzzle)
        {
            puzzle.Create(assetManager);
            CurrentPuzzle = puzzle;
        }
    }

    public class Puzzle
    {
        public bool Locked { get; set; }
        public int PuzzleNumber { get; set; }
        public Board Board { get; set; }
        public Goal Goal { get; set; }
        public PuzzleBoardInfo BoardInfo { get; set; }
        public int NumberOfMoves { get; set; }

        public void Create(AssetManager assetManager)
        {
            Goal = BoardInfo.Goal;
            NumberOfMoves = BoardInfo.NumberOfMoves;
            Board = new Board(assetManager, null /*todo*/, BoardInfo);
            Board.StartGame();
        }
    }

    public class PuzzleBoardInfo
    {
        public PuzzleBoardInfo(GameService gameService)
        {
            GameService = gameService;
            Squares = new List<PuzzleBoardInfoSquare>();
            Goal = new Goal();
        }

        public GameService GameService { get; set; }

        public List<PuzzleBoardInfoSquare> Squares { get; set; }
        public int NumberOfMoves { get; set; }
        public Goal Goal { get; set; }

        public static string Serialize(PuzzleBoardInfo board)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("{0};{1};", board.Goal.GoalIndex(), board.NumberOfMoves));

            foreach (PuzzleBoardInfoSquare puzzleBoardInfoSquare in board.Squares)
            {
                sb.Append(puzzleBoardInfoSquare.Serialize() + ";");
            }

            return sb.ToString();
        }

        public static PuzzleBoardInfo Deserialize(string board)
        {
            var p = new PuzzleBoardInfo(null /*todo*/);

            string[] sps = board.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            p.Goal = new Goal();
            p.Goal.SetGoalIndex(int.Parse(sps[0]));
            p.NumberOfMoves = int.Parse(sps[1]);

            p.Squares = new List<PuzzleBoardInfoSquare>();

            for (int i = 3; i < sps.Length; i++)
            {
                p.Squares.Add(PuzzleBoardInfoSquare.Deserialize(sps[i]));
            }

            return p;
        }

        public void BuildBoard(Board board)
        {
            board.SquarePieces = new List<ISquarePiece>();

            for (int x = 0; x < BoardConstants.SquareWidth; x++)
            {
                board.SquarePieces.Add(new BorderWallPiece(x, -1));
                board.SquarePieces.Add(new BorderWallPiece(x, BoardConstants.SquareHeight));
            }

            for (int y = 0; y < BoardConstants.SquareHeight; y++)
            {
                board.SquarePieces.Add(new BorderWallPiece(-1, y));
                board.SquarePieces.Add(new BorderWallPiece(BoardConstants.SquareWidth, y));
            }


            foreach (PuzzleBoardInfoSquare puzzleBoardInfoSquare in Squares)
            {
                switch (puzzleBoardInfoSquare.SquareType)
                {
                    case SquareTypes.Goal:
                        //                        board.SquarePieces.Add(new GoalPiece(board.ImageManager, board, puzzleBoardInfoSquare.Position.X, puzzleBoardInfoSquare.Position.Y, GameService));
                        break;
                    case SquareTypes.Player:

                        board.Players.Add(new Player(board.AssetManager, board, int.Parse(puzzleBoardInfoSquare.State) + 1, puzzleBoardInfoSquare.Position.X, puzzleBoardInfoSquare.Position.Y));
                        break;
                    case SquareTypes.SolidWall:
                        board.SquarePieces.Add(new SolidWallPiece(board.AssetManager, board, puzzleBoardInfoSquare.Position.X, puzzleBoardInfoSquare.Position.Y));
                        break;
                    case SquareTypes.Wall:
                        board.SquarePieces.Add(new WallPiece(board.AssetManager, board, puzzleBoardInfoSquare.Position.X, puzzleBoardInfoSquare.Position.Y, (Direction) int.Parse(puzzleBoardInfoSquare.State)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class PuzzleBoardInfoSquare
    {
        public SquareTypes SquareType { get; set; }
        public Point Position { get; set; }
        public string State { get; set; }

        public static string Serialize(ISquarePiece piece)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("{0}|{1}|{2}|", piece.Position.X, piece.Position.Y, piece.Type));
            switch (piece.Type)
            {
                case SquareTypes.Goal:
                    sb.Append(((GoalPiece) piece).Goal.GoalIndex());
                    break;
                case SquareTypes.Player:
                    sb.Append(((PlayerPiece) piece).PlayerNumber);
                    break;
                case SquareTypes.SolidWall:
                    break;
                case SquareTypes.Wall:
                    sb.Append(((WallPiece) piece).Direction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sb.ToString();
        }

        public string Serialize()
        {
            PuzzleBoardInfoSquare piece = this;
            var sb = new StringBuilder();
            sb.Append(string.Format("{0}|{1}|{2}|{3}", piece.Position.X, piece.Position.Y, piece.SquareType, piece.State));
            return sb.ToString();
        }

        public static PuzzleBoardInfoSquare Deserialize(string str)
        {
            var p = new PuzzleBoardInfoSquare();

            string[] sps = str.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            p.Position = new Point(int.Parse(sps[0]), int.Parse(sps[1]));
            p.SquareType = (SquareTypes) int.Parse(sps[2]);
            p.State = sps[3];

            return p;
        }
    }


    public class ClassicGameState
    {
        public GameSelectionState GameState;
        public List<Goal> Goals = new List<Goal>();

        public ClassicGameState()
        {
            Goals = new List<Goal>();
            List<int> items = NumberUtils.Shuffle(NumberUtils.MakeRange(0, 13));

            foreach (int item in items)
            {
                Goals.Add(new Goal {GoalSize = item%2 == 0 ? GoalSize.Small : GoalSize.Big, Used = false, PlayerNumber = (item/2)%6, IsExtra = item == 12});
            }

            Characters = new Character[6];
            for (int i = 0; i < 6; i++)
            {
                Characters[i] = new Character(i);
            }
            ChosenNumbers = new List<ChosenNumber>();
        }

        public Goal[] UnusedGoals
        {
            get { return Goals.Where(a => !a.Used); }
        }

        public Goal CurrentGoal { get; set; }
        public Board Board { get; set; }
        public Character[] Characters { get; set; }
        public List<ChosenNumber> ChosenNumbers { get; set; }
        public long TimeStarted { get; set; }
        public int NumberOfPlayers { get; set; }

        public void CharacterWon(Character character)
        {
            character.Score++;

            CurrentGoal = null;
            ChosenNumbers = new List<ChosenNumber>();
            TimeStarted = 0;
            GameState = GameSelectionState.PickCard;
        }

        public void CharacterLost()
        {
            CurrentGoal = null;
            ChosenNumbers = new List<ChosenNumber>();
            TimeStarted = 0;
            GameState = GameSelectionState.PickCard;
        }
    }

    public class CreateGameState
    {
        public Board Board { get; set; }
    }


    public class ChosenNumber
    {
        public Character Character { get; set; }
        public int Number { get; set; }
        public int OrderChosenIn { get; set; }
    }

    public class Goal
    {
        public int PlayerNumber { get; set; }
        public GoalSize GoalSize { get; set; }
        public bool IsExtra { get; set; }
        public bool Used { get; set; }

        public int GoalIndex()
        {
            if (IsExtra) return 12;
            return (GoalSize == GoalSize.Small ? 0 : 1)*PlayerNumber;
        }

        public void SetGoalIndex(int index)
        {
            if (index == 12)
            {
                IsExtra = true;
                return;
            }

            if (index%2 == 0)
            {
                GoalSize = GoalSize.Small;
            }
            else
            {
                GoalSize = GoalSize.Big;
            }

            PlayerNumber = index/2;
        }
    }

    public enum GoalSize
    {
        Big,
        Small
    }
}