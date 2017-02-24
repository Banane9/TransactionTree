using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionTree
{
    /// <summary>
    /// Represents a generic transaction, that takes a lambda for the change.
    /// </summary>
    /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
    public class Transaction<T> : ITransaction<T>
    {
        private readonly Func<T, T> apply;

        /// <summary>
        /// Gets the set of transactions coming after this.
        /// <para/>
        /// If empty, this is currently the end of the transaction chain.
        /// </summary>
        public HashSet<ITransaction<T>> Next { get; } = new HashSet<ITransaction<T>>();

        /// <summary>
        /// Gets or sets the transaction coming before this.
        /// <para/>
        /// If null, this is currently the first transaction in the chain.
        /// </summary>
        public ITransaction<T> Previous { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Transaction{T}"/> class with the given change.
        /// </summary>
        /// <param name="apply">The function that will be applied to get from the previous state to the new state.</param>
        public Transaction(Func<T, T> apply)
        {
            this.apply = apply;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Transaction{T}"/> class with the given change.
        /// </summary>
        /// <param name="apply">The function that will be applied to get from the previous state to the new state.</param>
        /// <param name="previousTransaction">The previous transaction to link this one up with.</param>
        public Transaction(Func<T, T> apply, ITransaction<T> previousTransaction)
            : this(apply)
        {
            Previous = previousTransaction;

            previousTransaction?.Next.Add(this);
        }

        /// <summary>
        /// Applies this transaction to the state.
        /// </summary>
        /// <param name="oldT">The state before this transaction.</param>
        /// <returns>The state after this transaction.</returns>
        public T Apply(T oldT)
        {
            return apply(oldT);
        }
    }
}