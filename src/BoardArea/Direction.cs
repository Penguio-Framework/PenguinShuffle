using System;

namespace PenguinShuffle.BoardArea
{
    [Flags]
    public enum Direction
    {
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }
}