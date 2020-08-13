using System;
using System.Numerics;
using System.Security.Cryptography;
using Libplanet.Action;

namespace Libplanet.Blockchain
{
    /// <summary>
    /// A delegate to be called when <see cref="BlockChain{T}.GetBalance"/> method encounters
    /// a block having incomplete dirty states. <see cref="BlockChain{T}.GetBalance"/> method
    /// returns this delegate's return value instead for such case.
    /// </summary>
    /// <typeparam name="T">An <see cref="IAction"/> type.  It should match
    /// to <see cref="BlockChain{T}"/>'s type parameter.</typeparam>
    /// <param name="blockChain">The blockchain to query.</param>
    /// <param name="blockHash">The hash of a block to lacks its dirty states.</param>
    /// <param name="address">The account to query its balance.</param>
    /// <param name="currency">The currency to query.</param>
    /// <param name="enterWriteMode">An optional function to enter the <em>write mode</em> and
    /// returns an <see cref="IDisposable"/> to exit it.  If the delegate tries to update
    /// the <paramref name="blockChain"/> such operation has to be done in the <em>write mode</em>
    /// in order to prevent race condition.</param>
    /// <returns>A complement balance value.</returns>
    /// <seealso cref="FungibleAssetStateCompleters{T}"/>
    public delegate BigInteger FungibleAssetStateCompleter<T>(
        BlockChain<T> blockChain,
        HashDigest<SHA256> blockHash,
        Address address,
        Currency currency,
        Func<IDisposable> enterWriteMode
    )
        where T : IAction, new();
}
