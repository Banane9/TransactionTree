using System.Collections.Generic;

namespace TransactionTree
{
    /// <summary>
    /// The common interface for transactions.
    /// </summary>
    /// <typeparam name="T">The type that the transactions can be applied to.</typeparam>
    public interface ITransaction<T>
    {
        /// <summary>
        /// Gets the set of transactions coming after this.
        /// <para/>
        /// If empty, this is currently the end of the transaction chain.
        /// Should never be null.
        /// </summary>
        HashSet<ITransaction<T>> Next { get; }

        /// <summary>
        /// Gets or sets the transaction coming before this.
        /// <para/>
        /// If null, this is currently the first transaction in the chain.
        /// </summary>
        ITransaction<T> Previous { get; set; }

        /// <summary>
        /// Applies this transaction to the state.
        /// </summary>
        /// <param name="oldT">The state before this transaction.</param>
        /// <returns>The state after this transaction.</returns>
        T Apply(T oldT);
    }
}