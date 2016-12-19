using System;
using PenguinShuffle.Utils;

namespace PenguinShuffle.BoardArea
{
    public static class BoardUtils
    {
        public static Direction GetAdjacent(this Direction d)
        {
            bool swap = RandomUtil.RandomBool();
            switch (d)
            {
                case Direction.Left:
                    return swap ? Direction.Top : Direction.Bottom;
                case Direction.Right:
                    return swap ? Direction.Top : Direction.Bottom;

                case Direction.Top:
                    return swap ? Direction.Left : Direction.Right;

                case Direction.Bottom:
                    return swap ? Direction.Left : Direction.Right;
                default:
                    throw new ArgumentOutOfRangeException("d");
            }
        }
    }
}