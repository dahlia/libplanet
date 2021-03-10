using GraphQL.Language.AST;
using Libplanet.Explorer.GraphTypes;
using Xunit;

namespace Libplanet.Explorer.Tests.GraphTypes
{
    public class ByteStringTypeTest : ScalarGraphTypeTestBase<ByteStringType>
    {
        [Theory]
        [InlineData(new byte[0], "")]
        [InlineData(new byte[] { 0xbe, 0xef }, "beef")]
        public void Serialize(byte[] bytes, string expected)
        {
            Assert.Equal(expected, _type.Serialize(bytes));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("beef", new byte[] { 0xbe, 0xef })]
        public void ParseValue(object value, object parsed)
        {
            Assert.Equal(parsed, _type.ParseValue(value));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("beef", new byte[] { 0xbe, 0xef })]
        public void ParseLiteral(string stringValue, object parsed)
        {
            Assert.Equal(parsed, _type.ParseLiteral(new StringValue(stringValue)));
        }

        [Fact]
        public void ParseLiteral_NotStringValue_ReturnNull()
        {
            Assert.Null(_type.ParseLiteral(new IntValue(0)));
            Assert.Null(_type.ParseLiteral(new BigIntValue(0)));
            Assert.Null(_type.ParseLiteral(new EnumValue("NAME")));
        }
    }
}
