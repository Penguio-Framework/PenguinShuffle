namespace PenguinShuffle.BoardArea
{
    public static class BoardConstants
    {
        public static int BoardWidth { get { return BoardConstants.SquareHeight * BoardConstants.SquareSize; } }
        public static int BoardHeight { get { return BoardConstants.SquareWidth * BoardConstants.SquareSize; } }
        public const int SquareWidth = 14;
        public const int SquareHeight = 14;
        public const int SquareSize = 104;
        public const int TopOffset = 40;
        public const int SideOffset = 40;
        public const int BottomOffset = 10;
        public const string EmailAddress = "penguin@omnomapps.com";
        public const string EmailSubject = "Got Feedback?";
        public const string EmailMessage = "";

        public static int TopAreaHeight
        {
            get { return SquareHeight * SquareSize + TopOffset + BottomOffset; }
        }
        public static int TotalWidth
        {
            get
            {
                return 1536;
                return SquareWidth * SquareSize + SideOffset * 2;
            }
        }
        public static int TotalHeight
        {
            get
            {
                return 2048;
            }
        }
    }
}