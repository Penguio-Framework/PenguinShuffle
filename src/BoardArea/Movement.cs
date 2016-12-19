namespace PenguinShuffle.BoardArea
{
    public class Movement
    {
        public Movement(Player player, Direction direction)
        {
            Player = player;
            Direction = direction;
        }

        public Player Player { get; set; }
        public Direction Direction { get; set; }
    }
}