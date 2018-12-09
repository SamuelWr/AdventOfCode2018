using System;
using Xunit;

namespace ExampleInputs
{
    public class Day9Tests
    {
        [Theory]
        [InlineData(9, 25, 32)]
        [InlineData(10, 1618, 8317)]
        [InlineData(13, 7999, 146373)]
        [InlineData(17, 1104, 2764)]
        [InlineData(21, 6111, 54718)]
        [InlineData(30, 5807, 37305)]

        public void A(int numPlayers, int maxMarble, long maxScore)
        {
            long result = Day9.Program.A(numPlayers: numPlayers, maxMarble: maxMarble);

            Assert.Equal(maxScore, result);
        }
    }
}
