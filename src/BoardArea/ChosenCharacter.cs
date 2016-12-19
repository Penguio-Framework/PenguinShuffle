namespace PenguinShuffle.BoardArea
{
    public class Character
    {
        public Character(int characterNumber)
        {
            Playing = false;
            CharacterNumber = characterNumber;
        }

        public int CharacterNumber { get; set; }
        public int Score { get; set; }
        public bool Playing { get; set; }

        public bool Selected { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}