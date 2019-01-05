using Libplanet.Base;
using Libplanet.Crypto;
using Xunit;

namespace Libplanet.Tests
{
    public class AddressExtensionTest
    {
        [Fact]
        public void CanGetAddress()
        {
            PublicKey key = new PublicKey(ByteUtils.ParseHex(
                "03438b935389a7ebf838b3ae4125bd28506aa2dd457f20afc843729d3e7d60d728"));
            Assert.Equal(
                new Address(
                    ByteUtils.ParseHex(
                        "d41fadf61badf5be2de60e9fc3230c0a8a4390f0")),
                key.ToAddress()
            );
        }
    }
}
