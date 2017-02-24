using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionTree
{
    /// <summary>
    /// A transaction that always gives the same state when <see cref="Apply(T)"/> is called.
    /// <para/>
    /// Best used as the transaction at the start of a chain.
    /// </summary>
    /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
    public sealed class FixedStateTransaction<T> : ITransaction<T>
    {
        /// <summary>
        /// Gets the set of transactions coming after this.
        /// <para/>
        /// If empty, this is currently the end of the transaction chain.
        /// </summary>
        public HashSet<ITransaction<T>> Next { get; } = new HashSet<ITransaction<T>>();

        /// <summary>
        /// Gets or sets the transaction coming before this.
        /// <para/>
        /// If it comes after any other transactions, it's going to reset the state to whatever was defined at construction.
        /// <para/>
        /// If null, this is currently the first transaction in the chain.
        /// </summary>
        public ITransaction<T> Previous { get; set; }

        /// <summary>
        /// Gets the state that will be returned when <see cref="Apply(T)"/> is called.
        /// </summary>
        public T State { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="FixedStateTransaction{T}"/> class with the given state.
        /// </summary>
        /// <param name="state">The state that will be returned when <see cref="Apply(T)"/> is called.</param>
        public FixedStateTransaction(T state)
        {
            State = state;
        }

        public T Apply(T oldT)
        {
            return State;
        }
    }
}