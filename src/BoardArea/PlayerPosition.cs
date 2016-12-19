namespace PenguinShuffle.BoardArea
{
    public class PlayerPosition
    {
        public PlayerPosition(Player character, int x, int y, Direction direction)
        {
            Character = character;
            X = x;
            Y = y;
            Direction = direction;
        }

        public Player Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
    }
}