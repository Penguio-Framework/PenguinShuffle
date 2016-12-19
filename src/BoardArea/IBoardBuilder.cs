namespace PenguinShuffle.BoardArea
{
    public interface IBoardBuilder
    {
        void BuildBoard(Board board);
        void AddPlayer(int x, int y, int characterNumber);
    }
}