using Libplanet.Base;
using Xunit;

namespace Libplanet.Tests
{
    public class ByteUtilsTest
    {
        [Fact]
        public void HexTest()
        {
            var bs = new byte[20]
            {
                0x45, 0xa2, 0x21, 0x87, 0xe2, 0xd8, 0x85, 0x0b, 0xb3, 0x57,
                0x88, 0x69, 0x58, 0xbc, 0x3e, 0x85, 0x60, 0x92, 0x9c, 0xcc,
            };
            Assert.Equal(
                "45a22187e2d8850bb357886958bc3e8560929ccc",
                ByteUtils.Hex(bs));
        }

        [Fact]
        public void ParseTest()
        {
            const string hex = "45a22187e2d8850bb357886958bc3e8560929ccc";
            Assert.Equal(
                new byte[20]
                {
                    0x45, 0xa2, 0x21, 0x87, 0xe2, 0xd8, 0x85, 0x0b, 0xb3, 0x57,
                    0x88, 0x69, 0x58, 0xbc, 0x3e, 0x85, 0x60, 0x92, 0x9c, 0xcc,
                },
                ByteUtils.ParseHex(hex)
            );
        }

        [Fact]
        public void CanCalculateHashCode()
        {
            var bytes = new byte[20]
            {
                0x45, 0xa2, 0x21, 0x87, 0xe2, 0xd8, 0x85, 0x0b, 0xb3, 0x57,
                0x88, 0x69, 0x58, 0xbc, 0x3e, 0x85, 0x60, 0x92, 0x9c, 0xcc,
            };

            Assert.Equal(465595541, ByteUtils.CalculateHashCode(bytes));

            var otherBytes = TestUtils.GetRandomBytes(20);
            otherBytes[19] = 0xdd;

            Assert.NotEqual(
                ByteUtils.CalculateHashCode(bytes),
                ByteUtils.CalculateHashCode(otherBytes)
            );
        }
    }
}
